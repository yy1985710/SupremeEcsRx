public class Singleton<T> where T : new()
{
    private static T instance;
    private static object _lock = new object();

    private Singleton()
    {

    }

    public static T Instance
    {
        get{
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
       
    }
}