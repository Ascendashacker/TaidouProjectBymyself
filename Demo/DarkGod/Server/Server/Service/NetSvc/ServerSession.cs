
using PENet;
using PEProtocol;

public class ServerSession:PESession<GameMsg>
{
    public int sessionID = 0;
    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();
        base.OnConnected();
        PECommon.Log("客户端已连接,ID：" + sessionID);
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        base.OnReciveMsg(msg);
        PECommon.Log("ID:"+ sessionID + "的客户端消息：");
        NetSvc.Instance.AddMsgQue(this,msg);
    }

    protected override void OnDisConnected()
    {
        base.OnDisConnected();
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("ID:"+ sessionID + "的客户端断开连接");
    }
}

