using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _uniqueInstance;

    public static T Instance
    {
        get { return _uniqueInstance; }
        set
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = value;
                DontDestroyOnLoad(_uniqueInstance.gameObject);
            }
            else if (_uniqueInstance != value)
            {
                DestroyImmediate(value.gameObject);
                Debug.LogWarning("Singleton duplicado destruido!!");
            }
        }
    }

    // Awake is called when the script instance is being loaded
    protected virtual void Awake()
    {
        //_uniqueInstance = this as T;
        //Debug.LogWarning("Instancia singleton iniciada");
        if (Instance == null)
            Instance = this as T;
        else
        {
            DestroyImmediate(this.gameObject);
            Debug.LogWarning("Singleton duplicado destruido!!");
        }
    }

    private void Reset()
    {
#if UNITY_EDITOR
        //Debug.LogWarning("Instancia singleton resetada");
        //if (_uniqueInstance != null)
        //{
        //    Debug.LogWarning("Instancia singleton resetada");
        //    DestroyImmediate(this.gameObject);
        //}

        if (_uniqueInstance == null)
            _uniqueInstance = this as T;
        else
        {
            DestroyImmediate(this.gameObject);
            Debug.LogWarning("Singleton duplicado destruido!!");
        }

#endif
    }



    // This function is called when the MonoBehaviour will be destroyed
    protected virtual void OnDestroy()
    {
        if (_uniqueInstance == this)
            _uniqueInstance = null;
    }


}
