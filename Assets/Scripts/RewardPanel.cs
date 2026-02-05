using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TMP_Text weaponNameText;

    public void Setup(WeaponData weapon)
    {
        weaponIcon.sprite = weapon.icon;
        weaponNameText.text = weapon.idx.ToString();
    }

    public void OnConfirm()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; // 필요하면 재시작
    }
}
