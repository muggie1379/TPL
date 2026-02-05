using UnityEngine;
using UnityEngine.UI;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] private int slotIndex; // 버튼에 연결된 슬롯 번호 (1~3)

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickSaveSlot);
    }

    private void OnClickSaveSlot()
    {
        if (slotIndex < 1 || slotIndex > 3)
        {
            Debug.LogWarning($"잘못된 슬롯 버튼: {slotIndex}");
            return;
        }

        GameManager.Instance.StartGameWithSlot(slotIndex);
    }


}