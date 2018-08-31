using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance;

    protected virtual void Awake()
    {
        Singleton<T>.Instance = this as T;
    }

    public static bool IsInstantiated { get { return Instance != null; } }
}