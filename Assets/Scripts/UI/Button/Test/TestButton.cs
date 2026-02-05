using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void OnClickShowOK()
    {
        Debug.Log("Clicked");

        GameManager.Instance.ShowPopupOK("OK Popup", "Message : OK POPUP");
    }

    public void OnClickShowYesNo()
    {
        Debug.Log("Clicked");

        GameManager.Instance.ShowPopupYESNO("YesNo Popup", "Message : YesNo POPUP", (result) =>
        {
            if (result) Debug.Log("Yes Clicked!");
            else Debug.Log("No Clicked!");
        });
    }

    public void OnClickTestScene()
    {
        Debug.Log("Clicked");

        GameManager.Instance.Scene.LoadScene(SceneEnum.TestScene);
    }

    public void OnClickDungeonBattleScene()
    {
        Debug.Log("Clicked");

        GameManager.Instance.Scene.LoadScene(SceneEnum.DungeonBattleScene);
    }

    public void OnClickForgeScene()
    {
        Debug.Log("Clicked");

        GameManager.Instance.Scene.LoadScene(SceneEnum.ForgeScene);
    }

    public void OnClickAdd100Gem()
    {
        Debug.Log("Clicked");

        GameManager.Instance.Inventory.AddGem(100);

        Debug.Log($"Slot : {GameManager.Instance.UserData.CurrentSlot}");
        Debug.Log($"Gem : {GameManager.Instance.UserData.Data.gem}");
    }

    public void OnClickAdd100Credit()
    {
        Debug.Log("Clicked");

        GameManager.Instance.Inventory.AddCredit(100);

        Debug.Log($"Slot : {GameManager.Instance.UserData.CurrentSlot}");
        Debug.Log($"Credit : {GameManager.Instance.UserData.Data.credit}");
    }

    [Header("무기 강화 버튼")]
    [SerializeField] private WeaponEnum forgeWeaponType;
    public void OnClickForgeTest()
    {
        Debug.Log("Clicked");
        GameManager.Instance.ShowPopupYESNO(
            title: "Forge",
            message: "Want to Forge?", // TODO : Message
            (result) =>
            {
                if (result)
                {
                    ForgeManager.Instance.SetSelectedWeapon(forgeWeaponType);
                    var r = ForgeManager.Instance.TryForge();
                    if(r.Tryed) Debug.LogWarning($"강화 결과: {r.BeforeLevel} → {r.AfterLevel}, 성공: {r.Success}");
                }
                else
                {
                    // No Click Action
                }
            }
        );
    }

    [Header("무기 지급 버튼")]
    [SerializeField] private WeaponEnum giveWeaponType;
    public void OnClickDebugGiveWeapon()
    {
        var data = SaveDataManager.Instance.Data;

        if (data.owned_Weapons.Exists(w => w.weaponType == giveWeaponType))
        {
            Debug.Log($"무기 보유 중 - {giveWeaponType}:{InventoryManager.Instance.HasWeapon(giveWeaponType)}, ForgeLevel:+{InventoryManager.Instance.GetWeaponForgeLevel(giveWeaponType)}");
            return;
        }

        data.owned_Weapons.Add(new SaveData.OwnedWeapon
        {
            weaponType = giveWeaponType,
            forgeLevel = 0
        });

        Debug.Log($"무기 지급 완료 - {giveWeaponType}:{InventoryManager.Instance.HasWeapon(giveWeaponType)}, ForgeLevel:+{InventoryManager.Instance.GetWeaponForgeLevel(giveWeaponType)}");
    }

    [Header("아이템 지급 버튼")]
    [SerializeField] private  ItemEnum giveItemType;
    [SerializeField] private int giveItemCount;
    public void OnClickDebugGiveItem()
    {
        var data = SaveDataManager.Instance.Data;

        InventoryManager.Instance.AddItem(giveItemType, giveItemCount);

        bool hasEnough = 
                            InventoryManager.Instance.HasItem(ItemEnum.ForgeStone_Greater, 0) &&
                         InventoryManager.Instance.HasItem(ItemEnum.ForgeStone_Normal, 10);

        Debug.Log($"아이템 지급 완료 - {giveItemType}:{InventoryManager.Instance.GetItemCount(giveItemType)}개 {hasEnough}");
    }

}
