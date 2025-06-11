using UnityEngine;

/// <summary>
/// Generic Singleton base class for MonoBehaviour singletons.
/// </summary>
/// <typeparam name="T">Type of the singleton class.</typeparam>
public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool isDontDestroyOnLoad = false;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
           if(isDontDestroyOnLoad == true)
                DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
