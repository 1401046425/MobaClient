using System;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BattleFieldManager : MonoBehaviour
{
    public static BattleFieldManager Instance;

    public Sprite[] ResultUI = new Sprite[2];
    public GameObject ResultUIPanel;

    // Use this for initialization

    public GameObject HurtDamageFX;

    //public GameObject HurtFX2;
    public List<Transform> HPos;

    public List<Transform> CPos;
    public int MyPlayerIndex = 0;
    public List<HeroModel> PlayerList = new List<HeroModel>();
    public List<TowerModel> TowerList = new List<TowerModel>();
    private HeroModel Myplayer;
    public int JOINNUM;
    public BattleFieldCamp MyCamp = BattleFieldCamp.None;
    public string IP;
    private Ping ping;
    private float delayTime;

    private void Start()
    {
        IP = PhotonEngine.ServerIp;
        StartCoroutine(Pingip());
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(10, 10, 100, 20), "ping: " + delayTime.ToString() + "ms");
        GUI.Label(new Rect(0, 0, 200, 50), "我的阵营" + MyCamp.ToString());
    }

    private IEnumerator Pingip()
    {
        float pingTime = 0;
        ping = new Ping(IP);
        delayTime = Math.Abs(ping.time);
        while (!ping.isDone)
        {
            yield return new WaitForSeconds(0.1f);//0.1秒检测一次
            if (pingTime > 3.0)//如果大于3秒还没有ping成功，默认网络不可用
            {
                Debug.Log("ping失败");
                break;
            }
            pingTime += 0.1f;
        }
        if (ping.isDone)
        {
            Debug.Log("ping成功");
        }
        ping.DestroyPing();
        yield return new WaitForSeconds(1f);//1秒重新ping一次
        StartCoroutine(Pingip());
    }

    private void Awake()
    {
        Instance = this;

        PhotonEngine.PhotonEngine_INS.JoinTheGame(MatchingRequest.Instance.MyplayerIndex, MatchingRequest.Instance.ChoiseHero);
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(delayTime);
    }

    private void 这个函数是用来修复BUG的为什么这个函数这么长而且要用中文我就想说你管我我乐意()
    {
        if (MyPlayerIndex == 1 && PhotonEngine.PhotonEngine_INS.ClientIndex == 2)
        {
            GetPlayer(1).ISME = false;
            InitBattleField(GetPlayer(2));
            if (GetPlayer(2).ISME && MyPlayerIndex == 2)
            {
                CancelInvoke("这个函数是用来修复BUG的为什么这个函数这么长而且要用中文我就想说你管我我乐意");
            }
        }
    }

    public HeroModel GetPlayer(int PlayerIndex)
    {
        HeroModel Player = null;
        foreach (var item in PlayerList)
        {
            if (item.PlayerIndex == PlayerIndex)
            {
                Player = item;
            }
        }
        return Player;
    }

    public void InitBattleField(HeroModel p)//初始化自己人物角色的函数
    {
        //初始化场景
        Debug.Log("调用");
        //p.PlayerIndex = MyPlayerIndex;
        p.ISME = true;
        // PlayerList.Add(p);
        Myplayer = p;
        switch (p.PlayerIndex)
        {
            case 1:
                MyCamp = BattleFieldCamp.Red;
                break;

            case 2:
                MyCamp = BattleFieldCamp.Blue;
                break;

            default:
                break;
        }
        Debug.Log("摄像机初始化的玩家" + p.PlayerIndex);
        CameraFollow.cameraFollow.enabled = true;
        CameraFollow.cameraFollow.Init(p.transform, CPos[Myplayer.PlayerIndex - 1]);
    }

    public void OnAttack()
    {
        if (Myplayer != null)
            Myplayer.GetComponent<HeroAttack>().Attack(AttackType.Normal);
    }

    internal void AddPlayer(string aLLPlayer)//初始化其他玩家角色的函数
    {
        //解析

        var All = aLLPlayer.Split(',');

        foreach (var item in All)
        {
            var Playerinfo = item.Split('|');
            if (Playerinfo[0] != "" && Playerinfo[0] != null)
            {
                Debug.Log("加载玩家ID：" + Playerinfo[0] + "该玩家使用的英雄" + Playerinfo[1]);
                try
                {
                    //转换成int
                    int i = Convert.ToInt16(Playerinfo[0]);
                    if (GetPlayer(i) == null)
                    {
                        var Hero = Resources.Load("Prefab/Hero/" + Playerinfo[1]);
                        HeroModel p = (Instantiate(Hero, HPos[i - 1].position, Quaternion.identity) as GameObject).GetComponent<HeroModel>();
                        p.PlayerIndex = i;
                        PlayerList.Add(p);
                        switch (p.PlayerIndex)
                        {
                            case 1:
                                p.HeroCamp = BattleFieldCamp.Red;
                                break;

                            case 2:
                                p.HeroCamp = BattleFieldCamp.Blue;
                                break;

                            default:
                                break;
                        }
                        if (p.PlayerIndex == MyPlayerIndex)
                        {
                            Debug.Log("我的序号:" + MyPlayerIndex);
                            InitBattleField(p);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
        InvokeRepeating("这个函数是用来修复BUG的为什么这个函数这么长而且要用中文我就想说你管我我乐意", 0.01f, 0.01f);
    }

    internal void DestoryTarget(int para)
    {
        if (para < 1000)
        {
            DestoryPlayer(para);
        }
        else if (para >= 1000)
        {
            DestoryTower(para);
        }
    }

    internal TowerModel GetTower(int HurtTowerIndex)
    {
        TowerModel Tower = null;
        foreach (var item in TowerList)
        {
            if (item.TowerIndex == HurtTowerIndex)
            {
                Tower = item;
            }
        }
        return Tower;
    }

    internal void DestoryPlayer(int playerIndex)
    {
        if (GetPlayer(playerIndex) != null)
        {
            GetPlayer(playerIndex).PlayDestory();
        }
    }

    internal void DestoryTower(int TowerIndex)
    {
        if (GetTower(TowerIndex) != null)
        {
            GetTower(TowerIndex).PlayDestory();
        }
    }

    internal void TestPlayerPos(int playerIndex, float posx, float posy, float posz)
    {
        //客户端玩家角色同步坐标
        ////////////////////////
        {
            if (GetPlayer(playerIndex) != null && playerIndex != MyPlayerIndex)
            {
                var player = GetPlayer(playerIndex);

                var v = new Vector3(player.Hero_Move.DirX, player.Hero_Move.DirZ) * delayTime * Time.fixedDeltaTime;
                // var newpos = new Vector3(posx, posy, posz);
                var newpos = new Vector3(posx + v.x, posy, posz + v.y);
                var dir = player.transform.position - newpos;
                player.transform.position = newpos;
                if ((dir.magnitude > player.Hero_Move.Speed / 10))
                {
                    //newpos = new Vector3(posx + v.x, posy, posz + v.y);
                    Debug.Log("同步坐标");
                    player.transform.position = newpos;
                    // }
                }
            }
            ///////////////////////
        }
    }

    internal void CheckResult(int ResultIndex)
    {
        BattleFieldRequest.Instance.DestoryRequest(1);
        BattleFieldRequest.Instance.DestoryRequest(2);
        Debug.Log(ResultIndex);
        ResultUIPanel.GetComponent<Image>().sprite = ResultUI[ResultIndex];
        ResultUIPanel.SetActive(true);
        PlayerList = new List<HeroModel>();
    }

    public void BackToMainMscence()
    {
        PhotonEngine.PhotonEngine_INS.RemoveRequest(PhotonEngine.PhotonEngine_INS.GetRequest(OperationCode.BattleField));
        SceneManager.LoadSceneAsync(0);
    }

    internal void HurtTarget(int HurtPlayer, int HurtDamage)
    {
        if (HurtPlayer >= 1000)
        {
            var HurtTarget = GetTower(HurtPlayer);
            Debug.Log("攻击塔！！");
            if (HurtTarget.HP > 0)
            {
                HurtTarget.HP -= HurtDamage;
                HurtDamageFX.transform.Find("HPLabel").GetComponent<TextMesh>().text = "-" + HurtDamage;
                Instantiate(HurtDamageFX, new Vector3(HurtTarget.transform.position.x, HurtTarget.transform.position.y + 5.6f, HurtTarget.transform.position.z), Quaternion.identity);
                if (HurtTarget.HP <= 0)
                {
                    BattleFieldRequest.Instance.DestoryRequest(HurtTarget.TowerIndex);
                    Debug.Log("塔打爆了！");
                }
            }
        }
        else
        {
            var HurtTarget = GetPlayer(HurtPlayer);
            if (HurtTarget.HP > 0)
            {
                HurtTarget.HP -= HurtDamage;
                HurtDamageFX.transform.Find("HPLabel").GetComponent<TextMesh>().text = "-" + HurtDamage;
                Instantiate(HurtDamageFX, new Vector3(HurtTarget.transform.position.x, HurtTarget.transform.position.y + 2.5f, HurtTarget.transform.position.z), Quaternion.identity);
                if (HurtTarget.HP <= 0)
                {
                    BattleFieldRequest.Instance.EndRequest(HurtTarget.PlayerIndex);
                }
            }
        }
    }

    internal void AttackPlayer(int HurtPlayerIndex, int AttackPlayerIndex, AttackType attackType)
    {
        print("被打玩家：" + HurtPlayerIndex + "攻击玩家" + AttackPlayerIndex + "攻击类型" + attackType);
        var HeroAttackCtl = GetPlayer(AttackPlayerIndex).GetComponent<HeroAttack>();
        switch (attackType)
        {
            case AttackType.Normal:
                HeroAttackCtl.AttackAnimation(HurtPlayerIndex, AttackPlayerIndex);
                break;

            case AttackType.Skill1:
                HeroAttackCtl.PlaySkill1Animation(HurtPlayerIndex, AttackPlayerIndex);
                break;

            default:
                break;
        }

        //播放普通攻击动画
    }

    public void OnSkill1()
    {
        if (Myplayer != null)
            Myplayer.Hero_Attack.Attack(AttackType.Skill1);
    }

    internal void MovePlayer(int index, float x, float z, float PlayerX, float PlayerY, float PlayerZ)
    {
        //拿到对应Index的对象
        //设置移动对象
        foreach (var item in PlayerList)
        {
            //拿到对应的index对象
            if (item.PlayerIndex == index)
            {
                //设置对象
                item.Hero_Move.DirX = x * delayTime;
                item.Hero_Move.DirZ = z * delayTime;
            }
        }
    }
}

public enum BattleFieldCamp
{
    None,
    Red,
    Blue,
}