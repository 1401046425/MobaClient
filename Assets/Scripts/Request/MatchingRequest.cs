using Common;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MatchingRequest : Request
{
    public static MatchingRequest Instance;

    private AsyncOperation async;
    public GameObject Main_camera;
    public GameObject camera2;
    public int MyplayerIndex;
    public GameObject LoginScene;
    public HeroType ChoiseHero;
    private Transform INSCHPOS;

    [HideInInspector]
    public GameObject INSHero;

    public GameObject SpawnFX;
    public GameObject SpawnFX2;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void DefaultRequest()
    {
    }

    public override void OnEvent(EventData data)
    {
        if (DicTool.GetValue(data.Parameters, (byte)ParaType.BFOther) != null)
        {
            var dataCode = (ParaCode)DicTool.GetValue(data.Parameters, (byte)ParaType.BFOther);
            if (dataCode == ParaCode.MC_Confirm)
            {
                var PlayerIndex = (int)DicTool.GetValue(data.Parameters, (byte)ParaCode.MC_Confirm);
                Debug.Log("匹配成功！" + PlayerIndex);
                LoginScene = GameObject.Find("LoginScene");
                Main_camera = GameObject.Find("Main Camera");
                camera2 = GameObject.Find("Camera2");
                MyplayerIndex = PlayerIndex;
                camera2.GetComponent<Camera>().targetDisplay = 0;
                camera2.GetComponent<Camera2>().StartChooseHeroAnimation();
                Camera.main.targetDisplay = 1;
                //Main_camera.SetActive(false);
                LoginScene.SetActive(false);
                LoudBFScene();
            }
            if (dataCode == ParaCode.MC_Prepare)
            {
                JoinTheGame();
            }
            if (dataCode == ParaCode.MC_DestoryOnline)
            {
                async.allowSceneActivation = true;
                SceneManager.LoadSceneAsync(0);
                GC.Collect();
            }
        }
        else if (DicTool.GetValue(data.Parameters, (byte)ParaType.BFSendEvent) != null)

        {
            var dataCode = (ParaCode)DicTool.GetValue(data.Parameters, (byte)ParaType.BFSendEvent);
        }
    }

    private IEnumerator LoudScene()
    {
        yield return new WaitForEndOfFrame();
        GC.WaitForPendingFinalizers();//挂起当前线程，直到处理终结器队列的线程清空该队列为止
        GC.Collect();
        yield return new WaitForFixedUpdate();
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        yield return null;
    }

    public void JoinTheGame()
    {
        if (ChoiseHero != HeroType.None) StartCoroutine(StartGame(MyplayerIndex, ChoiseHero));
    }

    public void LoudBFScene()
    {
        StartCoroutine(LoudScene());
    }

    private IEnumerator StartGame(int Playerindex, HeroType heroType)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        async.allowSceneActivation = true;
    }

    public override void OnOperationRespionse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            GameObject.Find("MatchingText").GetComponent<Text>().text = "匹配中...";
            Debug.Log("匹配中...");
        }
        else
        {
            Debug.Log("匹配失败");
        }
    }

    public void PrepareRequest()//还没用这个方法
    {
        var data = new Dictionary<byte, object>();
        data.Add((byte)ParaType.BFOther, ParaCode.MC_Prepare);
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    public void MatchingStartRequest()
    {
        var data = new Dictionary<byte, object>();
        data.Add((byte)ParaType.BFOther, ParaCode.MC_Start);
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    public void InsChoiseHero(HeroType heroType)
    {
        if (heroType != HeroType.None)
        {
            ChoiseHero = heroType;
            if (INSHero != null)
            {
                Destroy(INSHero);
            }
            var INSPOS = GameObject.Find("INSCHPOS").transform.position;
            INSHero = Instantiate(Resources.Load("Prefab/HeroChoose/" + ChoiseHero.ToString()), INSPOS, Quaternion.identity) as GameObject;
            INSHero.transform.LookAt((new Vector3(INSHero.transform.position.x, INSHero.transform.position.y, INSHero.transform.position.z - 1)));

            //生成特效
            if (SpawnFX2 == null)
            {
                SpawnFX2 = Instantiate(Resources.Load("Prefab/HeroChoose/SpawnFX2"), INSPOS, Quaternion.identity) as GameObject;
            }
            if (SpawnFX == null)
            {
                SpawnFX = Instantiate(Resources.Load("Prefab/HeroChoose/SpawnFX"), INSPOS, Quaternion.identity) as GameObject;
            }
            else
            {
                SpawnFX.SetActive(true);
            }
        }
    }

    private void Start()
    {
    }
}