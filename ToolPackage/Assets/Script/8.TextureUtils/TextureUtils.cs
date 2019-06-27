using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TextureUtils
{
    /// <summary>
    /// 混合前4张贴图
    /// </summary>
    /// <param name="texList"></param>
    /// <param name="callback"></param>
    public static void BlendMap(List<Texture2D> texList, Action<Texture2D> callback)
    {
        Assert.IsNotNull(callback);
        GetMaxResolution(texList, (maxWidth, maxHight) => {
            if (texList == null || texList.Count == 0)
            {
                callback(null);
                return;
            }
            Texture2D blendTex = new Texture2D(maxWidth * 2, maxHight * 2);
            //Color defalut = blendTex.GetPixel(0, 0); //r:0.8039216 g:0.8039216 b:0.8039216 a:0.8039216
            for (int i = 0; i < texList.Count; i++)
            {
                Texture2D t2d = ScaleTexture(GetReadableT2d(texList[i]), maxWidth, maxHight);
                if (i == 0)
                {
                    blendTex.SetPixels(0, maxHight, t2d.width, t2d.height, t2d.GetPixels());
                }
                else if (i == 1)
                {
                    blendTex.SetPixels(maxWidth, maxHight, t2d.width, t2d.height, t2d.GetPixels());
                }
                else if (i == 2)
                {
                    blendTex.SetPixels(0, 0, t2d.width, t2d.height, t2d.GetPixels());
                }
                else if (i == 3)
                {
                    blendTex.SetPixels(maxWidth, 0, t2d.width, t2d.height, t2d.GetPixels());
                    break;
                }
            }
            //for (int i = 0; i < blendTex.width; i++)
            //{
            //    for (int j = 0; j < blendTex.height; j++)
            //    {
            //        Color color = blendTex.GetPixel(i, j);
            //        if (blendTex.GetPixel(i, j).a <= 0.5 || color == defalut) //将透明和t2d初始色都改成白色显示
            //        {
            //            blendTex.SetPixel(i, j, Color.white);
            //        }
            //    }
            //}
            blendTex.Apply();
            callback(blendTex);
        });
    }

    /// <summary>
    /// 贴图缩放到指定分辨率
    /// </summary>
    /// <param name="source">贴图</param>
    /// <param name="targetWidth">目标宽</param>
    /// <param name="targetHeight">目标高</param>
    /// <returns>目标贴图</returns>
    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        return result;
    }

    /// <summary>
    /// 获取可读写的贴图(通过rendertexture)
    /// 某些unity外部的贴图是无法读写的
    /// </summary>
    /// <param name="source">贴图源</param>
    /// <returns>可读写的贴图</returns>
    public static Texture2D GetReadableT2d(Texture2D source) //拿到可读写的贴图
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Default);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    /// <summary>
    /// 获取贴图列表中最大的宽高分辨率
    /// </summary>
    /// <param name="texList">贴图列表</param>
    /// <param name="callback">回调(宽，高)</param>
    public static void GetMaxResolution(List<Texture2D> texList, Action<int, int> callback)
    {
        Assert.IsNotNull(callback);
        int maxWidth = 0;
        int maxHight = 0;
        foreach (var tex in texList)
        {
            if (tex.width > maxWidth)
            {
                maxWidth = tex.width;
            }
            if (tex.height > maxHight)
            {
                maxHight = tex.height;
            }
        }
        callback(maxWidth, maxHight);
    }
}





