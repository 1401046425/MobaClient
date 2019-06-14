using Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;

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
        //Model.GetBFRequest().TestPosRequest(Model.Index, transform.position);

    }

    private void FixedUpdate()
    {
        Move();
    }


    #endregion 生命周期

    #region 移动

    internal float DirX;
    internal float DirZ;
    public float Speed;
    [HideInInspector] public float OrinSpeed;
   // private float Movepara;
    private Joystick joystick;
    private Vector3 moveVector;
    private Vector3 MovePos;
    private void SendAxis2Server()
    {
        if (Model.ISME && Model.GetBFManager().JOINNUM == 2)
        {
            if (Model.Index == 1)
            {
                moveVector = (Vector3.right * ETCInput.GetAxis("Vertical") + Vector3.back * ETCInput.GetAxis("Horizontal"));
            }
            else if (Model.Index == 2)
            {
                moveVector = (Vector3.left * ETCInput.GetAxis("Vertical") + Vector3.forward * ETCInput.GetAxis("Horizontal"));
            }
            BattleFieldRequest.Instance.MoveRequest(moveVector.x, moveVector.z, transform.position, BattleFieldManager.Instance.MyPlayerIndex);

            //if (ETCInput.GetAxis("Vertical") + ETCInput.GetAxis("Horizontal") != Movepara)
            //{
            //    Movepara = ETCInput.GetAxis("Vertical") + ETCInput.GetAxis("Horizontal");
            //}
        }
    }//发送输入轴给服务器

    private IEnumerator TestPlayerPos()
    {
        yield return new WaitForSeconds(1);
        BattleFieldRequest.Instance.TestPosRequest(Model.Index, transform.position);
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
        MovePos.x = DirX;
        MovePos.z = DirZ;
        HBNavMeshAgent.velocity = (MovePos/Mathf.PI) * Speed;
        if (DirX != 0 || DirZ != 0) transform.rotation = Quaternion.LookRotation(MovePos);
        Model.Hero_Animator.SetFloat("Speed", MovePos.magnitude);
    }//移动角色

    #endregion 移动
}