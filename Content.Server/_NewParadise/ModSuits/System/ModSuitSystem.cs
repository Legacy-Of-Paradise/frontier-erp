using System;
using Content.Server.ModSuits.Components;
using Content.Shared.Actions;
using Content.Shared.Popups;
using Content.Shared.Interaction;
using Content.Shared.PowerCell;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.ModSuits;

public sealed class ModSuitSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPowerCellSystem _power = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ModSuitComponent, ActivateInWorldEvent>(OnActivate);
    }

    private void OnActivate(EntityUid uid, ModSuitComponent comp, ActivateInWorldEvent args)
    {
        if (comp.Powered || comp.IsActivating)
            return;

        comp.IsActivating = true;
        Timer.Spawn(comp.ActivationDelay, () =>
        {
            comp.Powered = true;
            comp.IsActivating = false;
            _popup.PopupEntity("modular-suit-on", uid);
            _audio.PlayPvs("/Audio/Machines/suit_activate.ogg", uid);
        });
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var entity in EntityQuery<ModSuitComponent>())
        {
            var uid = entity.Owner;
            var comp = entity;

            if (!comp.Powered)
                continue;

            float totalDrain = 0f;
            float totalHeat = 0f;
            float totalCooling = comp.CoolingRate;

            foreach (var slot in comp.Slots)
            {
                var moduleUid = slot.Value;
                if (moduleUid is not { } mid || !EntityManager.EntityExists(mid))
                    continue;

                if (!TryComp<ModSuitModuleComponent>(mid, out var mod))
                    continue;

                if (!DependenciesMet(mod, comp))
                    continue;

                totalDrain += mod.PowerUsage;
                totalHeat += mod.HeatGeneration;
            }

            comp.PowerDrainPerSecond = totalDrain;
            if (comp.BatterySlot is { } batt && _power.HasCharge(batt) && _power.TryDrawPower(batt, totalDrain * frameTime))
            {
                comp.CurrentHeat += (totalHeat - totalCooling) * frameTime;
            }
            else
            {
                comp.Powered = false;
                _popup.PopupEntity("modular-suit-power-off", uid);
                continue;
            }

            if (comp.CurrentHeat >= comp.HeatThreshold)
            {
                TriggerOverheat(uid);
                comp.CurrentHeat = comp.MaxHeat;
            }
            else if (comp.CurrentHeat > 0)
            {
                comp.CurrentHeat -= comp.CoolingRate * frameTime;
                if (comp.CurrentHeat < 0) comp.CurrentHeat = 0;
            }
        }
    }

    private bool DependenciesMet(ModSuitModuleComponent mod, ModSuitComponent suit)
    {
        foreach (var req in mod.RequiredSlots)
        {
            if (!suit.Slots.TryGetValue(req, out var ent) || ent == null)
                return false;
        }
        return true;
    }

    private void TriggerOverheat(EntityUid uid)
    {
        _popup.PopupEntity("critical-overheat", uid);
        _audio.PlayPvs("/Audio/Effects/overheat.ogg", uid);
    }
}
