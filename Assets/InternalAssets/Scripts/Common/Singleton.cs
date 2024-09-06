using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                instance?.Initialize();
            }
            return instance;
        }
    }

    private static T instance;

    public bool dontDestroyOnLoad = false;

    private bool initialized = false;

    protected virtual void Awake()
    {
        if (!dontDestroyOnLoad)
        {
            instance = (T)this;
            Initialize();
            return;
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = (T)this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        if (initialized) return;

        OnIntialized();

        initialized = true;
    }

    protected virtual void OnIntialized()
    {

    }
}