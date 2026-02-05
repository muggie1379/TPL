using System;
using System.Collections.Generic;
using UnityEngine;
using static ForgeRules;

public class ForgeManager : MonoBehaviour
{
    public static ForgeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public WeaponEnum SelectedWeapon { get; private set; }

    public void SetSelectedWeapon(WeaponEnum weapon)
    {
        SelectedWeapon = weapon;
    }

    public struct ForgeResult
    {
        public bool Tryed;            // 강화 시도 여부
        public bool Success;          // 강화 성공 여부
        public int BeforeLevel;       // 강화 전 레벨
        public int AfterLevel;        // 강화 후 레벨
        public ForgeStoneRequirement UsedForgeStones; // 사용한 강화석
    }

    public ForgeResult TryForge()
    {
        var inventory = InventoryManager.Instance;
        var weapon = inventory.GetOwnedWeapon(SelectedWeapon);

        // 무기 없거나 최대 레벨이면 바로 반환
        if (weapon == null || weapon.forgeLevel >= ForgeRules.MaxForgeLevel)
            return default;

        short beforeLevel = weapon.forgeLevel;

        // ===== 강화석 필요량 계산 =====
        ForgeStoneRequirement requirement = ForgeRules.GetForgeStoneCost(beforeLevel);

        // ===== 강화석 충분한지 확인 =====
        bool hasEnough = inventory.HasItem(requirement.G_ForgeStone_Idx, requirement.G_ForgeStone_Count) &&
                         inventory.HasItem(requirement.N_ForgeStone_Idx, requirement.N_ForgeStone_Count);

        //Debug.LogWarning($"필요한 강화석 : {requirement.G_ForgeStone_Idx}:{requirement.G_ForgeStone_Count}개, {requirement.N_ForgeStone_Idx}:{requirement.N_ForgeStone_Count}개");

        if (!hasEnough)
        {
            Debug.LogWarning("강화석이 부족합니다!");
            return default;
        }

        // ===== 아이템 사용 확인 =====
        // ===== 아이템 충분한지 확인 =====
        // ===== 아이템 소모 =====

        // ===== 강화석 소모 =====
        inventory.ConsumeItem(requirement.G_ForgeStone_Idx, requirement.G_ForgeStone_Count);
        inventory.ConsumeItem(requirement.N_ForgeStone_Idx, requirement.N_ForgeStone_Count);

        // ===== 성공률 계산 =====
        double successRate = ForgeRules.GetSuccessRate(beforeLevel);
        bool success = UnityEngine.Random.value < successRate;

        // ===== 강화 결과 적용 =====
        if (success)
        {
            inventory.SetWeaponForgeLevel(SelectedWeapon, (short)(beforeLevel + 1));
        }
        else // failed
        {
            var (penalty, destroyable) = ForgeRules.GetFaildownPenalty(beforeLevel);

            int afterLevel = beforeLevel - penalty;
            if (afterLevel < 0)
                afterLevel = 0;

            inventory.SetWeaponForgeLevel(SelectedWeapon, (short)afterLevel);
        }

        bool tryed = true;

        return new ForgeResult
        {
            Tryed = tryed,
            Success = success,
            BeforeLevel = beforeLevel,
            AfterLevel = inventory.GetWeaponForgeLevel(SelectedWeapon),
            UsedForgeStones = requirement
        };
    }
}
