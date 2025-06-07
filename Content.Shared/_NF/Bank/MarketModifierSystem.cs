using Content.Shared.Examine;
using Content.Shared._NF.Bank.Components;
using Content.Shared.VendingMachines;
using Robust.Shared.Random; // LoP Edit

namespace Content.Shared._NF.Bank;

public sealed partial class MarketModifierSystem : EntitySystem
{

    // LoP Edit: Start

    [Dependency] private readonly IRobustRandom _random = default!;

    // LoP Edit: End

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MarketModifierComponent, ExaminedEvent>(OnExamined);

        // LoP Edit: Start

        SubscribeLocalEvent<MarketModifierComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, MarketModifierComponent component, MapInitEvent args)
    {
        if (component.Mod == null || component.Mod == 0)
        {
            component.Mod = _random.NextFloat(component.MinMod, component.MaxMod);
        }
    }

    // LoP Edit: End

    // This code is licensed under AGPLv3. See AGPLv3.txt
    private void OnExamined(Entity<MarketModifierComponent> ent, ref ExaminedEvent args)
    {
        // If the machine is a vendor, don't print out rates
        if (HasComp<VendingMachineComponent>(ent))
            return;

        string locVerb = ent.Comp.Buy ? "buy" : "sell";
        if (ent.Comp.Mod >= 1.0f)
            args.PushMarkup(Loc.GetString($"market-modifier-{locVerb}-high", ("mod", ent.Comp.Mod)));
        else
            args.PushMarkup(Loc.GetString($"market-modifier-{locVerb}-low", ("mod", ent.Comp.Mod)));
    }
}
