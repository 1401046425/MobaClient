using Common;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public static PhotonEngine PhotonEngine_INS;

    public static PhotonPeer peer;
    public GameObject MachingManager;
    public int ClientIndex;
    private Dictionary<OperationCode, Request> RequestDic = new Dictionary<OperationCode, Request>();
    public const string ServerIp = "localhost";
    private StatusCode Client_ConnectState;

    public void AddRequest(Request R)
    {
        RequestDic.Add(R.OpCode, R);
    }

    public void RemoveRequest(Request R)
    {
        RequestDic.Remove(R.OpCode);
    }

    public Request GetRequest(OperationCode code)
    {
        return DicTool.GetValue(RequestDic, code);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    //当服务器发送事件时执行↓这个函数
    public void OnEvent(EventData eventData)
    {
        try
        {
            DicTool.GetValue(RequestDic, (OperationCode) eventData.Code)
                .OnEvent(eventData);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //当客户端发送事件后服务器返回时执行↓这个函数
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        DicTool.GetValue(RequestDic, (OperationCode) operationResponse.OperationCode)
            .OnOperationRespionse(operationResponse);
        return;
    }

    //当与服务器连接时执行
    public void OnStatusChanged(StatusCode statusCode)
    {
        Client_ConnectState = statusCode;
    }

    public void JoinTheGame(int Playerindex, HeroType heroType)
    {
        StartCoroutine(LoudHero(Playerindex, heroType));
    }

    private IEnumerator LoudHero(int Playerindex, HeroType heroType)
    {
        yield return new WaitForEndOfFrame();
        var BF = (BattleFieldRequest) DicTool.GetValue(RequestDic, OperationCode.BattleField);
        Debug.Log("加载英雄ID" + Playerindex);
        ClientIndex = Playerindex;
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        BF.JoinRequest(heroType);

        yield return null;
    }

    private void Awake()
    {
        if (PhotonEngine_INS == null)
        {
            PhotonEngine_INS = this;
            //当前对象不会随着场景的销毁而销毁
            DontDestroyOnLoad(this.gameObject);
            Instantiate(MachingManager);
        }

        Application.targetFrameRate = 30;
    }

    private void OnEnable()
    {
    }

    // Use this for initialization
    private void Start()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect(ServerIp + ":5055", "Server");
    }

    // Update is called once per frame
    private void Update()
    {
        peer.Service();
    }

    private void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    private void OnApplicationQuit()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    private void OnGUI()
    {
        GUIStyle style=new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(Screen.width*0.93f, Screen.height*0.98f,10,10), "连接状态:"+Client_ConnectState.ToString(),style);
    }
}
