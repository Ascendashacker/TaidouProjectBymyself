/// <summary>
/// 单例基类
/// </summary>
/// <typeparam name="T"></typeparam>
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
