﻿using ECommons.ExcelServices;
using ECommons.GameHelpers;
using Lumina.Excel.GeneratedSheets;

namespace RotationSolver.Basic.Actions;

public class HealPotionItem : BaseItem
{
    readonly float _percent;
    readonly uint _maxHp;

    public uint MaxHealHp
    {
        get
        {
            if (!Player.Available) return 0;
            return Math.Min((uint)(Player.Object.MaxHp * _percent), _maxHp);
        }
    }

    protected override bool CanUseThis => Service.Config.UseHealPotions;

    public HealPotionItem(Item item, uint a4 = 65535) : base(item, a4)
    {
        var data = _item.ItemAction.Value.DataHQ;
        _percent = data[0] / 100f;
        _maxHp = data[1];
    }

    public override bool CanUse(out IAction item)
    {
        item = null;
        if (!Player.Available) return false;
        var job = (Job)Player.Object.ClassJob.Id;
        if (Player.Object.GetHealthRatio() > job.GetHealSingleAbility() -
            job.GetHealingOfTimeSubtractSingle()) return false;
        if (Player.Object.MaxHp - Player.Object.CurrentHp < MaxHealHp) return false;
        return base.CanUse(out item);
    }
}
