using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : SystemRoot {
    public static LoginSys Instance = null;
    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    }
    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        //异步加载登录场景
        resSvc.AsynLoadScene(Constants.SceneLogin,()=> {
            loginWnd.SetWndState();
        });
        audioSvc.PlayBGMusic(Constants.BGLogin,true);
        //兵显示加载速度
        //加载完成以后再打开注册登录界面

    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);
        if (msg.rspLogin.playerData.name == "") //新账号
        {
            //打开角色创建面板
            createWnd.SetWndState();
        }
        else
        {
            //进入主城 TODO
            MainCitySys.Instance.EnterMainCity();
        }
        //关闭登录界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);
        //TODO 
        //跳转场景
        //打开主城场景
        MainCitySys.Instance.EnterMainCity();
        //关闭创建面板
        createWnd.SetWndState(false);
    }
}
