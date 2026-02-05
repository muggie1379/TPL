using UnityEngine;
public class CloseOpenPanelButton : MonoBehaviour
{
    [SerializeField] private GameObject closePanel;
    [SerializeField] private GameObject openPanel;

    public void OnClick()
    {
        Debug.Log("[Button] CloseOpen Panel");
        if(closePanel != null && closePanel.activeSelf) closePanel.SetActive(false);
        if (openPanel != null && !openPanel.activeSelf) openPanel.SetActive(true);
    }
}