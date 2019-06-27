using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class DeepCopyUtils
{
    public static T DeepCopyByReflect<T>(T obj) //有问题
    {
        //如果是字符串或值类型则直接返回
        if (obj is string || obj.GetType().IsValueType) return obj;

        object retval = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (FieldInfo field in fields)
        {
            try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
            catch { }
        }
        return (T)retval;
    }
    public static T DeepCopyByXml<T>(T obj) //可用
    {
        object retval;
        using (MemoryStream ms = new MemoryStream())
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            xml.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            retval = xml.Deserialize(ms);
            ms.Close();
        }
        return (T)retval;
    }

    public static T DeepCopyByBin<T>(T obj)
    {
        object retval;
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            //序列化成流
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            //反序列化成对象
            retval = bf.Deserialize(ms);
            ms.Close();
        }
        return (T)retval;
    }
}




