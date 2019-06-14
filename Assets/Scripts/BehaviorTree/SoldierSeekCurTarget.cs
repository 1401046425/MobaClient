using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SoldierSeekCurTarget : Action
{
    private Soldier soldier;
    private float AttCD;
    public override void OnStart()
    {
        soldier = GetSolider();
    }

    public override TaskStatus OnUpdate()
    {
        if(soldier.IsDead) return TaskStatus.Failure;
        var Target = soldier.GetCurTarget();
        if (Target!=null)
        {
            var Dis = Vector3.Distance(transform.position, Target.transform.position);
            if (Dis < 3)
            {
                soldier.navMeshAgent.isStopped = true;
                soldier.transform.LookAt(Target.transform);
                if (Time.time >= AttCD)
                {
                    AttCD = Time.time + 2;

                    Target.GetComponent<EntityModel>().SendHurtRequest(10, soldier.Index);
                    soldier.PlayAttackAnim();
                }

            }
            else
            {
                soldier.navMeshAgent.isStopped = false;
                soldier.MoveToTarget(Target);
                //soldier.navMeshAgent.SetDestination(Target.transform.position);
            }         
            return TaskStatus.Running;
        }

        soldier.navMeshAgent.ResetPath();
        return TaskStatus.Failure;
    }

    private Soldier GetSolider()
    {
        return GetComponent<Soldier>();
    }
}