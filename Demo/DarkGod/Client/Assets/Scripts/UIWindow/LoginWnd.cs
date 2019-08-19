using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot {
    public InputField ipAcct;
    public InputField ipPass;
    public Button btnEnter;
    public Button btnNotice;
    protected override void InitWnd()
    {
        base.InitWnd();
        if (PlayerPrefs.HasKey("Acct")&&PlayerPrefs.HasKey("Pass"))
        {
            ipAcct.text = PlayerPrefs.GetString("Acct");
            ipPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            ipAcct.text = "";
            ipPass.text = "";
        }
    }
    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UILoginBtn);
        string _acct = ipAcct.text;
        string _pass = ipPass.text;
        if (_acct!= "" &&_pass !="")
        {
            PlayerPrefs.SetString("Acct",_acct);
            PlayerPrefs.SetString("Pass", _pass);
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqLogin,
                reqLogin = new ReqLogin
                {
                    acct = _acct,
                    pass= _pass
                } 
            };
            netSvc.SendMsg(msg);
            // TODO 网络
            //LoginSys.Instance.RspLogin(msg);
        }
        else
        {
            GameRoot.AddTips("账号或密码为空");
        }
    }

    public void ClickNoticeBtn()
    {
        GameRoot.AddTips("功能正在开发中...");
    }
}
