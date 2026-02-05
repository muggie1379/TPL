using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    protected GameObject blockerPanel;

    protected virtual void Awake()
    {
    }

    // GlobalUIManager에서 BlockerPanel을 전달
    public void SetBlocker(GameObject panel)
    {
        blockerPanel = panel;
        if (blockerPanel != null)
            blockerPanel.SetActive(false); // 초기에는 숨김
    }

    public virtual void Show()
    {
        if (blockerPanel != null)
            blockerPanel.SetActive(true); // 모달 활성화

        gameObject.SetActive(true); // 팝업 활성화
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false); // 팝업 비활성화

        if (blockerPanel != null)
            blockerPanel.SetActive(false); // 모달 비활성화
    }
}
