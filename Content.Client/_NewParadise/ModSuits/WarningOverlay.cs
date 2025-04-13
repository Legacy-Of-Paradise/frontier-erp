using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Enums;
using Robust.Client.ResourceManagement;
using System.Numerics;

namespace Content.Client.ModSuits;

public sealed class WarningOverlay : Overlay
{
    private readonly IPlayerManager _playerManager;
    private readonly IGameTiming _timing;
    private readonly Font _font;

    public WarningOverlay(IPlayerManager playerManager, IGameTiming timing, IResourceCache resourceCache)
    {
        _playerManager = playerManager;
        _timing = timing;
        ZIndex = 100;
        _font = resourceCache.GetFont("Resources/Fonts/_NewParadise/Roboto-Bold.ttf", 16);
    }

    public override OverlaySpace Space => OverlaySpace.ScreenSpace;

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.ScreenHandle;
        var viewport = args.Viewport.Size;

        var time = _timing.CurTime.TotalSeconds;
        var phase = MathF.Sin((float)(time * 10.0)) * 0.5f + 0.5f;
        var alpha = (byte)(phase * 255);
        var color = new Color(255, 0, 0, alpha);

        var position = new Vector2(viewport.X / 2f - 100f, 50f);
        handle.DrawString(_font, position, "overheat-warning", color);
    }
}
