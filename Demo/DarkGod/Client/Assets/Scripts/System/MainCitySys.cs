
using UnityEngine;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;
    public MainCityWnd mainCityWnd;
    private PlayerController playerCtrl;
    public InfoWnd infoWnd;
    private Transform charShowCamTrans;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        PECommon.Log("初始化主城系统！");
    }

    public void EnterMainCity()
    {
        MapCfg mapCfg = resSvc.GetMapCfgData(Constants.MainCityMapID);
        resSvc.AsynLoadScene(mapCfg.sceeName, ()=>{ 
            PECommon.Log("进入主城...");
            //加载游戏主角
            LoadPlayer(mapCfg);
            //打开主城场景UI
            mainCityWnd.SetWndState();
            //播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            //设置任务展示相机
            if (charShowCamTrans!=null)
            {
                charShowCamTrans.gameObject.SetActive(false);
            }
            });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab,true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3 (1.5f,1.5f,1.5f);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
    }

    public void SetMoveDir(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        playerCtrl.Dir = dir;
    }
    public void OpenInfoWnd()
    {
        if (charShowCamTrans == null)
        {
            charShowCamTrans = GameObject.FindGameObjectWithTag("CharShowCamera").transform;
        }
        //设置相对位置
        charShowCamTrans.localPosition = playerCtrl.transform.position+playerCtrl.transform.forward *3.8f + new Vector3(0,1.2F,0);
        charShowCamTrans.localEulerAngles = new Vector3 (0,180+playerCtrl.transform.localEulerAngles.y,0);
        charShowCamTrans.localScale = Vector3.one;
        charShowCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        if (charShowCamTrans != null)
        {
            charShowCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    private float startRoate = 0;
    public void SetStartRotate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.y;
    }
    public void SetPlayerRotate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3 (0,startRoate + roate ,0);
    }
}

