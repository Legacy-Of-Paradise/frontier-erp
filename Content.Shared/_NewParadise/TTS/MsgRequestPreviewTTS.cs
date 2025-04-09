using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._NewParadise.TTS;

public sealed class MsgRequestPreviewTTS : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.Command;

    public string Text { get; set; } = string.Empty;
    public ProtoId<TTSVoicePrototype> VoiceId { get; set; } = string.Empty;

    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        Text = buffer.ReadString();
        VoiceId = buffer.ReadString();
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(Text);
        buffer.Write(VoiceId);
    }
}
