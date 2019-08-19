using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour {
    public static ResSvc Instance{ get; private set;}

   public void InitSvs()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
    }

    private Action prgCB = null;
    public void AsynLoadScene(string sceneName,Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState();
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = () => {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1)
            {
                if (loaded!=null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }
    private Dictionary<string,AudioClip> adDic = new Dictionary<string, AudioClip> ();
    public AudioClip LoadAudio(string path,bool cache = false)
    {
        AudioClip audioClip = null;
        if (!adDic.TryGetValue(path,out audioClip))
        {
            audioClip = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path,audioClip);
            }
        }
        return audioClip;
    }

    private void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }

    #region  InitCfgs
    #region  随机名字
    private List<string> surNameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();
    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("XML 解析失败", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement element = nodList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surNameList.Add(e.InnerText);
                            break;
                        case "man":
                            manList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }
    public string GetRDNameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surNameList[PETools.RDInt(0, surNameList.Count - 1)];
        if (man)
        {
            rdName += manList[PETools.RDInt(0, manList.Count - 1)];
        }
        else
        {
            rdName += womanList[PETools.RDInt(0, womanList.Count - 1)];
        }
        return rdName;
    }
    #endregion

    #region 地图

    private Dictionary<int,MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg> ();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("XML 解析失败", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement element = nodList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                MapCfg mc = new MapCfg{ ID = ID};
                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceeName = e.InnerText;
                            break;
                        case "mainCamPos":
                            string [] valArr = e.InnerText.Split(',');
                            mc.mainCamPos = new Vector3 (float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            break;
                        case "mainCamRote":
                            string[] valArr1 = e.InnerText.Split(',');
                            mc.mainCamRote = new Vector3(float.Parse(valArr1[0]), float.Parse(valArr1[1]), float.Parse(valArr1[2]));
                            break;
                        case "playerBornPos":
                            string[] valArr2 = e.InnerText.Split(',');
                            mc.playerBornPos = new Vector3(float.Parse(valArr2[0]), float.Parse(valArr2[1]), float.Parse(valArr2[2]));
                            break;
                        case "playerBornRote":
                            string[] valArr3 = e.InnerText.Split(',');
                            mc.playerBornRote = new Vector3(float.Parse(valArr3[0]), float.Parse(valArr3[1]), float.Parse(valArr3[2]));
                            break;
                    }
                }
                mapCfgDataDic.Add(ID,mc);
            }
        }
    }
    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id,out data))
        {
            return data;
        }
        return null;
    }

    #endregion
    #endregion
    private Dictionary<string,GameObject> goDic = new Dictionary<string, GameObject> ();
    public GameObject LoadPrefab(string path,bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path,out prefab))
        {
            prefab  = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path,prefab);
            }
        }
        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }
        return go;
    }
}
