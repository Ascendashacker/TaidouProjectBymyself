using PEProtocol;

public class LoginSys
{
    private static LoginSys instance = null;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("登录系统初始化成功");
    }

    public void ReqLogin(MsgPack msgPack)
    {
        ReqLogin reqLogin = msgPack.msg.reqLogin;
        //当前账号是否已经上线
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin
        };
        //已上线，返回错误信息
        if (cacheSvc.IsAcctOnLine(reqLogin.acct))
        {
            msg.err = (int)ErrorCode.AcctIsOnLine;
        }
        else
        {
            //未上线 ：
            //账号是否存在
            PlayerData pd = CacheSvc.Instance.GetPlayerData(reqLogin.acct,reqLogin.pass);
            if (pd == null)
            {
                //存在：密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };
                //缓存账号数据
                cacheSvc.AcctOnLine(reqLogin.acct, msgPack.session,pd);
            }
        }
       
        
       
        //不存在：创建默认的账号密码

        //回应客户端

        msgPack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };
        if (cacheSvc.IsAcctOnLine(data.name))
        {
            //名称存在：返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            //不存在：更新缓存  以及数据库，再返回给客户端
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name= data.name;
            if (!cacheSvc.UpdatePlayerData(playerData.id,playerData)) //更新数据库出错
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                } ;
            }
        }
        pack.session.SendMsg(msg);

    }

    public void ClearOfflineData(ServerSession session)
    {
        cacheSvc.AccOffLine(session);
    }
}

