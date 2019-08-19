using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private  Transform camTrans;
    public Animator ani;
    public CharacterController ctrl;
    private Vector3 camOffset;
    private Vector2 dir = Vector2.zero;
    private bool isMove =false;
    private float targetBlend;
    private float curBlend;
    
    public Vector2 Dir
    {
        get
        {
            return dir;
        }

        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    // Use this for initialization
    public void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO 如果是PC 用方向键控制 或WASD


        /*
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2 (h,v).normalized;
        if (_dir != Vector2.zero) 
        {
            Dir =_dir;
            SetBlend(Constants.BlendWalk);
        }
        else
        {
            SetBlend(Constants.BlendIdle);
            Dir =Vector2.zero;
        }
        */

        if (curBlend != targetBlend)
        {
            UpdateMixBlend();
        }
        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir,new Vector2 (0,1)) + camTrans.eulerAngles.y;
        Vector3 eularAngles = new Vector3 (0,angle,0);
        transform.localEulerAngles = eularAngles;
    }
    private void SetMove()
    {
        ctrl.Move(transform.forward *Time.deltaTime*Constants.PlayerMoveSpeed);
    }
    private void SetCam()
    {
        if (camTrans!=null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(curBlend -targetBlend) < Constants.AccelerSpeed*Time.deltaTime)
        {
            curBlend = targetBlend;
        }
        else if(curBlend >targetBlend)
        {
            curBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            curBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", curBlend);
    }
}
