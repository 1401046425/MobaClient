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
    public Collider[] canseeColliders;
    public float Distence = 10f;
    public float Dis;
    public GameObject Target;
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
        canseeColliders = Physics.OverlapSphere(transform.position, Distence);
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

        if (ISEnemySoldier)
        {
            Target = GetCurTarget();
            Dis = Vector3.Distance(transform.position, Target.transform.position);
            if (Dis < 3)
            {
                transform.LookAt(Target.transform);
                PlayAttackAnim();
            }
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
        if (canseeColliders.Length == 0) return null;
        EntityModel ColliderEntity=null;
        GameObject FinalEntity = null;
        float MinDis=1000;
        foreach (var itemCollider in canseeColliders)
        {
            ColliderEntity = itemCollider.GetComponent<EntityModel>();
            if (ColliderEntity.Camp != Camp)
            {
                if (!ColliderEntity.IsDead)
                {
                   var dis= Vector3.Distance(ColliderEntity.transform.position, transform.position);
                   if (dis < MinDis)
                   {
                       MinDis = dis;
                       FinalEntity = ColliderEntity.gameObject;
                   }
                }

            }
        }
        return FinalEntity;
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