using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DynamicWnd : WindowRoot
{
    public Animation tipsAni;
    public Text tipsTxt;
    private Queue<string> tipQueue = new Queue<string>();
    private bool isTipsShow = false;

    protected override void InitWnd()
    {
        base.InitWnd();
        tipsTxt.gameObject.SetActive(false);
    }
    public void AddTips(string tips)
    {
        lock (tipQueue)
        {
            tipQueue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if (tipQueue.Count >0 && isTipsShow == false)
        {
            lock (tipQueue)
            {
                string tips = tipQueue.Dequeue();
                SetTips(tips);
                isTipsShow = true;
            }
        }
    }
    private void SetTips(string tips)
    {
        tipsTxt.gameObject.SetActive(true);
        SetText(tipsTxt,tips);
        AnimationClip clip = tipsAni.GetClip("TipsShowAmi");
        tipsAni.Play();
        //延迟关闭激活状态
        StartCoroutine(AniPlayerDone(clip.length,()=>{
            tipsTxt.gameObject.SetActive(false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayerDone(float sec,Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb!=null)
        {
            cb();
        }
    }
}
