using UnityEngine;
using System.Collections;

public class LoopDragonAni : MonoBehaviour
{
    private Animation animation;
    // Use this for initialization
    private void Awake()
    {
        animation = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        if (animation!=null)
        {
            InvokeRepeating("PlayDragonAni", 0,20);
        }
    }
    private void PlayDragonAni()
    {
        if (animation!=null)
        {
            animation.Play();
        }
    }
}
