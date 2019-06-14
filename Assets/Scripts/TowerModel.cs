using System.Collections.Generic;
using UnityEngine;

public class TowerModel : EntityModel
{
    private GameObject Target;
    public GameObject FireFB;
    private GameObject Fire;
    private LineRenderer Line;
    public GameObject Circle;
    public int AttackDamage;
    public List<EntityModel> EnterEntity=new List<EntityModel>();
    // Start is called before the first frame update

    private void Start()
    {
        BattleFieldManager.Instance.TowerList.Add(this);
        Line = transform.Find("Line").gameObject.GetComponent<LineRenderer>();
        if (Circle.activeSelf)
        {
            Circle.SetActive(false);
        }
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (Target != null)
        {
            Line.gameObject.SetActive(true);
            Line.SetPosition(0, transform.position + new Vector3(0, 5.3f, 0));
            Line.SetPosition(1, Target.transform.position + new Vector3(0, 1.4f, 0));
        }
        else
        {
            Line.gameObject.SetActive(false);
        }
    }

    EntityModel GetFirstEnterEntity()
    {
        if (EnterEntity.Count == 0) return null;
        if (EnterEntity[0] == null || EnterEntity[0].IsDead)
        {

            EnterEntity.Remove(EnterEntity[0]);
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
        if(!EnterEntity.Contains(EnterINS))EnterEntity.Add(EnterINS);
        Target = GetFirstEnterEntity().gameObject;
        Debug.Log("炮塔攻击啦！！");
        //只有炮塔攻击组中添加第一个才会进行攻击
        if (EnterEntity.Count == 1)
        {
            Attack();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        var Entity = other.GetComponent<EntityModel>();
        if(!Entity)return;
        if (Entity.Camp != Camp)
        {
            if (Entity != null)
            {
                if (EnterEntity.IndexOf(Entity) >= 1)
                {

                    EnterEntity.Remove(Entity);

                }
            }
            foreach (var VARIABLE in EnterEntity)
            {
                if (VARIABLE.IsDead)
                {
                    EnterEntity.Remove(VARIABLE);
                }

            }
            Circle.SetActive(false);
            Target = null;
        }



    }

    private void Attack()
    {
        //创建火球
        Fire = Instantiate(FireFB, transform.position + new Vector3(0, 5.6f, 0), Quaternion.identity);
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