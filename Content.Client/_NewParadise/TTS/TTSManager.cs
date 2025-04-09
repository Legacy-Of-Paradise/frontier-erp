using Content.Shared._NewParadise.TTS;
using Robust.Shared.Network;

namespace Content.Client._NewParadise.TTS;

// ReSharper disable once InconsistentNaming
public sealed class TTSManager
{
    [Dependency] private readonly IClientNetManager _netMgr = default!;
    [Dependency] private readonly EntityManager _entityManager = default!;

    public void Initialize()
    {
        _netMgr.RegisterNetMessage<MsgRequestTTS>();
    }

    // ReSharper disable once InconsistentNaming
    public void RequestTTS(EntityUid uid, string text, string voiceId)
    {
        var netEntity = _entityManager.GetNetEntity(uid);

        var msg = new MsgRequestTTS { Text = text, Uid = netEntity, VoiceId = voiceId };

        _netMgr.ClientSendMessage(msg);
    }

    public void RequestPreviewTTS(string text, string voiceId)
    {
        var msg = new MsgRequestPreviewTTS
        {
            Text = text,
            VoiceId = voiceId
        };

        _netMgr.ClientSendMessage(msg);
    }
}
