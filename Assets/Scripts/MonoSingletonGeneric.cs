using UnityEngine;

public class MonoSingletonGeneric<T> : MonoBehaviour where T: MonoSingletonGeneric<T>
{
    private static T instance;

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Debug.LogError("Someone is trying to create a duplicate singleton");
            Destroy(this);
        }
    }
}
