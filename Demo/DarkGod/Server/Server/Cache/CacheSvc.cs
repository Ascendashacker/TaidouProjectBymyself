
using PEProtocol;
using System.Collections.Generic;

public class CacheSvc
{
    private static CacheSvc instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;
        }
    }
    private DBMgr dBMgr;
    public void Init()
    {
        dBMgr= DBMgr.Instance;
        PECommon.Log("缓存模块已启动！");
    }
    private Dictionary<string,ServerSession> onLineAccDic = new Dictionary<string, ServerSession> ();
    private Dictionary<ServerSession,PlayerData> onlineSessionDic = new Dictionary<ServerSession, PlayerData> ();
    public bool IsAcctOnLine(string acct)
    {
        return onLineAccDic.ContainsKey(acct);
    }
    public bool IsNameExist(string name)
    {
        return dBMgr.QueryNameData(name);
    }
    //根据账号密码返回对应账号数据，密码错误返回null，账号不存在则默认创建新账号
    public PlayerData GetPlayerData(string acct,string pass)
    {
        //TODO  从数据库中查找账号数据
        return dBMgr.QueryPlayerData(acct,pass);
    }

    //账号上线，缓存数据
    public void AcctOnLine(string acct,ServerSession serverSession,PlayerData playerData)
    {
        onLineAccDic.Add(acct,serverSession);
        onlineSessionDic.Add(serverSession,playerData);
    }

    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onlineSessionDic.TryGetValue(session,out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }
    public bool UpdatePlayerData(int id,PlayerData playerData)
    {
        return dBMgr.UpdatePlayerData(id,playerData);
    }
    public void AccOffLine(ServerSession session)
    {
        foreach (var item in onLineAccDic)
        {
            if (item.Value == session)
            {
                onLineAccDic.Remove(item.Key);
                break;
            }
        }
       bool succ = onlineSessionDic.Remove(session);
        PECommon.Log("ID:"+session.sessionID+"的客户端移除结果："+succ);
    }
}
