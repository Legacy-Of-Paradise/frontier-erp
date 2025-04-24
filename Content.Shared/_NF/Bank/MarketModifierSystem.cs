using Content.Shared.Examine;
using Content.Shared._NF.Bank.Components;
using Content.Shared.VendingMachines;
// LOP edit start  
using Robust.Shared.Random;  
using Robust.Shared.GameObjects;  
using Robust.Shared.IoC;  
using Robust.Shared.Random;  
// LOP edit end  

namespace Content.Shared._NF.Bank;

public sealed partial class MarketModifierSystem : EntitySystem
{

    /// <summary>
    /// LoP Edit: Start
    /// </summary>

    [Dependency] private readonly IRobustRandom _random = default!;

    private const int MinUpdateMinutes = 5;
    private const int MaxUpdateMinutes = 10;
    private bool update = false;

    /// <summary>
    /// LoP Edit: End
    /// </summary>

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MarketModifierComponent, ExaminedEvent>(OnExamined);

        // LOP edit start

        [Dependency] private readonly IRobustRandom _random = default!;  

        private const int MinUpdateMinutes = 5;  
        private const int MaxUpdateMinutes = 10;  
        private bool update = false;  

        SubscribeLocalEvent<MarketModifierComponent, ComponentInit>(OnComponentInit);  
    }  

    private void OnComponentInit(EntityUid uid, MarketModifierComponent component, ComponentInit args)  
    {  
        #if !DebugOpt 
            component.Mod = _random.NextFloat(component.MinMod, component.MaxMod); 
        #endif 
    }  

    // LOP edit end 


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
