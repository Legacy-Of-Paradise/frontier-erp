using System.IO;
using Content.Shared._NewParadise;
using Content.Shared._NewParadise.TTS;
using Robust.Client.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Configuration;

// ReSharper disable InconsistentNaming

namespace Content.Client._NewParadise.TTS;

/// <summary>
/// Plays TTS audio in world
/// </summary>
public sealed class TTSSystem : EntitySystem
{
    [Dependency] private readonly IAudioManager _audioManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly AudioSystem _audioSystem = default!;

    private float _volume;

    private readonly Dictionary<EntityUid, AudioComponent> _currentlyPlaying = new();
    private readonly Dictionary<EntityUid, Queue<AudioStreamWithParams>> _enquedStreams = new();

    // Same as Server.ChatSystem.VoiceRange
    private const float VoiceRange = 10;

    private Entity<AudioComponent>? _currentlyPreviewing;

    public override void Initialize()
    {
        _cfg.OnValueChanged(NewParadiseCvars.TtsVolume, OnTtsVolumeChanged, true);
        SubscribeNetworkEvent<PlayTTSEvent>(OnPlayTTS);
        SubscribeNetworkEvent<PlayPreviewTTSEvent>(OnPlayPreview);
    }

    private void OnPlayPreview(PlayPreviewTTSEvent ev)
    {
        var stream = CreateAudioStream(ev.Data);

        var audioParams = new AudioParams
        {
            Volume = _volume
        };

        _audioSystem.Stop(_currentlyPreviewing);

        _currentlyPreviewing = _audioSystem.PlayGlobal(stream, audioParams);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _cfg.UnsubValueChanged(NewParadiseCvars.TtsVolume, OnTtsVolumeChanged);
        ClearQueues();
    }

    public override void FrameUpdate(float frameTime)
    {
        foreach (var (uid, audioComponent) in _currentlyPlaying)
        {
            if (audioComponent is { Running: true, Playing: true })
            {
                continue;
            }

            if (!_enquedStreams.TryGetValue(uid, out var queue))
            {
                continue;
            }

            if (!queue.TryDequeue(out var toPlay))
            {
                continue;
            }


            var audio = _audioSystem.PlayEntity(toPlay.Stream, uid, toPlay.Params);
            if (!audio.HasValue)
            {
                continue;
            }

            _currentlyPlaying[uid] = audio.Value.Component;
        }
    }

    private void OnTtsVolumeChanged(float volume)
    {
        _volume = volume;
    }

    private void OnPlayTTS(PlayTTSEvent ev)
    {
        PlayTTS(GetEntity(ev.Uid), ev.Data, ev.BoostVolume ? _volume + 5 : _volume);
    }

    public void PlayTTS(EntityUid uid, byte[] data, float volume)
    {
        if (_volume <= -20f)
        {
            return;
        }

        var stream = CreateAudioStream(data);

        var audioParams = new AudioParams
        {
            Volume = volume,
            MaxDistance = VoiceRange
        };

        var audioStream = new AudioStreamWithParams(stream, audioParams);
        EnqueueAudio(uid, audioStream);
    }

    public void StopCurrentTTS(EntityUid uid)
    {
        _audioSystem.Stop(uid);
    }

    private void EnqueueAudio(EntityUid uid, AudioStreamWithParams audioStream)
    {
        if (!_currentlyPlaying.ContainsKey(uid))
        {
            var audio = _audioSystem.PlayEntity(audioStream.Stream, uid, audioStream.Params);

            if (!audio.HasValue)
            {
                return;
            }

            _currentlyPlaying[uid] = audio.Value.Component;

            return;
        }

        if (_enquedStreams.TryGetValue(uid, out var queue))
        {
            queue.Enqueue(audioStream);
            return;
        }

        queue = new Queue<AudioStreamWithParams>();
        queue.Enqueue(audioStream);
        _enquedStreams[uid] = queue;
    }

    private void ClearQueues()
    {
        foreach (var (_, queue) in _enquedStreams)
        {
            queue.Clear();
        }
    }

    private AudioStream CreateAudioStream(byte[] data)
    {
        var dataStream = new MemoryStream(data) { Position = 0 };
        return _audioManager.LoadAudioOggVorbis(dataStream);
    }

    private record AudioStreamWithParams(AudioStream Stream, AudioParams Params);
}
