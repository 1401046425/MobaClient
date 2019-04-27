using Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HeroModel))]
public class HeroMove : MonoBehaviour
{
    private NavMeshAgent HBNavMeshAgent;

    //当前的攻击状态
    private HeroModel Hero_Model;

    public HeroModel Model
    {
        get
        {
            if (Hero_Model == null)
            {
                Hero_Model = GetComponent<HeroModel>();
            }
            return Hero_Model;
        }
    }

    #region 生命周期

    private void Start()
    {
        Model.GetBFManager().JOINNUM++;
        OrinSpeed = Speed;
        HBNavMeshAgent = GetComponent<NavMeshAgent>();
        //添加技能事件侦听
    }

    private void Update()
    {
        SendAxis2Server();
    }

    private void FixedUpdate()
    {
        Model.GetBFRequest().TestPosRequest(Model.PlayerIndex, transform.position);
        Move();
    }

    #endregion 生命周期

    #region 移动

    internal float DirX;
    internal float DirZ;
    public float Speed;
    [HideInInspector] public float OrinSpeed;
    private float Movepara;
    private Joystick joystick;
    private Vector3 moveVector;

    private void SendAxis2Server()
    {
        if (Model.ISME && Model.GetBFManager().JOINNUM == 2)
        {
            if (Model.PlayerIndex == 1)
            {
                moveVector = (Vector3.right * ETCInput.GetAxis("Vertical") + Vector3.back * ETCInput.GetAxis("Horizontal"));
            }
            else if (Model.PlayerIndex == 2)
            {
                moveVector = (Vector3.left * ETCInput.GetAxis("Vertical") + Vector3.forward * ETCInput.GetAxis("Horizontal"));
            }

            if (ETCInput.GetAxis("Vertical") + ETCInput.GetAxis("Horizontal") != Movepara)
            {
                BattleFieldRequest.Instance.MoveRequest(moveVector.x, moveVector.z, transform.position);
                Movepara = ETCInput.GetAxis("Vertical") + ETCInput.GetAxis("Horizontal");
            }
        }
    }//发送输入轴给服务器

    private IEnumerator TestPlayerPos()
    {
        yield return new WaitForSeconds(1);
        BattleFieldRequest.Instance.TestPosRequest(Model.PlayerIndex, transform.position);
        StartCoroutine(TestPlayerPos());
        yield return null;
    }//检测玩家坐标位置

    public void Move()
    {
        //人物移动
        if (Model.Hero_Attack.IsAttacking == true) return;
        else
        {
            if (Model.Hero_Animator.GetBool("EndAttack") == false && DirX != 0 || DirZ != 0)
            {
                Model.Hero_Animator.SetBool("EndAttack", true);

                Model.Hero_Animator.SetTrigger("AttackToRun");
            }
        }
        var NEWPOS = new Vector3(DirX, 0, DirZ);
        if (DirX != 0 || DirZ != 0) transform.rotation = Quaternion.LookRotation(NEWPOS);
        HBNavMeshAgent.velocity = (NEWPOS / Mathf.PI) * Speed;
        Model.Hero_Animator.SetFloat("Speed", NEWPOS.magnitude);
    }//移动角色

    #endregion 移动
}