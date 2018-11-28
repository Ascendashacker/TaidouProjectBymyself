using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadTextrueInfoUtils : MonoBehaviour
{
    void Start()
    {

    }

    /// <summary>
    /// 获取贴图的主色调
    /// 方法有很多种，主要是性能问题
    /// </summary>
    /// <param name="mapPath">贴图路径</param>
    /// <param name="callback">回调函数</param>
    private void GetMostColor(string mapPath, Action<Color> callback)
    {
        if (!File.Exists(mapPath))
        {
            Debug.LogErrorFormat("{0}的贴图不存在,请先检查素材", mapPath);
            return;
        }
        Texture2D t2d = new Texture2D(1,1);
        t2d.LoadImage(File.ReadAllBytes(mapPath));
        Dictionary<Color, int> colorList = new Dictionary<Color, int>();  //字典查找和列表遍历查找性能差异
        Color[] colors = t2d.GetPixels();
        Color mostColor = Color.white;
        int max = -1;
        foreach (var color in colors)
        {
            if (colorList.ContainsKey(color))
            {
                colorList[color]++;
                int num = colorList[color];
                if (num > max)
                {
                    max = num;
                    mostColor = color;
                }
            }
            else
            {
                colorList.Add(color, 1);
            }
        }
        callback(mostColor);
    }

    /// <summary>
    /// unity方式读取贴图是否含有透明信息
    /// 一些地方需要根据贴图是否是透明贴图设置材质为透明材质
    /// </summary>
    /// <param name="mapPath">贴图路径</param>
    /// <returns></returns>
    private bool? GetTexFormat(string mapPath)
    {
        if (!File.Exists(mapPath))
        {
            Debug.LogErrorFormat("{0}的贴图不存在,请先检查素材", mapPath);
            return null;
        }
        Texture2D t2d = new Texture2D(1, 1);
        t2d.LoadImage(File.ReadAllBytes(mapPath));
        string selfFormat = t2d.format.ToString();
        for (int i = 0; i < transparentFormat.Length; i++)
        {
            if (selfFormat == transparentFormat[i])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 透明格式
    /// </summary>
    private string[] transparentFormat = {
                TextureFormat.Alpha8.ToString(),
                TextureFormat.ARGB32.ToString(),
                TextureFormat.ARGB4444.ToString(),
                TextureFormat.ATC_RGBA8.ToString(),
                TextureFormat.BGRA32.ToString(),
                TextureFormat.PVRTC_RGB2.ToString(),
                TextureFormat.PVRTC_RGB4.ToString(),
                TextureFormat.RGBA32.ToString(),
                TextureFormat.RGBA4444.ToString()
            };
}
