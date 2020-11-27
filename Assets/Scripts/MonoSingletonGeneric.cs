using UnityEngine;

public class MonoSingletonGeneric<T> : MonoBehaviour where T: MonoSingletonGeneric<T>
{
    static T instance;
    static object m_lock = new object();
    public static T GetInstance()
    {
        lock (m_lock)
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }
        }
        return instance;
    }
}
