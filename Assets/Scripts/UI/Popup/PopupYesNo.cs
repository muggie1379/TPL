using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupYesNo : PopupBase
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action onYes;
    private Action onNo;

    protected override void Awake()
    {
        base.Awake();
        yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            //Hide();
        });

        noButton.onClick.AddListener(() =>
        {
            onNo?.Invoke();
            //Hide();
        });
    }

    public void Show(string title, string message, Action onYesAction = null, Action onNoAction = null)
    {
        onYes = onYesAction;
        onNo = onNoAction;
        titleText.text = title;
        messageText.text = message;
        base.Show();
    }
}
