using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._LOP.LOPCCVars;

public sealed partial class LOP_CCVars : CVars
{
    public static readonly CVarDef<bool> ChatIconsEnable =
        CVarDef.Create("chat_icon.enable", true, CVar.CLIENTONLY | CVar.ARCHIVE);
}
