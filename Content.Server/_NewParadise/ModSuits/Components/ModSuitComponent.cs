using Robust.Shared.GameStates;
using Robust.Shared.Timing;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.ModSuits.Components;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class ModSuitComponent : Component
{
    [DataField]
    public bool Powered;

    [DataField]
    public TimeSpan ActivationDelay = TimeSpan.FromSeconds(2);

    [DataField]
    public Dictionary<ModSlot, EntityUid?> Slots = new();

    [DataField]
    public float CurrentHeat;
    [DataField]
    public float MaxHeat = 100f;
    [DataField]
    public float CoolingRate = 5f;

    [DataField]
    public float HeatThreshold = 120f;

    [DataField]
    public EntityUid? BatterySlot;

    public bool IsActivating;
    public float PowerDrainPerSecond;
}

public enum ModSlot
{
    Helmet,
    Chest,
    Arms,
    Legs,
    Backpack,
}
