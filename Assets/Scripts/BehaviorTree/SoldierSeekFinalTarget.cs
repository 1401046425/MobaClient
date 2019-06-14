using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SoldierSeekFinalTarget : Action
{
    private Soldier soldier;

    public override void OnStart()
    {
        soldier = GetSolider();
    } 

    public override TaskStatus OnUpdate()
    {
        if (soldier.IsDead) return TaskStatus.Failure;
        var Target = soldier.GetFinalTarget();
        if (Target!=null)
        {
            soldier.navMeshAgent.isStopped = false;
            soldier.MoveToTarget(Target);


            //soldier.MoveToTarget(Target);
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }

    }


    private Soldier GetSolider()
    {
        return GetComponent<Soldier>();
    }
}