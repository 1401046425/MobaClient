using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModel : EntityModel
{
    public bool ISME = false;
    public HeroAttack Hero_Attack;
    public HeroMove Hero_Move;
    public Animator Hero_Animator;
    private int OrinHealth;
    private Vector3 SpawnPos;
    // Start is called before the first frame update
    private void Awake()
    {
        InitHeroModel();
    }

    private void InitHeroModel()
    {
        OrinHealth = Health;
        SpawnPos = transform.position;
        Hero_Animator = GetComponent<Animator>();
        Hero_Attack = GetComponent<HeroAttack>();
        Hero_Move = GetComponent<HeroMove>();
    }

    internal void PlayDestory()
    {
        transform.position = SpawnPos;
        Health = OrinHealth;
        //GetComponent<HealthBar>().healthLink.targetScript = null;
        //Destroy(this.gameObject);
        //GetBFManager().PlayerList.Remove(this);
    }//当玩家销毁

    public BattleFieldManager GetBFManager()
    {
        return BattleFieldManager.Instance;
    }

    public BattleFieldRequest GetBFRequest()
    {
        return BattleFieldRequest.Instance;
    }


    // Update is called once per frame
    private void Update()
    {
    }
}