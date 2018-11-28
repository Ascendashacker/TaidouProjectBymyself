using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class ReadMtlFileInfoUtils : MonoBehaviour
{

    /// <summary>
    /// Untiy端标准材质球
    ///  当一个模型有多个mtl文件时，看业务需要进行去重操作
    /// </summary>
    private List<Material> matList;
    private Shader shader;
    void Start()
    {
        shader = Shader.Find("Standard");
        string path = Application.streamingAssetsPath + "/WebFabMtlList";
        //1.读取一个模型的所有mtl文件信息
        ReadModelMtlFilesInfo(path);
        //2.读取单个mtl文件信息
        //path = "";
        //ReadMtlFileInfo(path);
    }

    /// <summary>
    /// 读取单个mtl文件的信息
    /// 一个mtl文件里面有多个不同材质球数据
    /// </summary>
    /// <param name="mtlFilePath">mtl文件路径</param>
    private List<Material> ReadMtlFileInfo(string mtlFilePath)
    {
        string[] lns = File.ReadAllLines(mtlFilePath); 
        //判断mlt材质球名称是否已经存在。一个模型下来有很多个mtl文件。每个部位可能用到相同的材质球，那么mlt文件里面就有相同的材质球信息，此处只针对多个mtl文件，单个mtl不存在材质球名称重复的情况。
        //bool isExist = false;
        List<Material> matList = new List<Material>();
        Material curMat = null;
        //定义一个临时变量lns和直接将方法写在循环体，性能是否有差别？string ln in File.ReadAllLines(mtlFile.FullName)
        foreach (string ln in lns)
        {
            string l = ln.Trim().Replace("  ", " ");//mtl文件中，每一行的字符串基本上都是单个空格间隔，此处防止有两个属性之前有多个空格的情况导致解析错误。
            string[] cmps = l.Split(' '); //按照单个空格拆分字符串。 属性 属性值 属性值 属性 属性值 属性
            string data = cmps[cmps.Length - 1];  //每行最后一个字符串，主要用来读取材质球名称
            if (cmps[0].Equals("newmtl"))
            {
                if (curMat!=null)
                {
                    matList.Add(curMat);
                }
                else
                {
                    curMat = new Material(shader);
                    curMat.name = data;
                }
            }
            if (cmps[0] == "Kd")
            {
                curMat.SetColor("_Color", ParseColorFromCMPS(cmps));
            }
            else if (cmps[0] == "map_Kd")
            {
                SetTexture(curMat, "_MainTex", cmps, data);
            }
            else if (cmps[0] == "map_Bump")
            {
                SetTexture(curMat, "_BumpMap", cmps, data);
                //curMat.SetFloat("_BumpScale",float.Parse(cmps[2]));  设置凹凸强度
                curMat.EnableKeyword("_NORMALMAP");
            }
            else if (cmps[0] == "Ks")
            {
                curMat.SetColor("_SpecColor", ParseColorFromCMPS(cmps));
            }
            else if (cmps[0] == "Ka")
            {
                curMat.SetColor("_EmissionColor", ParseColorFromCMPS(cmps));
                curMat.EnableKeyword("_EMISSION");
            }
            else if (cmps[0] == "d")
            {
                float visibility = float.Parse(cmps[1]);
                Color temp = curMat.color;
                temp.a = visibility;
                curMat.SetColor("_Color", temp);
            }
            else if (cmps[0] == "Ns")
            {
                float Ns = float.Parse(cmps[1]);
                Ns = (Ns / 1000);
                curMat.SetFloat("_Glossiness", Ns);
            }
        }
        if (curMat != null)
        {
            matList.Add(curMat);
        }
        return matList;
    }

    /// <summary>
    /// 读取一个模型底下所有mtl文件信息
    /// 一个模型不同部位的mtl文件里面的材质球有可能共用同一个材质球，此时需要去重
    /// </summary>
    /// <param name="mtlFolderPath">存放一个模型所有mtl的文件夹路径</param>
    /// <returns>材质球集合</returns>
    private List<Material> ReadModelMtlFilesInfo(string mtlsFolderPath)
    {
        if (string.IsNullOrEmpty(mtlsFolderPath))
        {
            Debug.LogError("路径为空");
            return null;
        } 
        DirectoryInfo dir = new DirectoryInfo(mtlsFolderPath);
        if (!dir.Exists)
        {
            Debug.LogErrorFormat("请检查mtl文件路径{0}是否正确", mtlsFolderPath);
            return null;
        }
        List<Material> matList = new List<Material>();
        FileInfo[] mtlFiles = dir.GetFiles("*.mtl");
        Material curMat = null;
        foreach (var mtlfile in mtlFiles)
        {
            bool isExist = false;
            string[] lns= File.ReadAllLines(mtlfile.FullName);
            foreach (var ln in lns)
            {
                string l = ln.Trim().Replace("  ", " ");
                string[] cmps = l.Split(' ');
                string data = cmps[cmps.Length - 1];
                if (cmps[0] == "newmtl")
                {
                    isExist = false;
                    if (curMat != null)
                    {
                        foreach (var mat in matList)
                        {
                            if (mat.name.Equals(curMat.name))
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            matList.Add(curMat);
                        }
                    }
                    isExist = false;
                    foreach (var mat in matList)
                    {
                        if (mat.name.Equals(data))
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        curMat = new Material(shader);
                        curMat.name = data;
                    }
                }
                if (isExist)
                {
                    continue;
                }
                if (cmps[0] == "Kd")
                {
                    curMat.SetColor("_Color", ParseColorFromCMPS(cmps));
                }
                else if (cmps[0] == "map_Kd")
                {
                    SetTexture(curMat, "_MainTex", cmps, data);
                }
                else if (cmps[0] == "map_Bump")
                {
                    SetTexture(curMat, "_BumpMap", cmps, data);
                    //curMat.SetFloat("_BumpScale",float.Parse(cmps[2]));  设置凹凸强度
                    curMat.EnableKeyword("_NORMALMAP");
                }
                else if (cmps[0] == "Ks")
                {
                    curMat.SetColor("_SpecColor", ParseColorFromCMPS(cmps));
                }
                else if (cmps[0] == "Ka")
                {
                    curMat.SetColor("_EmissionColor", ParseColorFromCMPS(cmps));
                    curMat.EnableKeyword("_EMISSION");
                }
                else if (cmps[0] == "d")
                {
                    float visibility = float.Parse(cmps[1]);
                    Color temp = curMat.color;
                    temp.a = visibility;
                    curMat.SetColor("_Color", temp);
                }
                else if (cmps[0] == "Ns")
                {
                    float Ns = float.Parse(cmps[1]);
                    Ns = (Ns / 1000);
                    curMat.SetFloat("_Glossiness", Ns);
                }
            }
            if (!isExist && curMat != null)
            {
                matList.Add(curMat);
            }
        }
        return matList;
    }

    /// <summary>
    /// 设置贴图
    /// 贴图文件必须放在指定路径下，mtl文件里面的贴图路径只是一个相对路径
    /// </summary>
    /// <param name="curMat">当前材质球</param>
    /// <param name="attName">属性名称</param>
    /// <param name="cmps">mtl文件里面每一行的string</param>
    /// <param name="texPath">贴图相对路径</param>
    /// <param name="folderPath">文件夹路径前缀</param>
    private void SetTexture(Material curMat, string attName, string[] cmps, string texPath)
    {
        string path = "D:/maps/" + texPath.Split('\\')[texPath.Split('\\').Length - 1].Split('.')[0];
        //Texture tex = Resources.Load(path) as Texture;
        if (!File.Exists(path))
        {
            Debug.LogErrorFormat("请检查贴图路径{0}是否正确", path);
            return;
        }
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(File.ReadAllBytes(path));
        curMat.SetTexture(attName, tex);
        if (cmps.Length >= 10)//有可能无平铺,待优化判断条件
        {
            curMat.SetTextureOffset(attName, new Vector2(float.Parse(cmps[2]), float.Parse(cmps[3])));
            curMat.SetTextureScale(attName, new Vector2(float.Parse(cmps[6]), float.Parse(cmps[7])));
        }
    }

    /// <summary>
    /// 读取mtl文件里面的颜色信息
    /// </summary>
    /// <param name="cmps">mtl文本里面每行的字符</param>
    /// <returns></returns>
    private Color ParseColorFromCMPS(string[] cmps)
    {
        return new Color(float.Parse(cmps[1]), float.Parse(cmps[2]), float.Parse(cmps[3]));
    }
}
