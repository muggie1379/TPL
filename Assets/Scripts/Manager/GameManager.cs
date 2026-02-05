using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : Singleton<GameManager>
{
    #region Global
    #region Managers (Facade)
    public SaveDataManager UserData => SaveDataManager.Instance;
    public InventoryManager Inventory => InventoryManager.Instance;
    public AudioManager Audio => AudioManager.Instance;
    public SceneChanger Scene => SceneChanger.Instance;
    public GlobalUIManager UI => GlobalUIManager.Instance;
    #endregion

    #region GameState
    public GameState CurrentState { get; private set; } = GameState.None;
    public GameState PrevState { get; private set; } = GameState.None;
    public event Action<GameState> OnGameStateChanged;

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;
        PrevState = CurrentState;
        CurrentState = newState;
        OnGameStateChanged?.Invoke(CurrentState);
        Debug.Log($"[GameState] → {CurrentState}");
    }
    #endregion

    #endregion

    #region Lifecycle
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnApplicationQuit()
    {
        UserData.ResetCurrentSlot();
    }
    #endregion

    #region GameFlow
    public void OnSceneEntered(SceneEnum scene)
    {
        switch (scene)
        {
            case SceneEnum.TitleScene:
                UI.HideCurrencyUI();
                UserData.ResetCurrentSlot();
                SetState(GameState.Title);
                break;

            case SceneEnum.InfoSelectScene:
                UI.HideCurrencyUI();
                SetState(GameState.Sequence);
                break;

            case SceneEnum.StoryScene:
                UI.HideCurrencyUI();
                SetState(GameState.Sequence);
                break;

            case SceneEnum.LobbyScene:
                UI.ShowCurrencyUI();
                SetState(GameState.Hub);
                break;

            case SceneEnum.ArmoryScene:
                UI.ShowCurrencyUI();
                SetState(GameState.Hub);
                break;

            case SceneEnum.ForgeScene:
                UI.ShowCurrencyUI();
                SetState(GameState.Hub);
                break;

            case SceneEnum.ItemShopScene:
                UI.ShowCurrencyUI();
                SetState(GameState.Hub);
                break;

            case SceneEnum.DungeonLevelScene:
                UI.HideCurrencyUI();
                SetState(GameState.InGame);
                break;

            case SceneEnum.DungeonBattleScene:
                UI.HideCurrencyUI();
                SetState(GameState.InGame);
                break;

            // Arena Scenes

            case SceneEnum.MinigameMainScene:
                UI.HideCurrencyUI();
                SetState(GameState.Minigame);
                break;

            case SceneEnum.TutorialDungeonScene:
                UI.HideCurrencyUI();
                SetState(GameState.Sequence);
                break;

            case SceneEnum.TutorialArenaScene:
                UI.HideCurrencyUI();
                SetState(GameState.Sequence);
                break;

            default:
                break;
        }
    }

    public void PauseGame()
    {
        if ((int)CurrentState >= 0) PrevState = CurrentState;
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if((int)PrevState >= 0) SetState(PrevState);
        PrevState = GameState.None;
    }

    public void QuitGame()
    {
        Debug.Log("[Button] Exit");
        SaveCurrentSlotData(); // 저장 실행

    #if UNITY_EDITOR
        // 에디터에서는 Play 모드만 종료
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        // 실제 빌드에서는 게임 종료
        Application.Quit();
    #endif
    }

    public void StartGameWithSlot(int slotIndex)
    {
        SetState(GameState.Loading);

        UserData.SelectSlot(slotIndex);  // 슬롯 선택 + Load
        InitializeFromUserData();        // 로드한 데이터 반영

        if (UserData.Data == null)
        {
            Debug.LogError("[GameManager] SaveData 로드 실패");
            SetState(GameState.Title);
            return;
        }

        Debug.Log($"[StartGame] Slot {slotIndex} | Credit {Inventory.Credit} | Gem {Inventory.Gem}");

        Scene.LoadScene(SceneEnum.LobbyScene);
    }
    #endregion

    #region UserData

    #region Save Data
    public void InitializeFromUserData()
    {
        var data = UserData.Data;
        if (data == null)
        {
            Debug.LogWarning("[GameManager] UserData가 아직 준비되지 않음");
            return;
        }
        Inventory.LoadFromSaveData(data);
    }

    public void SaveCurrentSlotData()
    {
        if (UserData.CurrentSlot <= 0)
        {
            Debug.LogError("[GameManager] 슬롯이 선택되지 않아 저장할 수 없습니다.");
            return;
        }

        UserData.Save();
        Debug.Log($"[GameManager] Slot {UserData.CurrentSlot} 저장 완료!");
    }

    public void LoadCurrentSlotData()
    {
        if (UserData.CurrentSlot <= 0)
        {
            Debug.LogError("[GameManager] 슬롯이 선택되지 않아 로드할 수 없습니다.");
            return;
        }

        UserData.Load();
        InitializeFromUserData(); // 로드한 데이터 반영
        Debug.Log($"[GameManager] Slot {UserData.CurrentSlot} 로드 완료!");
    }
    #endregion

    #endregion

    #region UI Popups
    public void ShowPopupOK(string title, string message)
    {
        Action onOK = UI.HidePopupOK;
        UI?.ShowPopupOK(title, message, onOK);
    }

    //// "How to Use"
    //GameManager.Instance.ShowPopupYESNO(
    //    title: "", 
    //    message: "", 
    //    (result) =>
    //    {
    //        if (result)
    //        {
    //            // Yes Click Action : If Delete, Just Close Popup
    //        }
    //        else
    //        {
    //            // No Click Action : If Delete, Just Close Popup
    //        }
    //    }
    //);
    public void ShowPopupYESNO(string title, string message, Action<bool> onResult)
    {
        Action onYes = () => {
            UI.HidePopupYesNo();
            onResult?.Invoke(true);
        };

        Action onNo = () => {
            UI.HidePopupYesNo();
            onResult?.Invoke(false);
        };

        UI?.ShowPopupYesNo(title, message, onYes, onNo);

    }

    public void ShowPopupOPTION(bool title_enable, bool lobby_enable, bool exit_enable, bool title_yesno, bool lobby_yesno, bool exit_yesno, string title_Message = null, string lobby_Message = null, string exit_Message = null)
    {
        if (string.IsNullOrWhiteSpace(title_Message)) title_Message = "Your current progress may be lost.";
        if (string.IsNullOrWhiteSpace(lobby_Message)) lobby_Message = "Your current progress may be lost.";
        if (string.IsNullOrWhiteSpace(exit_Message)) exit_Message = "Your current progress may be lost.";

        Action onClose = UI.HidePopupOption;
        Action onTitle = () => Option_LoadTitle(title_enable, title_yesno, title: "Warning!", title_Message);
        Action onLobby = () => Option_LoadLobby(lobby_enable, lobby_yesno, title: "Warning!", lobby_Message);
        Action onExit = () => Option_Exit(exit_enable, exit_yesno, title: "Warning!", exit_Message);

        UI?.ShowPopupOption(onClose, onTitle, onLobby, onExit, title_enable, lobby_enable, exit_enable);
    }

    public void Option_LoadTitle(bool enable, bool popup, string title, string message)
    {
        if (!enable)
        {
            return;
        }
        if (popup)
        {
            message = message + "\n" + "Really Go to Title?";
            ShowPopupYESNO(title, message, (result) =>
            {
                if (result)
                {
                    UI.HidePopupOption();
                    SceneChanger.Instance.LoadScene(SceneEnum.TitleScene);
                }
            });
        }
        else
        {
            UI.HidePopupOption();
            SceneChanger.Instance.LoadScene(SceneEnum.TitleScene);
        }
    }

    public void Option_LoadLobby(bool enable, bool popup, string title, string message)
    {
        if (!enable)
        {
            return;
        }
        if (popup)
        {
            message = message + "\n" + "Really Go to Lobby?";
            ShowPopupYESNO(title, message, (result) =>
            {
                if (result)
                {
                    UI.HidePopupOption();
                    SceneChanger.Instance.LoadScene(SceneEnum.LobbyScene);
                }
            });
        }
        else
        {
            UI.HidePopupOption();
            SceneChanger.Instance.LoadScene(SceneEnum.LobbyScene);
        }
    }

    public void Option_Exit(bool enable, bool popup, string title, string message)
    {
        if (!enable)
        {
            return;
        }
        if (popup)
        {
            message = message + "\n" + "Really Exit?";
            ShowPopupYESNO(title, message, (result) =>
            {
                if (result)
                {
                    UI.HidePopupOption();
                    QuitGame();
                }
            });
        }
        else
        {
            UI.HidePopupOption();
            QuitGame();
        }
    }
    #endregion
}
