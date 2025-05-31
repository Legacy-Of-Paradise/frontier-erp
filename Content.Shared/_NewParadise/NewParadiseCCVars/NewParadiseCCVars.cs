using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._NewParadise.NewParadiseCCVars;

public sealed partial class NewParadise_CCVars : CVars
{
    public static readonly CVarDef<bool> ChatIconsEnable =
        CVarDef.Create("chat_icon.enable", true, CVar.CLIENTONLY | CVar.ARCHIVE);
}
