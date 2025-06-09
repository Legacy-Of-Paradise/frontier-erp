using Robust.Shared.Configuration;

namespace Content.Shared._CorvaxNext.NextVars;

/// <summary>
///     Corvax modules console variables
/// </summary>
[CVarDefs]
// ReSharper disable once InconsistentNaming
public sealed class NextVars
{
    public static readonly CVarDef<bool> AutoGetUp =
        CVarDef.Create("laying.auto_get_up", true, CVar.CLIENT | CVar.ARCHIVE | CVar.REPLICATED);

    /// <summary>
    ///     When true, entities that fall to the ground will be able to crawl under tables and
    ///     plastic flaps, allowing them to take cover from gunshots.
    /// </summary>
    public static readonly CVarDef<bool> CrawlUnderTables =
        CVarDef.Create("laying.crawlundertables", true, CVar.REPLICATED);
}
