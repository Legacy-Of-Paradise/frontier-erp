using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;

namespace Content.Shared.Clothing;

[RegisterComponent, NetworkedComponent(), AutoGenerateComponentState]
[Access(typeof(SharedNightVisionSystem))]
public sealed partial class NightVisionComponent : Component
{
    [DataField]
    public EntProtoId ToggleAction = "ActionToggleNightVision";

    [DataField, AutoNetworkedField]
    public EntityUid? ToggleActionEntity;

    [DataField, AutoNetworkedField]
    public bool Enabled;

    [DataField]
    public float Tint1 { get; set; } = 0.3f;

    [DataField]
    public float Tint2 { get; set; } = 0.3f;

    [DataField]
    public float Tint3 { get; set; } = 0.3f;

    public Vector3 Tint
    {
        get => new(Tint1, Tint2, Tint3);
        set
        {
            Tint1 = value.X;
            Tint2 = value.Y;
            Tint3 = value.Z;
        }
    }

    [DataField]
    public float Strength = 2f;

    [DataField]
    public float Noise = 0.5f;

    [DataField]
    public string Slot = "eyes";

    [DataField]
    public SoundSpecifier SoundOn = new SoundPathSpecifier("/Audio/_NewParadise/SoundUse/nvg_on.ogg");

    [DataField]
    public SoundSpecifier SoundOff = new SoundPathSpecifier("/Audio/_NewParadise/SoundUse/nvg_off.ogg");
}
