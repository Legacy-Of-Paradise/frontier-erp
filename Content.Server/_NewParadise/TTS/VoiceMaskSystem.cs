using Content.Server._NewParadise.TTS;
using Content.Shared.Inventory;
using Content.Shared.VoiceMask;

namespace Content.Server.VoiceMask;

public partial class VoiceMaskSystem
{
    private void InitializeTTS()
    {
        SubscribeLocalEvent<VoiceMaskComponent, InventoryRelayedEvent<TransformSpeakerVoiceEvent>>(OnSpeakerVoiceTransform);
        SubscribeLocalEvent<VoiceMaskComponent, InventoryRelayedEvent<VoiceMaskChangeVoiceMessage>>(OnChangeVoice);
    }

    private void OnSpeakerVoiceTransform(EntityUid uid, VoiceMaskComponent component, ref InventoryRelayedEvent<TransformSpeakerVoiceEvent> evt)
    {
        //evt.Args.VoiceId = component.VoiceId;
    }

    private void OnChangeVoice(Entity<VoiceMaskComponent> entity, ref InventoryRelayedEvent<VoiceMaskChangeVoiceMessage> message)
    {
        //entity.Comp.VoiceId = message.Args.Voice;

        _popupSystem.PopupCursor(Loc.GetString("voice-mask-voice-popup-success"), message.Args.Actor);

        UpdateUI(entity);
    }
}
