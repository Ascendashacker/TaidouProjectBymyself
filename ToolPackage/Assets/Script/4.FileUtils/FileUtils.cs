using System.IO;
/// <summary>
/// 功能描述(Description)：	单例基类
/// 作者(Author)：			Ascendashacker
/// 日期(Create Date)：		2019/2/18 15:31:39
public class FileUtils: Singleton<FileUtils> {

    /// <summary>
    /// 将文件转换成byte[] 数组
    /// </summary>
    /// <param name="fileUrl">文件路径文件名称</param>
    /// <returns>byte[]</returns>
    public byte[] GetFileByteData(string fileUrl)
    {
        return File.ReadAllBytes(fileUrl);
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

