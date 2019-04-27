using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class Request : MonoBehaviour
{
    public OperationCode OpCode;

    //抽象方法就是只声明，具体的实现由子类完成
    public abstract void DefaultRequest();

    public abstract void OnOperationRespionse(OperationResponse operationResponse);

    public abstract void OnEvent(EventData data);

    public void Awake()
    {
        PhotonEngine.PhotonEngine_INS.AddRequest(this);
    }

    public void OnDestroy()
    {
        PhotonEngine.PhotonEngine_INS.RemoveRequest(this);
    }
}