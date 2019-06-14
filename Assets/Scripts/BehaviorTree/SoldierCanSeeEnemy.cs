using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SoldierCanSeeEnemy : Conditional
{
    private Soldier soldier;

    public override void OnStart()
    {
        soldier = GetSolider();
    }

    public override TaskStatus OnUpdate()
    {
        if (soldier.IsDead) return TaskStatus.Failure;
        if (soldier.GetCurTarget() == null)           return TaskStatus.Failure;

        return TaskStatus.Success;
    }

    private Soldier GetSolider()
    {
        return GetComponent<Soldier>();
    }
}