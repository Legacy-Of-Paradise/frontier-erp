using Content.Shared._NewParadise.NVG;
using Content.Shared.GameTicking;
using Robust.Client.Player;
using Robust.Client.Graphics;
using Content.Client.Inventory;
using Content.Shared.Inventory.Events;
using Robust.Shared.Audio.Systems;

namespace Content.Client._NewParadise.NVG;

public sealed class LoPNightVisionSystem : SharedLoPNightVisionSystem
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly ILightManager _lightManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    private LoPNightVisionOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRestart);
        SubscribeLocalEvent<LoPNightVisionComponent, GotUnequippedEvent>(OnGotUnequipped);

        _overlay = new(this);
    }

    public LoPNightVisionComponent? GetNightComp()
    {
        var playerUid = EntityUid.Parse(_playerManager.LocalPlayer?.ControlledEntity.ToString());
        var slot = _entityManager.GetComponent<InventorySlotsComponent>(playerUid);
        _entityManager.TryGetComponent<LoPNightVisionComponent>(slot.SlotData["eyes"].HeldEntity, out var nightvision);
        return nightvision;
    }

    protected override void UpdateLoPNightVisionEffects(EntityUid parent, EntityUid uid, bool state, LoPNightVisionComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        state = state && component.Enabled;

        if (state)
        {
            _audio.PlayLocal(component.SoundOn, parent, uid);
            _lightManager.DrawLighting = false;
            _overlayMan.AddOverlay(_overlay);
        }
        else
        {
            _audio.PlayLocal(component.SoundOff, parent, uid);
            _lightManager.DrawLighting = true;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
    private void OnGotUnequipped(EntityUid uid, LoPNightVisionComponent component, GotUnequippedEvent args)
    {
        if (args.Slot == component.Slot)
        {
            _overlayMan.RemoveOverlay(_overlay);
            _lightManager.DrawLighting = true;
        }
    }
    private void OnRestart(RoundRestartCleanupEvent ev)
    {
        _overlayMan.RemoveOverlay(_overlay);
        _lightManager.DrawLighting = true;
    }
}
