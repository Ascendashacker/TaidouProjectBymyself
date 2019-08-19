using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    //教程 38
    public static GameRoot Instance = null;
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;
	// Use this for initialization
	void Start () {
        Instance = this;
        DontDestroyOnLoad(this);
        ClearUIRoot();
		Init();
	}
	
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWnd.gameObject.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
		
	}

    private void Init()
    {
        //服务模块初始化
        NetSvc netSvc = GetComponent<NetSvc>();
        netSvc.InitSvc();
        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvs();
        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();
        //业务模块初始化
        LoginSys loginSys = GetComponent<LoginSys>();
        loginSys.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        //进入登录场景 并加载相应UI
        loginSys.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name)
    {
        playerData.name = name;
    }
}
