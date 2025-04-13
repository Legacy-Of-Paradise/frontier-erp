namespace Content.Server.ModSuits.Components;

[RegisterComponent]
public sealed partial class ModSuitModuleComponent : Component
{
    [DataField(required: true)]
    public ModSlot Slot;

    [DataField("powerDrain")]
    public float PowerUsage;

    [DataField("heatGen")]
    public float HeatGeneration;

    [DataField("reqSlots")]
    public List<ModSlot> RequiredSlots = new();

    [DataField]
    public bool Malfunctioning;
}
