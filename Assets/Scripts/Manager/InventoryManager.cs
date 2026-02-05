using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    #region SaveData
    public void LoadFromSaveData(SaveData data)
    {
        if (data == null) return;

        OnCreditChanged?.Invoke(data.credit);
        OnGemChanged?.Invoke(data.gem);
    }

    private bool IsSaveDataReady()
    {
        return SaveDataManager.Instance != null &&
               SaveDataManager.Instance.Data != null &&
               SaveDataManager.Instance.CurrentSlot > 0;
    }
    #endregion

    #region Currency
    public int Credit
    {
        get
        {
            if (!IsSaveDataReady())
                return 0;

            return SaveDataManager.Instance.Data.credit;
        }
    }

    public int Gem
    {
        get
        {
            if (!IsSaveDataReady())
                return 0;

            return SaveDataManager.Instance.Data.gem;
        }
    }

    public event Action<int> OnCreditChanged;
    public event Action<int> OnGemChanged;

    private const int CreditMin = 0;
    private const int CreditMax = 99_999_999;
    private const int GemMin = 0;
    private const int GemMax = 999_999;

    public void AddCredit(int amount)
    {
        if (!IsSaveDataReady()) return;

        var data = SaveDataManager.Instance.Data;
        data.credit = Mathf.Clamp(data.credit + amount, CreditMin, CreditMax);

        OnCreditChanged?.Invoke(data.credit);
    }

    public void AddGem(int amount)
    {
        if (!IsSaveDataReady()) return;

        var data = SaveDataManager.Instance.Data;
        data.gem = Mathf.Clamp(data.gem + amount, GemMin, GemMax);

        OnGemChanged?.Invoke(data.gem);
    }

    public void SubCredit(int amount) => AddCredit(-amount);

    public void SubGem(int amount) => AddGem(-amount);
    #endregion


    #region Items
    // Data가 없거나 슬롯 선택 전이면 빈 리스트 반환
    public IReadOnlyList<SaveData.OwnedItem> OwnedItems
    {
        get
        {
            if (!IsSaveDataReady())
                return Array.Empty<SaveData.OwnedItem>();

            return SaveDataManager.Instance.Data.owned_Items;
        }
    }

    public SaveData.OwnedItem GetOwnedItem(ItemEnum type)
    {
        if (!IsSaveDataReady()) return null;

        return SaveDataManager.Instance.Data.owned_Items
            .Find(i => i.itemType == type);
    }

    public bool HasItem(ItemEnum type, int count = 1)
    {
        if (!IsSaveDataReady()) return false;
        if (count <= 0) return true;
        var item = GetOwnedItem(type);
        return item != null && item.count >= count;
    }

    public int GetItemCount(ItemEnum type)
    {
        if (!IsSaveDataReady()) return -1;

        var item = GetOwnedItem(type);
        if (item == null) return 0;
        else return item.count;
    }

    public bool AddItem(ItemEnum type, int amount)
    {
        if (!IsSaveDataReady()) return false;

        var data = SaveDataManager.Instance.Data;
        var item = GetOwnedItem(type);

        int max = ItemRules.GetMax(type);

        if (item == null)
        {
            int finalAmount = Mathf.Clamp(amount, 0, max);

            data.owned_Items.Add(new SaveData.OwnedItem
            {
                itemType = type,
                count = finalAmount
            });

            return finalAmount > 0;
        }

        int newCount = Mathf.Clamp(item.count + amount, 0, max);
        item.count = newCount;

        return true;
    }

    public bool ConsumeItem(ItemEnum type, int amount)
    {
        if (!IsSaveDataReady()) return false;
        if(amount < 0) return false;
        if (amount == 0) return true;

        var item = GetOwnedItem(type);
        if (item == null || item.count < amount)
        {
            int count;
            if (item == null) count = 0;
            else count = item.count;
            Debug.LogWarning($"아이템이 부족합니다! {type} {count}개 소유 중");
            return false;
        }

        item.count -= amount;

        if (item.count == 0)
            SaveDataManager.Instance.Data.owned_Items.Remove(item);

        return true;
    }

    #endregion

    #region Weapons
    public IReadOnlyList<SaveData.OwnedWeapon> OwnedWeapons
    {
        get
        {
            if (!IsSaveDataReady())
                return Array.Empty<SaveData.OwnedWeapon>();

            return SaveDataManager.Instance.Data.owned_Weapons;
        }
    }

    public SaveData.OwnedWeapon GetOwnedWeapon(WeaponEnum type)
    {
        if (!IsSaveDataReady()) return null;

        return SaveDataManager.Instance.Data.owned_Weapons
            .Find(w => w.weaponType == type);
    }

    public bool HasWeapon(WeaponEnum type)
    {
        return GetOwnedWeapon(type) != null;
    }

    public bool GetWeapon(WeaponEnum type)
    {
        if (!IsSaveDataReady()) return false;

        if (HasWeapon(type))
            return false;

        SaveDataManager.Instance.Data.owned_Weapons.Add(
            new SaveData.OwnedWeapon { weaponType = type, forgeLevel = 0 }
        );

        return true;
    }

    public bool DestroyWeapon(WeaponEnum type)
    {
        if (!IsSaveDataReady()) return false;

        var weapon = GetOwnedWeapon(type);
        if (weapon == null) return false;

        SaveDataManager.Instance.Data.owned_Weapons.Remove(weapon);
        return true;
    }

    public short GetWeaponForgeLevel(WeaponEnum type)
    {
        if (!IsSaveDataReady()) return 0;

        var weapon = GetOwnedWeapon(type);
        if (weapon == null) return 0;

        return weapon.forgeLevel;
    }

    public bool SetWeaponForgeLevel(WeaponEnum type, short level)
    {
        if (!IsSaveDataReady()) return false;

        var weapon = GetOwnedWeapon(type);
        if (weapon == null) return false;

        weapon.forgeLevel = level;
        return true;
    }
    #endregion
}
