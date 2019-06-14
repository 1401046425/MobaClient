using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MySeek : Action
{
    public SharedGameObject Target;
    public float Speed;
    public float arriveDistence;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        //跟踪目标

        //如果没有目标返回false
        if (Target.Value == null)
        {
            return TaskStatus.Failure;
        }
        transform.LookAt(Target.Value.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, Target.Value.transform.position, Speed * Time.deltaTime);

        //如果有目标朝着目标移动
        //如果距离小于某个值，就返回True
        if ((Target.Value.transform.position - transform.position).sqrMagnitude < arriveDistence)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}