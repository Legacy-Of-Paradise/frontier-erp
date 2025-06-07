using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Client.Clothing;

public sealed class NightVisionOverlay : Overlay
{
    private readonly IPrototypeManager _prototypeManager;
    private readonly NightVisionSystem _nightVisionSystem;

    public override bool RequestScreenTexture => true;
    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _shader;

    public NightVisionOverlay(NightVisionSystem nightVisionSystem)
    {
        IoCManager.InjectDependencies(this);
        _nightVisionSystem = nightVisionSystem;
        _prototypeManager = IoCManager.Resolve<IPrototypeManager>();
        _shader = _prototypeManager.Index<ShaderPrototype>("LoPNightVision").InstanceUnique();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;

        var handle = args.WorldHandle;
        var nightcomp = _nightVisionSystem.GetNightComp();

        if (nightcomp == null)
        {
            Logger.Error("Failed to get night vision component from eyes.");
            return;
        }

        _shader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _shader.SetParameter("tint", nightcomp.Tint);
        _shader.SetParameter("luminance_threshold", nightcomp.Strength);
        _shader.SetParameter("noise_amount", nightcomp.Noise);

        handle.UseShader(_shader);
        handle.DrawRect(args.WorldBounds, Color.White);
        handle.UseShader(null);
    }
}
