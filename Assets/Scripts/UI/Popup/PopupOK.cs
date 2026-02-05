using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupOK : PopupBase
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button okButton;

    private Action onOK;

    protected override void Awake()
    {
        base.Awake();
        okButton.onClick.AddListener(() =>
        {
            onOK?.Invoke();
        });
    }

    public void Show(string title, string message, Action onOKAction = null)
    {
        onOK = onOKAction;
        titleText.text = title;
        messageText.text = message;
        base.Show();
    }
}
