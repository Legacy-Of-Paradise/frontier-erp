using Content.Server._NF.Bank;
using Content.Shared._NF.Bank.Components;
using Content.Shared.Access.Components;
using Content.Shared.GameTicking;
using Content.Shared.Humanoid;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Content.Shared.Roles;
using Robust.Shared.Player;
using Content.Server.Instruments;
using Robust.Shared.Audio.Systems;
using Content.Shared.Popups;


namespace Content.Server.Auto.Salary;

public sealed class AutoSalarySystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly BankSystem _bank = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    private static float _currentTime = 3600f;

    [ValidatePrototypeId<DepartmentPrototype>]
    private const string FrontierDep = "Frontier";
    [ValidatePrototypeId<DepartmentPrototype>]
    private const string SecurityDep = "Security";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        _currentTime -= frameTime;

        if (_currentTime <= 0)
        {
            _currentTime = 3600f;
            ProcessSalary();
        }
    }

    private void OnRoundRestart(RoundRestartCleanupEvent args)
    {
        _currentTime = 3600f;
    }

    private void ProcessSalary()
    {
        var currentTime = EntityQueryEnumerator<HumanoidAppearanceComponent, BankAccountComponent, ActorComponent>();
        while (currentTime.MoveNext(out var uid, out _, out _, out _))
        {
            if (GetDepartment(uid, out var job))
            {
                int salary = GetSalary(job);

                _audio.PlayPvs("/Audio/Effects/Cargo/ping.ogg", uid);
                var message = Loc.GetString("salary-received-popup", ("amount", salary));
                _popup.PopupEntity(message, uid, PopupType.Medium);

                _bank.TryBankDeposit(uid, salary);
            }
        }
    }

    private int GetSalary(string key) => key switch
    {
        // Зарплаты службы аванпоста
        var s when s == Loc.GetString("job-name-sr") => 45000,
        var s when s == Loc.GetString("job-name-stc") => 30000,
        var s when s == Loc.GetString("job-name-security-guard") => 30000,
        var s when s == Loc.GetString("job-name-chef") => 20000,
        var s when s == Loc.GetString("job-name-mail-carrier") => 20000,
        var s when s == Loc.GetString("job-name-janitor") => 20000,
        var s when s == Loc.GetString("job-name-valet") => 20000,
        var s when s == Loc.GetString("job-name-pilot") => 20000,
        // Зарплаты департамента шерифа
        var s when s == Loc.GetString("job-name-sheriff") => 45000,
        var s when s == Loc.GetString("job-name-bailiff") => 30000,
        var s when s == Loc.GetString("job-name-senior-officer") => 30000,
        var s when s == Loc.GetString("job-name-brigmedic") => 25000,
        var s when s == Loc.GetString("job-name-nf-detective") => 30000,
        var s when s == Loc.GetString("job-name-pal") => 25000,
        var s when s == Loc.GetString("job-name-deputy") => 25000,
        var s when s == Loc.GetString("job-name-cadet-nf") => 20000,
        // Зарплаты службы Реанимации
        var s when s == Loc.GetString("job-name-doc") => 50000,
        var s when s == Loc.GetString("job-name-paramedic") => 30000,
        _ => throw new KeyNotFoundException()
    };



    private bool GetDepartment(EntityUid uid, out string job)
    {
        job = string.Empty;
        var idCard = GetIdCard(uid);

        if (idCard is null)
            return false;

        foreach (var departmentProtoId in idCard.JobDepartments)
        {
            if (departmentProtoId == FrontierDep || departmentProtoId == SecurityDep)
            {
                job = idCard.LocalizedJobTitle != null ? idCard.LocalizedJobTitle : string.Empty;
                return true;
            }
        }
        return false;
    }

    private IdCardComponent? GetIdCard(EntityUid uid)
    {
        if (!_inventory.TryGetSlotEntity(uid, "id", out var idUid))
            return null;

        if (EntityManager.TryGetComponent(idUid, out PdaComponent? pda) && pda.ContainedId != null)
        {
            return TryComp<IdCardComponent>(pda.ContainedId, out var idComp) ? idComp : null;
        }
        return EntityManager.TryGetComponent(idUid, out IdCardComponent? id) ? id : null;
    }
}
