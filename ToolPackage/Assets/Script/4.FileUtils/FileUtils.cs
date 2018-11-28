
using System;
using System.IO;

public class FileUtils: Singleton<FileUtils> {
    /// <summary>
    /// 将文件转换成byte[] 数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]</returns>
    public byte[] GetFileByteData(string fileUrl)
    {
        FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
        try
        {
            byte[] buffur = new byte[fs.Length];
            fs.Read(buffur, 0, (int)fs.Length);
            return buffur;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            if (fs != null)
            {
                //关闭资源
                fs.Close();
            }
        }
    }

    /// <summary>
    /// 将文件转换成byte[] 数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]</returns>
    public byte[] AuthGetFileData(string fileUrl)
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

