using UnityEngine;
public class ClosePanel : MonoBehaviour 
{ 
    [SerializeField] private GameObject closePanel; 
    
    public void OnClick() 
    {
        Debug.Log("[Button] Close Panel");
        if (closePanel != null && closePanel.activeSelf) closePanel.SetActive(false);
    } 
}