using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneChanger : Singleton<SceneChanger>
{
    private bool isChanging;
    public bool IsChanging => isChanging;
    public SceneEnum currentScene { get; private set; } = SceneEnum.None;

    // 로딩 UI용 이벤트
    public event Action<float> OnLoadingProgress;
    public event Action OnLoadingStarted;
    public event Action OnLoadingFinished;

    public void LoadScene(SceneEnum scene)
    {
        if (isChanging) return;

        string sceneName = scene.ToString();
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[SceneChanger] {sceneName} 씬이 Build Settings 에 없습니다!");
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
        currentScene = scene;
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isChanging = true;

        // GameState 진입
        GameManager.Instance.SetState(GameState.Loading);
        OnLoadingStarted?.Invoke();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            OnLoadingProgress?.Invoke(op.progress);
            yield return null;
        }

        // 90% 이후는 연출용
        OnLoadingProgress?.Invoke(1f);
        yield return new WaitForSeconds(0.2f);

        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        if (Enum.TryParse(sceneName, out SceneEnum sceneEnum))
        {
            GameManager.Instance.OnSceneEntered(sceneEnum);
        }
        else
        {
            Debug.LogWarning($"[SceneChanger] SceneEnum 매핑 실패: {sceneName}");
        }

        isChanging = false;
        OnLoadingFinished?.Invoke();
    }
}
