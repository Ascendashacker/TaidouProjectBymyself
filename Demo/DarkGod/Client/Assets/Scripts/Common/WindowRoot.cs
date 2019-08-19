using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class WindowRoot : MonoBehaviour
{
    [HideInInspector]
    protected ResSvc resSvc = null;
    [HideInInspector]
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    public void SetWndState(bool isActive =true)
    {
        if (gameObject.activeSelf!=isActive)
        {
            gameObject.SetActive(isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
    }

    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }

    protected void SetText(Transform trans,int num = 0)
    {
        SetText(trans.GetComponent<Text>(),num);
    }

    protected void SetText(Transform trans,string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    
    protected T GetOrAddComponet<T>(GameObject GO) where T:Component
    {
        T t = GO.GetComponent<T>();
        if (t == null)
        {
            t = GO.AddComponent<T>();
        }
        return t;
    }
    #region 点击事件

    protected void OnClickDown(GameObject go,Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponet<PEListener>(go);
        listener.onclickDown = callback;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponet<PEListener>(go);
        listener.onclickUp = callback;
    }
    protected void OnDrag(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponet<PEListener>(go);
        listener.onDrag = callback;
    }
    #endregion
}
