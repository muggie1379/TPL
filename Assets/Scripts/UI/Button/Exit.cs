using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnClickExit()
    {
        GameManager.Instance.QuitGame();
    }
}