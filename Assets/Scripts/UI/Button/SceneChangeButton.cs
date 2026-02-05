using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private SceneEnum scene;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickSceneChange);
    }

    private void OnClickSceneChange()
    {
        // Scene으로 전환
        GameManager.Instance.Scene.LoadScene(scene);
    }
}