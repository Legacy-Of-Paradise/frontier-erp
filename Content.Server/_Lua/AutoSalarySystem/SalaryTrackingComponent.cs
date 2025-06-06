namespace Content.Server._Lua.AutoSalarySystem;

[RegisterComponent]
public sealed partial class SalaryTrackingComponent : Component
{
    [DataField] public EntityUid Station;
    [DataField] public string JobId = string.Empty;
}
