using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;
using BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform;

[RequireComponent(typeof(HeroModel))]
public class HeroAttack : MonoBehaviour
{
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

    private void Start()
    {
        AddSkillFXListener();
    }

    #region 攻击

    private void AddSkillFXListener()
    {
        var RFXTM = GetComponentInChildren<RFX4_TransformMotion>(true);
        var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
        if (tm != null) tm.CollisionEnter += Tm_CollisionEnter;
    }

    public bool IsAttacking = false;
    public IHurtObject Hurttarget;
    public EntityModel AttargetPlayer;
    public GameObject Arrow;
    public GameObject Handposition;
    public int Damage;
    public GameObject Skill1FX;
    public float Distence=5;
   public List<EntityModel> Targetlist=new List<EntityModel>();
    // Start is called before the first frame update
    public Collider[] hitColliders;
    private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
    {
        if (e.Hit.transform.CompareTag("Hero") && e.Hit.transform.GetComponent<HeroModel>().Index != BattleFieldManager.Instance.MyPlayerIndex)
        {
            Debug.Log(e.Hit.transform.name);
            //发伤害请求
            Model.GetBFRequest().HurtRequest(e.Hit.transform.GetComponent<HeroModel>().Index, 20);
        }
        //will print collided object name to the console.
    }

    // Update is called once per frame
    EntityModel GetCloseEnterEntity(Collider[] colliders)
    {
        Targetlist.Clear();
        foreach (var VARIABLE in colliders)
        {
            var EnterTarget = VARIABLE.GetComponent<EntityModel>();

            if (EnterTarget&& EnterTarget.Camp != Hero_Model.Camp&& !EnterTarget.IsDead)
            {
                if (!Targetlist.Contains(EnterTarget))
                {
                    Targetlist.Add(EnterTarget);
                }

            }
        }

        float min_distence = Distence*10;
        EntityModel FinalTarget = null;
        foreach (var item in Targetlist)
        {
            if (item.Index < 10)
            {
                FinalTarget = item;
                break;
            }
            var Dis=  Vector3.Distance(item.transform.position, transform.position);
           if (Dis < min_distence)
           {
               min_distence = Dis;
               FinalTarget = item;
           }
        }

        return FinalTarget;
    }
    public void Attack(AttackType attackType)
    {
        if (IsAttacking == false && Model.Hero_Animator.GetBool("EndAttack") == true)
        {
            hitColliders = Physics.OverlapSphere(transform.position, Distence);
            var Target = GetCloseEnterEntity(hitColliders);
            if (Target == null)return;
            if (Model.ISME) Model.GetBFRequest().AttackRequest(Target.Index, BattleFieldManager.Instance.MyPlayerIndex, attackType);
            
        }
        return;
        if (IsAttacking == false && Model.Hero_Animator.GetBool("EndAttack") == true)
        {
            foreach (var item in Model.GetBFManager().PlayerList)
            {
                if (item.Index != Model.GetBFManager().MyPlayerIndex)
                {
                    var dir = Vector3.Distance(item.transform.position, this.transform.position);
                    if (dir < 7.5f)
                    {
                        if (Model.ISME) Model.GetBFRequest().AttackRequest(item.Index, BattleFieldManager.Instance.MyPlayerIndex, attackType);
                        return;
                    }
                }
            }
        }
        if (IsAttacking == false && Model.Hero_Animator.GetBool("EndAttack") == true)
        {
            foreach (var item in Model.GetBFManager().TowerList)
            {
                var dir = Vector3.Distance(item.transform.position, this.transform.position);
                if (dir < 7.5f)
                {
                    if (Model.ISME) Model.GetBFRequest().AttackRequest(item.Index, Model.GetBFManager().MyPlayerIndex, attackType);
                    return;
                }
            }
        }
    }




    public void AttackAnimation(int HurtPlayerIndex, int AttackPlayerIndex)
    {
        Hurttarget = Model.GetBFManager().GetEntity(HurtPlayerIndex);
        //if (HurtPlayerIndex >= 1000)
        //{
        //    if (Model.GetBFManager().GetTower(HurtPlayerIndex).Camp == Model.GetBFManager().GetEntity(AttackPlayerIndex).Camp) return;
        //    Hurttarget = Model.GetBFManager().GetTower(HurtPlayerIndex);
        //}
        //else
        //{
        //    Hurttarget = Model.GetBFManager().GetEntity(HurtPlayerIndex);
        //}
        AttargetPlayer = Model.GetBFManager().GetEntity(AttackPlayerIndex);
        var HurtObj = Hurttarget as MonoBehaviour;
        transform.LookAt(HurtObj.transform);
        Model.Hero_Animator.SetBool("EndAttack", false);
        Model.Hero_Animator.SetTrigger("Attack");
        IsAttacking = true;
    }

    public void OnAttackAnimation()
    {
        //实例化弓箭
        var AttOBJ_Arr = Instantiate(Arrow, Handposition.transform.position, Quaternion.identity);
        var Aro = AttOBJ_Arr.GetComponent<Arrow>();
        Aro.OwnerIndex = AttargetPlayer.Index;
        Aro.ArrowCamp = AttargetPlayer.Camp;
        Aro.target = Hurttarget;
        IsAttacking = false;
    }

    public void OnAttackAnimationEnd()
    {
        Model.Hero_Animator.SetBool("EndAttack", true);
        Model.Hero_Animator.ResetTrigger("AttackToRun");
        Model.Hero_Animator.ResetTrigger("AttackToIdle");
        Invoke("AttackToIdle", 1);
    }

    public void AttackToIdle()
    {
        Model.Hero_Animator.SetTrigger("AttackToIdle");
        CancelInvoke();
    }

    public void PlaySkill1Animation(int HurtPlayerIndex, int AttackPlayerIndex)
    {
        Hurttarget = Model.GetBFManager().GetEntity(HurtPlayerIndex);
        AttargetPlayer = Model.GetBFManager().GetEntity(AttackPlayerIndex);
        var HurtObj = Hurttarget as MonoBehaviour;
        transform.LookAt(HurtObj.transform);
        Model.Hero_Animator.SetBool("EndAttack", false);
        Model.Hero_Animator.SetTrigger("Skill1");
        IsAttacking = true;
    }

    internal void OnSkill1Animation()
    {
        //实例化Skill1特效
        if (Skill1FX == null) return;
        var HurtObj = Hurttarget as MonoBehaviour;
        Skill1FX.transform.LookAt(HurtObj.transform.position + Vector3.up);
        Skill1FX.SetActive(true);
        IsAttacking = false;
    }

    public void OnSkillAnimationEnd()
    {
        Model.Hero_Animator.SetBool("EndAttack", true);
    }

    #endregion 攻击
}