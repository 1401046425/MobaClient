using System.Collections.Generic;
using UnityEngine;

public class TowerModel : EntityModel
{
    public GameObject Target;
    public GameObject FireFB;
    private GameObject Fire;
    private LineRenderer Line;
    public GameObject Circle;
    public int AttackDamage;
    public List<EntityModel> EnterEntity=new List<EntityModel>();
    // Start is called before the first frame update
    public float Radius;
    public bool isAttacking=false;
    private const int FireBallMaxNumber=1;
    public int FireBallNumber;
    private void Start()
    {
        BattleFieldManager.Instance.TowerList.Add(this);
        Radius = GetComponent<SphereCollider>().radius;
        Line = transform.Find("Line").gameObject.GetComponent<LineRenderer>();
        if (Circle.activeSelf)
        {
            Circle.SetActive(false);
        }
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        if (EnterEntity.Count > 0)
        {
            Target = GetFirstEnterEntity().gameObject;

        }
        if (Target != null&& EnterEntity.Count > 0)
        {

            Line.gameObject.SetActive(true);
            Line.SetPosition(0, transform.position + new Vector3(0, 5.3f, 0));
            Line.SetPosition(1, Target.transform.position + new Vector3(0, 1.4f, 0));
        }
        else
        {
            Line.gameObject.SetActive(false);
        }

        if (EnterEntity.Count >0&&!isAttacking)
        {
            if(FireBallNumber<FireBallMaxNumber)Attack();
        }
    }

    EntityModel GetFirstEnterEntity()
    {
        if (EnterEntity.Count == 0) return null;
        if (EnterEntity[0] == null || EnterEntity[0].IsDead)
        {

            EnterEntity.Remove(EnterEntity[0]);
            if(EnterEntity.Count>0) return GetFirstEnterEntity();
            if (EnterEntity.Count == 0) return null;
        }

        return EnterEntity[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        var EnterINS = other.GetComponent<EntityModel>();
        if (!EnterINS) return;
        if (EnterINS.Camp == Camp) return;
        Circle.SetActive(true);
        if (!EnterEntity.Contains(EnterINS)) EnterEntity.Add(EnterINS);
    }


    private void OnTriggerExit(Collider other)
    {
        var Entity = other.GetComponent<EntityModel>();
        if(!Entity)return;
        if (Entity.Camp != Camp)
        {
            EnterEntity.Remove(Entity);
            foreach (var VARIABLE in EnterEntity)
            {
                if (VARIABLE.IsDead)
                {
                    EnterEntity.Remove(VARIABLE);
                }

            }
        }

        if (EnterEntity.Count <= 0)
        {
            Target = null;
            Line.gameObject.SetActive(false);
            Circle.SetActive(false);
            FireBallNumber = 0;
        }

    }

    private void Attack()
    {
        //创建火球
        isAttacking = true;
        Fire = Instantiate(FireFB, transform.position + new Vector3(0, 5.6f, 0), Quaternion.identity);
        FireBallNumber++;
        Fire.GetComponent<FireBallMove>().Target = this.Target;
        Fire.GetComponent<FireBallMove>().TouchCallBack = OnTouchCallBack;
    }

    private void OnTouchCallBack(GameObject obj)
    {

        if (GetFirstEnterEntity() != null)
        {
            //Target = GetFirstEnterEntity().gameObject;
            if (GetFirstEnterEntity().Health > AttackDamage)
            {
                Attack();
            }
            else if(EnterEntity.Count>=2)
            {
                EnterEntity.Remove(EnterEntity[0]);
                Attack();
            }
        }

        if (obj.GetComponent<EntityModel>().Camp != BattleFieldManager.Instance.MyCamp)
        {
            BattleFieldRequest.Instance.HurtRequest(obj.GetComponent<EntityModel>().Index, AttackDamage);
        }
        FireBallNumber--;
        isAttacking = false;
    }

    internal void PlayDestory()
    {
        Instantiate(Resources.Load("Tower/TowerFX/Boom"), transform.position, Quaternion.identity);
        GetComponent<HealthBar>().healthLink.targetScript = null;
       //transform.Translate(Vector3.down * 25);
        gameObject.SetActive(false);
        //GetTower(Index).gameObject.SetActive(false);
        //Destroy(GetTower(Index).gameObject);
        BattleFieldManager.Instance.TowerList.Remove(this);
        IsDead = true;
    }
}