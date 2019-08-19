using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MainCityWnd : WindowRoot {

    #region UI
    [SerializeField]
    private Image imgTouch;
    [SerializeField]
    private Image imgDirBg;
    [SerializeField]
    private Image imgDirPoint;
    [SerializeField]
    private Animation menuAni;
    [SerializeField]
    private Text txtFight;
    [SerializeField]
    private Text txtPower;
    [SerializeField]
    private Text txtLevel;
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Text txtExpPrg;
    [SerializeField]
    private Image imgPowerPrg;
    [SerializeField]
    private Transform expPrgTrans; 
    [SerializeField]
    private Button menuBtn;
    #endregion
    private bool menuState = true;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    private float pointDis;
    protected override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height *1.0f /Constants.ScreenStandardHeight *Constants.ScreenOPDis;  //自适应
        defaultPos = imgDirBg.transform.position;
        imgDirPoint.gameObject.SetActive(false);
        RefreshUI();
        RegisterTouchEvts();
    }
    
    private void RefreshUI()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        SetText(txtFight,PECommon.GetFightByProps(playerData));
        int powerLimit = PECommon.GetPowerLimit(playerData.lv);
        SetText(txtPower,"体力："+playerData.power +"/"+ powerLimit);
        imgPowerPrg.fillAmount = playerData.power *1.0f / powerLimit;
        SetText(txtLevel,playerData.lv);
        SetText(txtName,playerData.name);
        
        //经验条自适应
        int expPrgVal= (int)(playerData.exp *1.0f / PECommon.GetExpUpValByLv(playerData.lv)*100);
        SetText(txtExpPrg,expPrgVal +"%");
        int index = expPrgVal /10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = 1.0f *Constants.ScreenStandardHeight /Screen.height;
        float screenWidth = Screen.width *globalRate;
        float width = (screenWidth - 180)/10;
        grid.cellSize = new Vector2 (width,7);
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i<index)
            {
                img.fillAmount = 1;
            }
            else if(i==index)
            {
                img.fillAmount = expPrgVal % 10 *1.0f /10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
    }

    public void ClickMenuBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);
        menuState = !menuState;
        AnimationClip clip = null;
        if (menuState)
        {
            clip = menuAni.GetClip("OpenMCMenu");
        }
        else
        {
            clip = menuAni.GetClip("CloseMCMenu");
        }
        menuAni.Play(clip.name);
    }

    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }


    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject,(data)=>{ 
            startPos = data.position;
            imgDirPoint.gameObject.SetActive(true);
            imgDirBg.transform.position = data.position;
            });
        OnClickUp(imgTouch.gameObject,(data)=>{ 
            imgDirBg.transform.position = defaultPos;
            imgDirPoint.gameObject.SetActive(false);
            imgDirPoint.transform.localPosition = Vector3.zero;
            MainCitySys.Instance.SetMoveDir(Vector2.zero);
            //TODO 方向信息 传递出去
        });
        OnDrag(imgTouch.gameObject, (data) => {
           Vector2 dir = data.position - startPos;
            float lenth = dir.magnitude;
            if (lenth > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos +clampDir;
            }
            else
            {
                imgDirPoint.transform.position = data.position;
            }
            MainCitySys.Instance.SetMoveDir(dir.normalized);
            //TODO 方向信息 传递出去
        });
    }
}
