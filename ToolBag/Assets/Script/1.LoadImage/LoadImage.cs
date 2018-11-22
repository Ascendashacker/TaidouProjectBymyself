using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour {
    /*该脚本介绍Unity加载贴图的几种方式*/
    //费曼学习法 想象对方会碰到的问题  将问题罗列化  一个一个举例说明情况
    //www的方式必须加file://
    //扩展加入 选择文件面板 给出文件路径
    //趁这个机会搞懂 @"C:\Users\Ascendashacker\Desktop\11.jpg"和反斜杠的区别file://D:/SOmeOther/NGUI/Assets/StreamingAssets/
    private Image image;
    private string loadpath = "C:/Users/Ascendashacker/Desktop/11.jpg"; //IO方式加载的路径
    private string picpathWWW = "11.jpg"; //WWW的加载方式路径

    // Use this for initialization
    private void Start()
    {
        image = GetComponent<Image>();
        //IO方法加载速度快
        //        LoadByIO();
        //WWW 加载速度慢
        LoadByWWW();

    }

    private void LoadByIO()
    {
        double startTime = (double)Time.time;
        //创建文件流
        FileStream fileStream = new FileStream(loadpath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度的缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取liu
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 300;
        int height = 372;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);

        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
        double time = (double)Time.time - startTime;
        Debug.Log("IO加载用时：" + time);
    }


    private void LoadByWWW()
    {
        StartCoroutine(Load());
    }

    //  file://C:/Users/Ascendashacker/Desktop/11.jpg 这个路径是读不出来的
    //  file://C:\\Users\\Ascendashacker\\Desktop\\11.jpg 这个路径读的出来
    //  C:\\Users\\Ascendashacker\\Desktop\\11.jpg  这个路径读不出来
    private string url = "file://C:\\Users\\Ascendashacker\\Desktop\\11.jpg";
    //private string url = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1502532130856&di=7135149ed906483861efdfc9770def3b&imgtype=0&src=http%3A%2F%2Fwww.newasp.net%2Fattachment%2Fsoft%2F2017%2F0811%2F144057_83971519.png"; 这里当然可以换做网络图片的URL 就加载网络图片了
    private IEnumerator Load()
    {
        double startTime = (double)Time.time;
        //Debug.LogError("该文件是否存在"+File.Exists(url));
        WWW www = new WWW(url);//只能放URL
        //WWW www = new WWW(url);//只能放URL 这里可以换做网络的URL
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {

            Texture2D texture = www.texture;
            //创建 Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            image.sprite = sprite;
            double time = (double)Time.time - startTime;
            Debug.Log("WWW加载用时：" + time);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
