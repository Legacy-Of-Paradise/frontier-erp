using Content.Shared._NewParadise.NVG;
using Content.Shared.Inventory.Events;

namespace Content.Server._NewParadise.NVG;

public sealed class LoPNightVisionSystem : SharedLoPNightVisionSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LoPNightVisionComponent, GotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<LoPNightVisionComponent, GotUnequippedEvent>(OnGotUnequipped);
    }

    private void OnGotUnequipped(EntityUid uid, LoPNightVisionComponent component, GotUnequippedEvent args)
    {
        if (args.Slot == component.Slot)
            UpdateLoPNightVisionEffects(args.Equipee, uid, false, component);
    }

    private void OnGotEquipped(EntityUid uid, LoPNightVisionComponent component, GotEquippedEvent args)
    {
        if (args.Slot == component.Slot)
            UpdateLoPNightVisionEffects(args.Equipee, uid, true, component);
    }
}
