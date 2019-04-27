using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModel : MonoBehaviour, IHurtObject
{
    public int HP = 100;
    public bool ISME = false;
    public int PlayerIndex;
    public HeroAttack Hero_Attack;
    public HeroMove Hero_Move;
    public Animator Hero_Animator;
    public BattleFieldCamp HeroCamp = BattleFieldCamp.None;

    // Start is called before the first frame update
    private void Awake()
    {
        InitHeroModel();
    }

    private void InitHeroModel()
    {
        Hero_Animator = GetComponent<Animator>();
        Hero_Attack = GetComponent<HeroAttack>();
        Hero_Move = GetComponent<HeroMove>();
    }

    internal void PlayDestory()
    {
        GetComponent<HealthBar>().healthLink.targetScript = null;
        Destroy(this.gameObject);
        GetBFManager().PlayerList.Remove(this);
    }//当玩家销毁

    public BattleFieldManager GetBFManager()
    {
        return BattleFieldManager.Instance;
    }

    public BattleFieldRequest GetBFRequest()
    {
        return BattleFieldRequest.Instance;
    }

    public void SendHurtRequest(int HurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(PlayerIndex, HurtValue);
    }//发送受伤请求

    // Update is called once per frame
    private void Update()
    {
    }
}