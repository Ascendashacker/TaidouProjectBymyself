
using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

public class NetSvc
{
    private static NetSvc instance = null;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetSvc();
            }
            return instance;
        }
    }
    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init()
    {
        PESocket<ServerSession,GameMsg> server = new PESocket<ServerSession, GameMsg> ();
        server.StartAsServer(SrvCfg.srvIP,SrvCfg.srvPort);

        PECommon.Log("服务器已启动！");
    }

    public void AddMsgQue(ServerSession session,GameMsg msg)
    {
        lock (obj) //异步 锁住  数据安全
        {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            PECommon.Log("消息队列个数：" +msgPackQue.Count);
            lock (obj)
            {
                MsgPack msgPack = msgPackQue.Dequeue();
                HandOutMsg(msgPack);
            }
        }
    }

    private void HandOutMsg(MsgPack msgPack) //消息分发
    {
        switch ((CMD)msgPack.msg.cmd)
        {
            case CMD.None:
                break;
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(msgPack);
                break;
            case CMD.RspLogin:
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(msgPack);
                break;
            default:
                break;
        }
    }
}

