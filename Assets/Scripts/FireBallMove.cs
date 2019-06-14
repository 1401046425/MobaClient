using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMove : MonoBehaviour
{
    internal GameObject Target;
    
    //这个变量是一个方法，并且这个方法是具有一个参数gameobject类型
    //委托
    public Action<GameObject> TouchCallBack;

    private float Speed = 5f;

    private float StartTime;
    // Start is called before the first frame update
    private void Start()
    {
        StartTime = Time.time;
    }

    private void FixedUpdate()
    {
        //移动
        //transform.LookAt(FinalTarget.transform.position);
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }
        var Dir = Target.transform.position - transform.position;
        transform.Translate(Dir.normalized * Speed* (Time.time-StartTime)* Time.timeScale* Time.fixedDeltaTime);
        //碰撞检测
        if (Dir.magnitude < 0.3f)
        {
            Destroy(gameObject);
            TouchCallBack(Target);
        }
    }
}