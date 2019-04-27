using Common;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldRequest : Request
{
    public static BattleFieldRequest Instance;
    private float curX;
    private float curY;
    public Vector3 pos;

    private void Awake()
    {
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
            if (dataCode == ParaCode.BF_Join)
            {
                var allPlayer = (string)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Join);
                BattleFieldManager.Instance.AddPlayer(allPlayer);
            }
            else if (dataCode == ParaCode.BF_DestoryOnline)
            {
                var playerindex = (int)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_DestoryOnline);
                BattleFieldManager.Instance.DestoryPlayer(playerindex);
            }
            else if (dataCode == ParaCode.BF_Ending)
            {
                var para = (int)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Ending);
                BattleFieldManager.Instance.CheckResult(para);
            }
        }
        else if (DicTool.GetValue(data.Parameters, (byte)ParaType.BFSendEvent) != null)
        {
            var dataCode = (ParaCode)DicTool.GetValue(data.Parameters, (byte)ParaType.BFSendEvent);
            if (dataCode == ParaCode.BF_Move)
            {
                var para = (string)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Move);
                var list = para.Split(',');
                BattleFieldManager.Instance.MovePlayer(Convert.ToInt32(list[0]), float.Parse(list[1]), float.Parse(list[2]), float.Parse(list[3]), float.Parse(list[4]), float.Parse(list[5]));
            }
            else if (dataCode == ParaCode.BF_Test_Pos)
            {
                var para = (string)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Test_Pos);
                var list = para.Split(',');
                BattleFieldManager.Instance.TestPlayerPos(Convert.ToInt32(list[0]), float.Parse(list[1]), float.Parse(list[2]), float.Parse(list[3]));
            }
            else if (dataCode == ParaCode.BF_Attack)
            {
                var para = (string)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Attack);
                var list = para.Split(',');
                BattleFieldManager.Instance.AttackPlayer(int.Parse(list[0]), int.Parse(list[1]), (AttackType)int.Parse(list[2]));
            }
            else if (dataCode == ParaCode.BF_Hurt)
            {
                var para = (string)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Hurt);
                var list = para.Split(',');
                BattleFieldManager.Instance.HurtTarget(int.Parse(list[0]), int.Parse(list[1]));
            }
            else if (dataCode == ParaCode.BF_Destory)
            {
                var para = (int)DicTool.GetValue(data.Parameters, (byte)ParaCode.BF_Destory);
                BattleFieldManager.Instance.DestoryTarget(para);
            }
        }
    }

    internal void HurtRequest(int HurtPlayer, int HurtDamage)
    {
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFSendEvent, ParaCode.BF_Hurt },
            { (byte)ParaCode.BF_Hurt, (HurtPlayer+","+HurtDamage) }
        };
        Debug.Log("受伤请求发送");
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void AttackRequest(int HurtPlayerIndex, int AttackPlayerIndex, AttackType attackType)
    {
        string attackdata = HurtPlayerIndex + "," + AttackPlayerIndex + "," + (int)attackType;
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFSendEvent, ParaCode.BF_Attack },
            { (byte)ParaCode.BF_Attack, attackdata }
        };
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void DestoryRequest(int Index)
    {
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFSendEvent, ParaCode.BF_Destory },
            { (byte)ParaCode.BF_Destory, Index }
        };
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void EndRequest(int LostplayerIndex)
    {
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFOther, ParaCode.BF_Ending },
            { (byte)ParaCode.BF_Ending, LostplayerIndex }
        };
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void TestPosRequest(int playerindex, Vector3 pos)
    {
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFSendEvent, ParaCode.BF_Test_Pos },
            { (byte)ParaCode.BF_Test_Pos, playerindex.ToString() + "," + pos.x + "," + pos.y + "," + pos.z }
        };
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    internal void MoveRequest(float posX, float posY, Vector3 pos)
    {
        //发送操作数据
        if (curX == (float)Math.Round(posX, 1) && curY == (float)Math.Round(posY, 1)) return;
        curX = (float)Math.Round(posX, 1);
        curY = (float)Math.Round(posY, 1);
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFSendEvent, ParaCode.BF_Move },
            { (byte)ParaCode.BF_Move, BattleFieldManager.Instance.MyPlayerIndex.ToString() + "," + posX + "," + posY + "," + pos.x + "," + pos.y + "," + pos.z }
        };
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationRespionse(OperationResponse operationResponse)
    {
        //解析数据
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            var playerIndex = (int)DicTool.GetValue(operationResponse.Parameters, (byte)ParaCode.BF_Join);
            Debug.Log("我的玩家序列号:" + playerIndex);
            BattleFieldManager.Instance.MyPlayerIndex = playerIndex;
        }
        else
        {
            Debug.Log("请求失败");
        }
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void JoinRequest(HeroType heroType)
    {
        var data = new Dictionary<byte, object>
        {
            { (byte)ParaType.BFOther, ParaCode.BF_Join },
            { (byte)ParaCode.BF_Join, heroType.ToString()}
        };

        Debug.Log(data.Count);

        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }
}