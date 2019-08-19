using PEProtocol;
using PENet;
public class ClientSession : PESession<GameMsg> {
    protected override void OnConnected()
    {
        base.OnConnected();
        GameRoot.AddTips("连接服务器成功！");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        base.OnReciveMsg(msg);
        PECommon.Log("接受到消息CMD："+((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected()
    {
        base.OnDisConnected();
        GameRoot.AddTips("断开服务器连接！");
    }
}
