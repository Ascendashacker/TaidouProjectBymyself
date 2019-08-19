
using PENet;
using System;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg:PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
    }
    #region 登录相关
    [Serializable]
    public class ReqLogin //请求登录
    {
        public string acct;
        public string pass;
    }

    [Serializable]//打上标签才能转成二进制
    public class RspLogin //登录回应
    {
        public PlayerData playerData;
    }
    [Serializable]
    public class PlayerData
    {
        public int id; //id
        public string name; //名字
        public int lv;//等级
        public int exp; //经验
        public int power;//体力
        public int coin;//金币
        public int diamond; //钻石

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

    }
    [Serializable]
    public class ReqRename
    {
        public string name;

    }
    [Serializable]
    public class RspRename
    {
        public string name;
    }
    #endregion

    public enum CMD
    {
        None = 0,
        //登录相关
        ReqLogin =101,
        RspLogin = 102,
        ReqRename = 103,
        RspRename = 104,
    }
    public enum ErrorCode
    {
        None =0, //没有错误
        AcctIsOnLine, //账号已经上线
        WrongPass,//密码错误
        NameIsExist, //名称已存在
        UpdateDBError, //更新数据库出错
    }
   
    public class SrvCfg
    {
        public const string srvIP= "127.0.0.1";
        public const int srvPort = 17666;
    }
}
