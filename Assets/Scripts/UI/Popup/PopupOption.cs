using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupOption : PopupBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button lobbyButton;
    [SerializeField] private Button exitButton;

    private Action onClose;
    private Action onTitle;
    private Action onLobby;
    private Action onExit;

    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(() =>
        {
            onClose?.Invoke();
        });

        titleButton.onClick.AddListener(() =>
        {
            onTitle?.Invoke();
        });

        lobbyButton.onClick.AddListener(() =>
        {
            onLobby?.Invoke();
        });

        exitButton.onClick.AddListener(() =>
        {
            onExit?.Invoke();
        });
    }

    public void Show(
    Action onCloseAction = null,
    Action onTitleAction = null,
    Action onLobbyAction = null,
    Action onExitAction = null,
    bool title_enable = true, 
    bool lobby_enable = true, 
    bool exit_enable = true)
    {
        if (!title_enable) titleButton.interactable = false;
        else titleButton.interactable = true;
        if (!lobby_enable) lobbyButton.interactable = false;
        else lobbyButton.interactable = true;
        if (!exit_enable) exitButton.interactable = false;
        else exitButton.interactable = true;
        onClose = onCloseAction;
        onTitle = onTitleAction;
        onLobby = onLobbyAction;
        onExit = onExitAction;
        base.Show();
    }
}
