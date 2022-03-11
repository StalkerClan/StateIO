using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            instance = FindObjectOfType<T>();
            if (instance != null)
            {
                return instance;
            }
            else
            {
                GameObject gameObject = new GameObject();
                instance = gameObject.AddComponent<T>();
                gameObject.name = typeof(T).ToString();
                return instance;
            }
        }
    }
}
