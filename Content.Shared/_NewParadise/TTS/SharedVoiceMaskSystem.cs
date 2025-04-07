using Robust.Shared.Serialization;

namespace Content.Shared.VoiceMask;

[Serializable, NetSerializable]
public sealed class VoiceMaskChangeVoiceMessage(string voice) : BoundUserInterfaceMessage
{
    public string Voice { get; } = voice;
}
