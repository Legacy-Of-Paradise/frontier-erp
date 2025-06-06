#if LOP
using Content.Shared._ERPModule.Data.Helpers;
#endif

namespace Content.Server._NewParadise._ERP;

[RegisterComponent]
public sealed partial class ERPBounderComponent : Component
{
#if LOP
    [DataField]
    public List<GenitalSlot> CustomGenitals { get; set; } = new();
#endif
}
