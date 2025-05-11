using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Shared.Preferences.Loadouts;

/// <summary>
/// Corresponds to a Job / Antag prototype and specifies loadouts
/// </summary>
[Prototype]
public sealed partial class RoleLoadoutPrototype : IPrototype
{
    /*
     * Separate to JobPrototype / AntagPrototype as they are turning into messy god classes.
     */

    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    /// <summary>
    /// Can the user edit their entity name for this role loadout?
    /// </summary>
    [DataField]
    public bool CanCustomizeName;

    /// <summary>
    /// Should we use a random name for this loadout?
    /// </summary>
    [DataField]
    public ProtoId<LocalizedDatasetPrototype>? NameDataset;

    // Not required so people can set their names.
    /// <summary>
    /// Groups that comprise this role loadout.
    /// </summary>
    public List<ProtoId<LoadoutGroupPrototype>> Groups => ValidatePrototypes();

    //LOP edit start
    [DataField("groups")]
    private List<string> _groups = new List<string>();          //эта дичь нужна для того, чтобы избегать ошибок из-за отсутствия группы спонсорских лодаутов
    private List<ProtoId<LoadoutGroupPrototype>> _sortedGroups = new(); //чтобы не выполнять преобразование каждый раз, лучше будет записать и ссылаться на него

    private List<ProtoId<LoadoutGroupPrototype>> ValidatePrototypes()   //суть в том, что по ТЗ мы должны игнорировать специально убранные прототипы, но делать возможным видеть ошибки при использовании реально несуществующих прототипов
    {
        if (_sortedGroups.Count == 0)
        {
            foreach (var protoid in _groups)
            {
                var moduled = false; //проверка на существование нужного субмодуля
#if LOP
                moduled = true;
#endif
                if (!(protoid.Contains("Sponsor") && !moduled)) //добавляем группу в том случае, если ее возможно добавить
                {
                    _sortedGroups.Add(new ProtoId<LoadoutGroupPrototype>(protoid));
                }
            }
        }
        return _sortedGroups;
    }
    //LOP edit end

    /// <summary>
    /// How many points are allotted for this role loadout prototype.
    /// </summary>
    [DataField]
    public int? Points;
}
