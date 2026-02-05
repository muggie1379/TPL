using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalEventSystem : MonoBehaviour
{
    private static GlobalEventSystem _instance;
    public static GlobalEventSystem Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 유지
    }
}
