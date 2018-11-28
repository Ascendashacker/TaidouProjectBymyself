using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Unity加载贴图的几种方式
/// </summary>
public class LoadImage : MonoBehaviour {
    /* 1.WWW方式
     * 2.IO方式
     * 3.Resource.load()方式
     * 
     * 这三种方法的区别？文件路径有何要求
     */

    //费曼学习法 想象对方会碰到的问题  将问题罗列化  一个一个举例说明情况
    //www的方式必须加file://
    //扩展加入 选择文件面板 给出文件路径
    //趁这个机会搞懂 @"C:\Users\Ascendashacker\Desktop\11.jpg"和反斜杠的区别file://D:/SOmeOther/NGUI/Assets/StreamingAssets/
    public Image image1;
    public Image image2;
    public Image image3;
    private string loadpath = "D:Texture/1.jpg"; //IO方式加载的路径
    private string picpathWWW = "1.jpg"; //WWW的加载方式路径

    // Use this for initialization
    private void Start()
    {
        //1.IO方式 加载速度快
        LoadByIO();
        //2.WWW方式 加载速度慢
        LoadByWWW();
        //3.Resource.load()方式
        LoadByResouceLoad();
    }

    /// <summary>
    /// 2.Unity内置的方式，只能调用项目内Resouce文件夹底下的资源
    /// </summary>
    private void LoadByResouceLoad()
    {
        //参数为Resource文件夹底下的相对路径
        //Texture/1.jpg  路径加文件后缀，无法正确获取文件
        Texture2D aa = (Texture2D)Resources.Load("Texture/1") as Texture2D;
        Sprite kk = Sprite.Create(aa, new Rect(0, 0, aa.width, aa.height), new Vector2(0.5f, 0.5f));
        image3.sprite = kk;
    }
    /// <summary>
    /// 1.IO方式加载贴图
    /// </summary>
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
        int width = 1;
        int height = 1;
        //创建贴图
        //t2d的长宽和贴图像素大小有关系？如果有，是什么关系
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);

        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
            new Vector2(0.5f, 0.5f));
        image1.sprite = sprite;
        double time = (double)Time.time - startTime;
        Debug.Log("IO加载用时：" + time);
    }

    /// <summary>
    /// 3.WWW方式加载贴图
    /// </summary>
    private void LoadByWWW()
    {
        StartCoroutine(Load());
    }

    //  file://C:/Users/Ascendashacker/Desktop/11.jpg 这个路径是读不出来的
    //  file://C:\\Users\\Ascendashacker\\Desktop\\11.jpg 这个路径读的出来
    //  C:\\Users\\Ascendashacker\\Desktop\\11.jpg  这个路径读不出来
    private string url = "file://D:\\Texture\\1.jpg";
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

            image2.sprite = sprite;
            double time = (double)Time.time - startTime;
            Debug.Log("WWW加载用时：" + time);
        }
    }
	
}
