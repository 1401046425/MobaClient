using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCanSeeObject : Conditional
{
    public float Distence;

    public Collider[] hitColliders;
    public float ViewDistence;
    public SharedGameObject ReturnObject;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        // Physics.OverlapSphere 检测场景中与投射的球体相交的碰撞器
        hitColliders = Physics.OverlapSphere(transform.position, Distence);
        if (hitColliders.Length <= 0)
        {
            return TaskStatus.Failure;
        }
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Hero"))
            {
                var angle = Vector3.Angle(transform.forward, hitColliders[i].transform.position - transform.position);
                if (angle * 2 < ViewDistence)
                {
                    ReturnObject.Value = hitColliders[i].gameObject;
                    return TaskStatus.Success;
                }
                else
                {
                    ReturnObject.Value = null;
                    Debug.Log(1);
                }
            }
        }
        if (ReturnObject.Value)
        {
            if ((ReturnObject.Value.transform.position - transform.position).magnitude > Distence)
            {
                ReturnObject.Value = null;
            }
        }

        return TaskStatus.Running;
    }
}