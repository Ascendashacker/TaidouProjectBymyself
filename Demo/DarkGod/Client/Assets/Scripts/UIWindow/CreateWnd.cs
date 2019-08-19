using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot {

    public InputField iptName;
    protected override void InitWnd()
    {
        base.InitWnd();
        iptName.text = resSvc.GetRDNameData(false);    
    }

    public void ClickRandomBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        string rdName= resSvc.GetRDNameData();
        iptName.text = rdName;
    }

    public void ClickEnterBtn()
    {
        if (iptName.text !="")
        {
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
                };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不符合规范");
        }
    }

}
