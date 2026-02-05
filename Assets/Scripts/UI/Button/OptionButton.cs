using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private bool Title_enable = true;
    [SerializeField] private bool Title_yesno = false;
    [SerializeField] private string Title_WarningMessage = "";
    [SerializeField] private bool Lobby_enable = true;
    [SerializeField] private bool Lobby_yesno = false;
    [SerializeField] private string Lobby_WarningMessage = "";
    [SerializeField] private bool Exit_enable = true;
    [SerializeField] private bool Exit_yesno = false;
    [SerializeField] private string Exit_WarningMessage = "";

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickOption);
    }

    private void OnClickOption()
    {
        Debug.Log("Option Clicked");
        GameManager.Instance.ShowPopupOPTION(Title_enable, Lobby_enable, Exit_enable, Title_yesno, Lobby_yesno, Exit_yesno, Title_WarningMessage, Lobby_WarningMessage, Exit_WarningMessage);
    }
}
