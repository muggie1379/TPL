using UnityEngine;
public class OpenPanel : MonoBehaviour
{
    [SerializeField] private GameObject openPanel;

    public void OnClick()
    {
        Debug.Log("[Button] Open Panel");
        if (openPanel != null && !openPanel.activeSelf) openPanel.SetActive(true);
    }
}