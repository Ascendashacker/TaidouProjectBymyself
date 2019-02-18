/// <summary>
/// 功能描述(Description)：	单例基类
/// 作者(Author)：			Ascendashacker
/// 日期(Create Date)：		2019/2/18 15:31:39
public class Singleton<T> where T : new()
{
    public static T Instance
    {
        get { return SingletonCreator.instance; }
    }
    class SingletonCreator
    {
        internal static readonly T instance = new T();
    }
}
