using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

//文件后缀
public enum Suffix
{
    jpg,
    png,
    json,
    txt,
    xml,
    zip,
    rar
}

/// <summary>
/// 功能描述(Description)：	文件工具类
/// 作者(Author)：			Ascendashacker
/// 日期(Create Date)：		2019/2/18 15:31:39
public class FileUtils
{
   
    /// <summary>
    /// 保存文件
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">实体类数据</param>
    /// <param name="suffix">文件后缀:.txt/.png/.json</param>
    /// <param name="directoryPath">文件所在的文件夹路径</param>
    /// <param name="fileName">文件名称</param>
    /// <returns>文件保存是否成功</returns>
    public static bool SaveFile<T>(T data, Suffix suffix, string directoryPath, string fileName) where T : new()
    {
        string json = null;
        try
        {
            json = JsonMapper.ToJson(data);
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("{0} file save failed", fileName);
            return false;
        }
        return SaveFile(json, suffix, directoryPath, fileName);
    }

    public static bool SaveFile(string data, Suffix suffix, string directoryPath, string fileName)
    {
        byte[] bytes = null;
        try
        {
            bytes = System.Text.Encoding.UTF8.GetBytes(data);
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("{0} file save failed", fileName);
            return false;
        }
        return SaveFile(bytes, suffix, directoryPath, fileName);
    }

    public static bool SaveFile(byte[] bytes, Suffix suffix, string directoryPath, string fileName)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string filePath = string.Format("{0}/{1}{2}", directoryPath, fileName, GetSuffix(suffix));
        try
        {
            File.WriteAllBytes(filePath, bytes);
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("{0} file save failed", fileName);
            return false;
        }
        return true;
    }
    public static bool SaveFile(Texture2D tex, Suffix suffix, string directoryPath, string fileName)
    {
        byte[] bytes = null;
        try
        {
            switch (suffix)//暂时只支持两种常用格式
            {
                case Suffix.jpg:
                    bytes = tex.EncodeToJPG();
                    break;
                case Suffix.png:
                    bytes = tex.EncodeToPNG();
                    break;
                default:
                    break;
            }
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("{0} file save failed", fileName);
            return false;
        }
        return SaveFile(bytes, suffix, directoryPath, fileName);
    }

    public static bool DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }
        try
        {
            File.Delete(filePath);
        }
        catch (Exception)
        {
            Debug.LogErrorFormat("Failed to delete file, the filePath is {0}", filePath);
            return false;
        }
        return true;
    }
    public static bool DeleteFiles(string directoryPath, Suffix suffix)
    {
        if (!Directory.Exists(directoryPath))
        {
            return false;
        }
        DirectoryInfo directory = new DirectoryInfo(directoryPath);
        string s = string.Format("*.{0}", suffix.ToString());
        FileInfo[] files = directory.GetFiles(s, SearchOption.TopDirectoryOnly);
        bool isSuccess = true;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] == null)
            {
                continue;
            }
            try
            {
                files[i].Delete();
            }
            catch (Exception)
            {
                Debug.LogErrorFormat("Failed to delete file, the filePath is {0}", files[i].FullName);
                isSuccess = false;
            }
        }
        return isSuccess;
    }

    /// <summary>
    /// 获取文件中的实体类数据
    /// suffix只适用于txt json 
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    /// <param name="directoryPath">文件夹路径</param>
    /// <param name="suffix">后缀名</param>
    /// <param name="callback">回调</param>
    public static void GetDtoFiles<T>(string directoryPath, Suffix suffix, Action<List<T>> callback) where T : new()
    {
        Assert.IsNotNull(callback);
        if (string.IsNullOrEmpty(directoryPath))
        {
            return;
        }
        DirectoryInfo dir = new DirectoryInfo(directoryPath);
        if (!dir.Exists)
        {
            return;
        }
        List<T> list = new List<T>();
        string s = string.Format("*.{0}", suffix.ToString());
        FileInfo[] files = dir.GetFiles(s);
        foreach (var file in files)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file.FullName);
                string str = sr.ReadToEnd();
                T t = JsonMapper.ToObject<T>(str);
                list.Add(t);
            }
            catch (Exception)
            {
                Debug.LogErrorFormat(" Failed to convert file specific information to entity class,the file path is {0}", file.FullName);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
            }
        }
        callback(list);
    }

    public static string GetSuffix(Suffix suffix)
    {
        return string.Format(".{0}", suffix.ToString());
    }

    /// <summary>
    /// 将文件转换成byte[] 数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]</returns>
    public static byte[] GetFileByteData(string fileUrl)
    {
        return File.ReadAllBytes(fileUrl);
    }

    /// <summary>
    /// 将文件转换成byte[] 数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]</returns>
    public static byte[] AuthGetFileData(string fileUrl)
    {
        using (FileStream fs = new FileStream(fileUrl, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            byte[] buffur = new byte[fs.Length];
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(buffur);
                bw.Close();
            }
            return buffur;
        }
    }
}

