using Content.Shared.Actions;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared._NewParadise.NVG;
using Content.Shared.Inventory;
using Content.Shared.Item;
using Content.Shared.Toggleable;
using Content.Shared.Verbs;
using Robust.Shared.Containers;

namespace Content.Shared._NewParadise.NVG;

public abstract class SharedLoPNightVisionSystem : EntitySystem
{
    [Dependency] private readonly ClothingSystem _clothing = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedActionsSystem _sharedActions = default!;
    [Dependency] private readonly SharedActionsSystem _actionContainer = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedContainerSystem _sharedContainer = default!;
    [Dependency] private readonly SharedItemSystem _item = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LoPNightVisionComponent, GetVerbsEvent<ActivationVerb>>(AddToggleVerb);
        SubscribeLocalEvent<LoPNightVisionComponent, GetItemActionsEvent>(OnGetActions);
        SubscribeLocalEvent<LoPNightVisionComponent, ToggleLoPNightVisionEvent>(OnToggleLoPNightVision);
        SubscribeLocalEvent<LoPNightVisionComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, LoPNightVisionComponent component, MapInitEvent args)
    {
        _actionContainer.AddAction(uid, ref component.ToggleActionEntity, component.ToggleAction);
        Dirty(uid, component);
    }

    private void OnToggleLoPNightVision(EntityUid uid, LoPNightVisionComponent component, ToggleLoPNightVisionEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        ToggleLoPNightVision(uid, component);
    }

    private void ToggleLoPNightVision(EntityUid uid, LoPNightVisionComponent nightvision)
    {
        nightvision.Enabled = !nightvision.Enabled;

        if (_sharedContainer.TryGetContainingContainer(uid, out var container) &&
            _inventory.TryGetSlotEntity(container.Owner, "eyes", out var entityUid) && entityUid == uid)
            UpdateLoPNightVisionEffects(container.Owner, uid, true, nightvision);

        if (TryComp<ItemComponent>(uid, out var item))
        {
            _item.SetHeldPrefix(uid, nightvision.Enabled ? "on" : null, component: item);
            _clothing.SetEquippedPrefix(uid, nightvision.Enabled ? "on" : null);
        }

        _appearance.SetData(uid, ToggleVisuals.Toggled, nightvision.Enabled);
        OnChanged(uid, nightvision);
        Dirty(uid, nightvision);
    }

    protected virtual void UpdateLoPNightVisionEffects(EntityUid parent, EntityUid uid, bool state, LoPNightVisionComponent? component) { }

    protected void OnChanged(EntityUid uid, LoPNightVisionComponent component)
    {
        _sharedActions.SetToggled(component.ToggleActionEntity, component.Enabled);
    }

    private void AddToggleVerb(EntityUid uid, LoPNightVisionComponent component, GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        ActivationVerb verb = new();
        verb.Text = Loc.GetString("toggle-nightvision-verb-get-data-text");
        verb.Act = () => ToggleLoPNightVision(uid, component);
        args.Verbs.Add(verb);
    }

    private void OnGetActions(EntityUid uid, LoPNightVisionComponent component, GetItemActionsEvent args)
    {
        args.AddAction(ref component.ToggleActionEntity, component.ToggleAction);
    }
}

public sealed partial class ToggleLoPNightVisionEvent : InstantActionEvent {}
