using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WeaponEnum idx;          // ¹«±â Index
    public Sprite icon;             // UI¿¡ ¶ç¿ï ¾ÆÀÌÄÜ
    public Sprite icon_veiled;      // ¾ÆÀÌÄÜ_È¹µæ Àü
    public WeaponRarity rarity;       // Èñ±Íµµ µî±Ş
}
