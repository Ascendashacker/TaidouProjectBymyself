using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoBehaviour {
    public static AudioSvc Instance = null;
    public AudioSource bgAudio;
    public AudioSource uiAudio;
    public void InitSvc()
    {
        Instance = this;
    }
    public void PlayBGMusic(string name,bool isLoop = true)
    {
        AudioClip audioClip = ResSvc.Instance.LoadAudio("ResAudio/"+ name,true);
        if (bgAudio.clip == null || bgAudio.clip.name != audioClip.name)
        {
            bgAudio.clip = audioClip;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {

        AudioClip audioClip = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audioClip;
        uiAudio.Play();
    } 
}
