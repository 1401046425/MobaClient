using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerModel : MonoBehaviour, IHurtObject
{
    private GameObject Target;
    public GameObject FireFB;
    private GameObject Fire;
    private LineRenderer Line;
    public GameObject Circle;
    public int TowerIndex;

    // Start is called before the first frame update
    public BattleFieldCamp TowerCamp;

    public int HP;

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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Hero")) return;
        if (other.GetComponent<HeroModel>().HeroCamp == TowerCamp) return;
        Circle.SetActive(true);
        Target = other.gameObject;
        Attack();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.CompareTag("Hero")) return;
        Circle.SetActive(false);
        Target = null;
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
        if (obj.GetComponent<HeroModel>().PlayerIndex == BattleFieldManager.Instance.MyPlayerIndex)
            BattleFieldRequest.Instance.HurtRequest(obj.GetComponent<HeroModel>().PlayerIndex, 10);
        if (Target != null)
        {
            Attack();
        }
    }

    public void SendHurtRequest(int HurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(TowerIndex, HurtValue);
    }

    internal void PlayDestory()
    {
        Instantiate(Resources.Load("Tower/TowerFX/Boom"), transform.position, Quaternion.identity);
        GetComponent<HealthBar>().healthLink.targetScript = null;
        transform.Translate(Vector3.down * 25);
        this.enabled = false;
        //GetTower(TowerIndex).gameObject.SetActive(false);
        //Destroy(GetTower(TowerIndex).gameObject);
        BattleFieldManager.Instance.TowerList.Remove(this);
    }
}