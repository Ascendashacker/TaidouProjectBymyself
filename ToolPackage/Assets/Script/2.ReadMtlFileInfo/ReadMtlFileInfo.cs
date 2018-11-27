using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class ReadMtlFileInfo : MonoBehaviour
{
    /// <summary>
    /// 材质球名称集合
    /// 当一个模型有多个mtl文件时，看业务需要进行去重操作
    /// </summary>
    private List<string> matNameList;

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// 加载mlt文件
    /// 此处会介绍几种加载文件的方式
    /// </summary>
    private void LoadLocalMtlFile()
    {
        string path = Application.streamingAssetsPath;
        DirectoryInfo dir = new DirectoryInfo (path);
        FileInfo[] mtlFiles = dir.GetFiles("*.mtl");
        matNameList = new List<string>();
        foreach (var mtlFile in mtlFiles)
        {
            string[] lns = File.ReadAllLines(mtlFile.FullName);
            //判断mlt材质球名称是否已经存在。一个模型下来有很多个mtl文件。每个部位可能用到相同的材质球，那么mlt文件里面就有相同的材质球信息，此处只针对多个mtl文件，单个mtl不存在材质球名称重复的情况。
            bool isExist = false;  
            //定义一个临时变量lns和直接将方法写在循环体，性能是否有差别？string ln in File.ReadAllLines(mtlFile.FullName)
            foreach (string ln in lns) //fullName 和name的区别
            {
                string l =ln.Trim().Replace("  "," ");
                string [] cmps = l.Split(' ');
                string data = cmps[cmps.Length-1];
                if (cmps[0].Equals("newmtl"))
                {
                    isExist = false;
                    if (true)
                    {
                        matNameList.Add(data);
                    }
                }
            }
        }

    }
}
