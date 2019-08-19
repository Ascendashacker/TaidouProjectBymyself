using PENet;
using PEProtocol;
using System.Collections.Generic;
using UnityEngine;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance = null;
    PESocket<ClientSession,GameMsg> client = null;
    private Queue<GameMsg> msgQue  = new Queue<GameMsg> ();
    private static readonly string obj ="lock";
    public void InitSvc()
    {
        Instance = this;
        client = new PESocket<ClientSession, GameMsg> ();
        client.SetLog(true,(string msg,int lv)=>{
            switch (lv)
            {
                case 0:
                    msg = "Log:" +msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    PECommon.Log(msg);
                    break;
            }
        });
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
        PECommon.Log("Init NetSvc ...",LogType.Log);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            client.session.SendMsg(new GameMsg{ });
        }
        if (msgQue.Count > 0)
        {
            lock (obj)
            {
               GameMsg msg = msgQue.Dequeue();
               ProcessMsg(msg);
            }
        }
    }
    public void SendMsg(GameMsg msg)
    {
        if (client.session !=null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接！");
            InitSvc();
        }
    }

    public void AddNetPkg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err!=(int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.AcctIsOnLine:
                    GameRoot.AddTips("当前账号已经在线!");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误！");
                    break;
                case ErrorCode.UpdateDBError:
                    GameRoot.AddTips("网络不稳定！");
                    PECommon.Log("数据库更新异常",LogType.Error);
                    break;
            }
            return;
        }
        switch ((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
                case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
        }
    }
}

