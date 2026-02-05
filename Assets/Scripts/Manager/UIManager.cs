using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GlobalUIManager : Singleton<GlobalUIManager>
{
    #region Global
    #region SerializeFields
    [SerializeField] private Canvas uiCanvas;

    [Header("Currency UI")]
    [SerializeField] private GameObject currencyPanel;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;

    [Header("Popup Prefabs")]
    [SerializeField] private Transform popupRoot;
    [SerializeField] private GameObject blockerPanel;
    [SerializeField] private GameObject popupOKPrefab;
    [SerializeField] private GameObject popupYesNoPrefab;
    [SerializeField] private GameObject popupOptionPrefab;

    #endregion

    private PopupOK popupOK;
    private PopupYesNo popupYesNo;
    private PopupOption popupOption;

    private int modalCount = 0;
    #endregion

    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();

        Debug.Log($"[UIManager Awake] {GetInstanceID()}");

        if (uiCanvas != null)
        {
            uiCanvas.sortingLayerName = "UI";
            uiCanvas.sortingOrder = 100;
        }

        LoadPopups();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged += HandleGameState;

        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.OnLoadingStarted += OnLoadingStarted;
            SceneChanger.Instance.OnLoadingProgress += OnLoadingProgress;
            SceneChanger.Instance.OnLoadingFinished += OnLoadingFinished;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= HandleGameState;

        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.OnLoadingStarted -= OnLoadingStarted;
            SceneChanger.Instance.OnLoadingProgress -= OnLoadingProgress;
            SceneChanger.Instance.OnLoadingFinished -= OnLoadingFinished;
        }

        if (modalCount > 0)
        {
            modalCount = 0;

            if (blockerPanel != null)
                blockerPanel.SetActive(false);

            GameManager.Instance.ResumeGame();
        }
    }
    #endregion

    #region HandleGameState
    // ============================================================
    // GameState → UI Group
    // ============================================================
    private void HandleGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                HideAll();
                break;

            case GameState.Sequence:
            case GameState.Hub:
            case GameState.InGame:
            case GameState.Minigame:
                // HUD / 씬 전용 UI 제어 예정
                break;
        }
    }

    private void HideAll()
    {
        // 나중에 Title UI, HUD 등 추가
    }
    #endregion

    #region Currency UI
    public void ShowCurrencyUI()
    {
        currencyPanel.SetActive(true);
    }

    public void HideCurrencyUI()
    {
        currencyPanel.SetActive(false);
    }
    #endregion

    #region Loading UI

    private void OnLoadingStarted()
    {
        loadingPanel?.SetActive(true);
    }

    private void OnLoadingProgress(float progress)
    {
        if (loadingBar != null)
            loadingBar.value = progress;

        if (loadingText != null)
            loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";
    }

    private void OnLoadingFinished()
    {
        loadingPanel?.SetActive(false);
    }
    #endregion

    #region Popup
    private void LoadPopups()
    {
        if (popupOK == null && popupOKPrefab != null)
        {
            popupOK = Instantiate(popupOKPrefab, popupRoot).GetComponent<PopupOK>();
            popupOK.SetBlocker(blockerPanel);
            popupOK.gameObject.SetActive(false);
        }

        if (popupYesNo == null && popupYesNoPrefab != null)
        {
            popupYesNo = Instantiate(popupYesNoPrefab, popupRoot).GetComponent<PopupYesNo>();
            popupYesNo.SetBlocker(blockerPanel);
            popupYesNo.gameObject.SetActive(false);
        }

        if (popupOption == null && popupOptionPrefab != null)
        {
            popupOption = Instantiate(popupOptionPrefab, popupRoot).GetComponent<PopupOption>();
            popupOption.SetBlocker(blockerPanel);
            popupOption.gameObject.SetActive(false);
        }
    }
    private void FocusPopup(Transform popup)
    {
        if (blockerPanel != null)
            blockerPanel.transform.SetAsLastSibling();

        popup.SetAsLastSibling();
    }

    public void ShowPopupOK(string title, string message, Action onOK = null) 
    {
        if (popupOK == null) return;
        EnterModal();
        popupOK.Show(title, message, onOK);
        FocusPopup(popupOK.transform);
    }

    public void HidePopupOK()
    {
        popupOK.Hide();
        ExitModal();
    }

    public void ShowPopupYesNo(string title, string message, Action onYes = null, Action onNo = null) 
    { 
        if (popupYesNo == null) return;
        EnterModal();
        popupYesNo.Show(title, message, onYes, onNo);
        FocusPopup(popupYesNo.transform);
    }

    public void HidePopupYesNo()
    {
        popupYesNo.Hide();
        ExitModal();
    }

    public void ShowPopupOption(Action onClose = null, Action onTitle = null, Action onLobby = null, Action onExit = null, bool title_enable = true, bool lobby_enable = true, bool exit_enable = true)
    {
        if (popupOption == null) return;
        EnterModal();
        popupOption.Show(onClose, onTitle, onLobby, onExit, title_enable, lobby_enable, exit_enable);
        FocusPopup(popupOption.transform);
    }

    public void HidePopupOption()
    {
        popupOption.Hide();
        ExitModal();
    }

    private void EnterModal()
    {
        modalCount++;

        if (modalCount == 1)
        {
            if (blockerPanel != null)
                blockerPanel.SetActive(true);

            GameManager.Instance.PauseGame();
        }
    }

    private void ExitModal()
    {
        if (modalCount <= 0) return;

        modalCount--;

        if (modalCount <= 0)
        {
            modalCount = 0;

            if (blockerPanel != null)
                blockerPanel.SetActive(false);

            GameManager.Instance.ResumeGame();
        }
    }
    #endregion
}
