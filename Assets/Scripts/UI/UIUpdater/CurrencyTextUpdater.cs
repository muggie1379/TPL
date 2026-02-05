using UnityEngine;
using TMPro;

public class CurrencyTextUpdater : MonoBehaviour
{
    public enum CurrencyType { Credit, Gem }
    [SerializeField] private CurrencyType currencyType;

    private TMP_Text textField;
    private bool isSubscribed = false;

    private void Awake()
    {
        textField = GetComponent<TMP_Text>();
        if (textField == null)
        {
            Debug.LogError($"{nameof(CurrencyTextUpdater)} requires a TMP_Text component.");
            enabled = false;
        }
    }

    private void Start()
    {
        TrySubscribe();
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void TrySubscribe()
    {
        if (!enabled || isSubscribed)
            return;

        if (GameManager.Instance == null)
            return;

        isSubscribed = true;

        switch (currencyType)
        {
            case CurrencyType.Credit:
                GameManager.Instance.Inventory.OnCreditChanged += UpdateText;
                UpdateText(GameManager.Instance.Inventory.Credit);
                break;

            case CurrencyType.Gem:
                GameManager.Instance.Inventory.OnGemChanged += UpdateText;
                UpdateText(GameManager.Instance.Inventory.Gem);
                break;
        }
    }

    private void Unsubscribe()
    {
        if (!isSubscribed || GameManager.Instance == null)
            return;

        isSubscribed = false;

        switch (currencyType)
        {
            case CurrencyType.Credit:
                GameManager.Instance.Inventory.OnCreditChanged -= UpdateText;
                break;

            case CurrencyType.Gem:
                GameManager.Instance.Inventory.OnGemChanged -= UpdateText;
                break;
        }
    }

    private void UpdateText(int value)
    {
        textField.text = value.ToString();
    }
}
