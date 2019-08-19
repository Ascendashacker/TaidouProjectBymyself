using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PEProtocol;

public class InfoWnd : WindowRoot
{
    public RawImage imgChar;
    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtJob;
    public Text txtFight;
    public Text txtHp;
    public Text txtHurt;
    public Text txtDef;
    public Button btnClose;
    public Button btnCloseDetail;
    public Button btnDetail;
    public Transform transDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;
    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        transDetail.gameObject.SetActive(false);
        RefreshUI();
    }

    private Vector2 startPos;
    private void RegTouchEvts()
    {
        OnClickDown(imgChar.gameObject,(data)=>{
            startPos = data.position;
            MainCitySys.Instance.SetStartRotate();
            });
        OnDrag(imgChar.gameObject, (data) => {
            float rotate = -(data.position.x -startPos.x)*0.5f;
            MainCitySys.Instance.SetPlayerRotate(rotate);
        });
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo,pd.name + " LV." +pd.lv);
        SetText(txtExp, "/"+PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount =pd.exp * 1.0f/PECommon.GetExpUpValByLv(pd.lv);
        SetText(txtPower, pd.power + "/"+ PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        SetText(txtJob, " 职业  暗夜刺客");
        SetText(txtFight, " 战力  " +PECommon.GetFightByProps(pd));
        SetText(txtHp, " 血量  " + pd.hp);
        SetText(txtHurt, " 伤害  " + pd.ad+pd.ap);
        SetText(txtDef, " 防御  " + pd.addef +pd.apdef);

        SetText(dtxhp,pd.hp);
        SetText(dtxad, pd.ad);
        SetText(dtxap, pd.ap);
        SetText(dtxaddef, pd.addef);
        SetText(dtxapdef, pd.apdef);
        SetText(dtxdodge, pd.dodge + "%");
        SetText(dtxpierce, pd.pierce + "%");
        SetText(dtxcritical, pd.critical + "%");
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }
    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        transDetail.gameObject.SetActive(false);
    }
    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        transDetail.gameObject.SetActive(true);
    }
}
