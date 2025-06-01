using Content.Client.UserInterface.Systems.Chat;
using Content.Shared._NewParadise;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;

namespace Content.Client._NewParadise.IconsJob;

public sealed class ChatIconsSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IUserInterfaceManager _uiMan = default!;

    public override void Initialize()
    {
        base.Initialize();

        _cfg.OnValueChanged(NewParadiseCvars.ChatIconsEnable, OnRadioIconsChanged, true);
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _cfg.UnsubValueChanged(NewParadiseCvars.ChatIconsEnable, OnRadioIconsChanged);
    }

    private void OnRadioIconsChanged(bool enable)
    {
        _uiMan.GetUIController<ChatUIController>().Repopulate();
    }
}
