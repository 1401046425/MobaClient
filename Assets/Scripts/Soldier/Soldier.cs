using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : EntityModel
{
    internal NavMeshAgent navMeshAgent;

    public List<GameObject> FinalTargetList;
    private GameObject LastTarget;
    public List<GameObject> CurTargetList;
    private Animator Soldier_Anim;
    public float DirX;
    public float DirZ;
    private bool ISEnemySoldier;
    private Vector3 MovePos;
    internal Vector3 Targetpos;
    public void init(List<GameObject> List,BattleFieldCamp camp,int Id,bool ISAuto)
    {
        Index = Id;
        FinalTargetList = List;
        Camp = camp;
        ISEnemySoldier = ISAuto;
        if (ISEnemySoldier)
        {
            Destroy(GetComponent<BehaviorTree>());
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Soldier_Anim = GetComponent<Animator>(); 
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        Soldier_Anim.SetFloat("SoldierMoveDis",navMeshAgent.velocity.magnitude);
        // MoveToTarget(FinalTarget);
        if (ISEnemySoldier)
        {
            MovePos.x = DirX;
            MovePos.z = DirZ;
            navMeshAgent.velocity = MovePos;

        }
        else
        {
            BattleFieldRequest.Instance.MoveRequest(navMeshAgent.velocity.x, navMeshAgent.velocity.y,transform.position,Index);
           // BattleFieldRequest.Instance.TestPosRequest(Index,transform.position);
        }
    }

    internal GameObject GetFinalTarget()
    {
        if (FinalTargetList[0])
        {
            //Debug.Log(FinalTargetList[0].name);
            if (FinalTargetList[0].GetComponent<EntityModel>().IsDead)
                FinalTargetList.Remove(FinalTargetList[0]);
        }
        return FinalTargetList[0];
    }
    internal GameObject GetCurTarget()
    {
        if (CurTargetList.Count == 0) return null;
        if (CurTargetList[0]==null|| CurTargetList[0].GetComponent<EntityModel>().IsDead)
        {
            CurTargetList.Remove(CurTargetList[0]);
        }

        if (CurTargetList.Count == 0) return null;
        return CurTargetList[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        var Entity = other.GetComponent<EntityModel>();
        if(!Entity)return;
        if (Entity.IsDead) return;

        if (Entity.Camp != Camp)
        {
            if(!CurTargetList.Contains(Entity.gameObject))
                CurTargetList.Add(Entity.gameObject);
        }
    }
    internal void PlayAttackAnim()
    {
        Soldier_Anim.SetTrigger("IsAttack");

    }

    internal void PlayDestory()
    {
        Soldier_Anim.SetTrigger("IsDeath");
        BattleFieldManager.Instance.SoldierList.Remove(this);
        IsDead = true;
        Invoke("Death",2f);

    }//当玩家销毁

    public void Death()
    {
        Destroy(this.gameObject);
    }
    public void MoveToTarget(GameObject Target)
    {
        if (Target)
        {
            if (Targetpos != Target.transform.position)
            {
                Targetpos = Target.transform.position;
                navMeshAgent.SetDestination(Target.transform.position);
            }

        }
    }
}