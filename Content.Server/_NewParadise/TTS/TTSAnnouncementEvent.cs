namespace Content.Server._NewParadise.TTS;

// ReSharper disable once InconsistentNaming
public sealed class TTSAnnouncementEvent(string message, string voiceId, EntityUid source, bool global)
    : EntityEventArgs
{
    public readonly string Message = message;
    public readonly bool Global = global;
    public readonly string VoiceId = voiceId;
    public readonly EntityUid Source = source;
}
