using System;
using UnityEngine;

public static class ForgeRules
{
    // ===== 기본 설정 =====
    public const short MaxForgeLevel = 99;

    // ===== 성공률 계산 =====

    // 성공률 계수 - 난이도에 따라 변경
    private const double ForgeCoefA = 50.0;
    private const double ForgeRateMin = 0.03;
    private const double ForgeRateMax = 1.00;

    private static double GetBaseAt98()
    {
        return ForgeCoefA / (ForgeCoefA + 98.0) * 100.0;
    }

    public static double GetSuccessRate(short forgeLevel)
    {
        double baseValue = ForgeCoefA / (ForgeCoefA + forgeLevel) * 100.0;

        double rate =
            ForgeRateMax -
            (100.0 - baseValue) * (ForgeRateMax - ForgeRateMin) /
            (100.0 - GetBaseAt98());

        return Math.Clamp(rate, ForgeRateMin, ForgeRateMax);
    }

    // ===== 강화 실패 페널티 =====
    public const short Faildown_Level_A = 10; // 미만 구간
    public const short Faildown_Level_B = 30; // 미만 구간
    public const short Faildown_Level_C = 50; // 미만 구간
    public const short Faildown_Level_D = 80; // 미만 구간

    public const int Faildown_Penalty_A = 0;
    public const int Faildown_Penalty_B = 1;
    public const int Faildown_Penalty_C = 2;
    public const int Faildown_Penalty_D = 3;
    public const int Faildown_Penalty_Max = 3;

    public static (int penalty, bool destroyable) GetFaildownPenalty(short forgeLevel)
    {

        if      (forgeLevel < Faildown_Level_A)     return (Faildown_Penalty_A, false);
        else if (forgeLevel < Faildown_Level_B)     return (Faildown_Penalty_B, false);
        else if (forgeLevel < Faildown_Level_C)     return (Faildown_Penalty_C, false);
        else if (forgeLevel < Faildown_Level_D)     return (Faildown_Penalty_D, false);
        else                                        return (Faildown_Penalty_Max, true);
    }

    // ===== 스크롤 사용 =====
    public const short Scroll_Level_A = 10; // 미만 구간
    public const short Scroll_Level_B = 30; // 미만 구간
    public const short Scroll_Level_C = 80; // 미만 구간

    public const ItemType_Scroll Scroll_Type_A = ItemType_Scroll.None;
    public const ItemType_Scroll Scroll_Type_B = ItemType_Scroll.Guard;
    public const ItemType_Scroll Scroll_Type_C = ItemType_Scroll.Aegis;
    public const ItemType_Scroll Scroll_Type_D = ItemType_Scroll.Immortal;

    public static ItemType_Scroll GetUsableScroll(short forgeLevel)
    {

        if      (forgeLevel < Scroll_Level_A)   return Scroll_Type_A;
        else if (forgeLevel < Scroll_Level_B)   return Scroll_Type_B;
        else if (forgeLevel < Scroll_Level_C)   return Scroll_Type_C;
        else                                    return Scroll_Type_D;
    }

    // ===== 비용 계산 =====
    public struct ForgeStoneRequirement
    {
        public ItemEnum G_ForgeStone_Idx;    // 고급 강화석 ItemEnum
        public int G_ForgeStone_Count;       // 고급 강화석 개수

        public ItemEnum N_ForgeStone_Idx;    // 일반 강화석 ItemEnum
        public int N_ForgeStone_Count;       // 일반 강화석 개수
    }

    public static ForgeStoneRequirement GetForgeStoneCost(short forgeLevel)
    {
        if (forgeLevel < 0 || forgeLevel >= MaxForgeLevel)
            return new ForgeStoneRequirement
            {
                G_ForgeStone_Idx = ItemEnum.ForgeStone_Greater,
                G_ForgeStone_Count = 0,
                N_ForgeStone_Idx = ItemEnum.ForgeStone_Normal,
                N_ForgeStone_Count = 0
            };

        int totalStones = forgeLevel + 1;

        int gCount = totalStones / 10;  // 십의 자리 → 고급 강화석
        int nCount = totalStones % 10;  // 일의 자리 → 일반 강화석

        Debug.Log($"필요한 강화석 : 고급 {gCount}개, 일반 {nCount}개");

        return new ForgeStoneRequirement
        {
            G_ForgeStone_Idx = ItemEnum.ForgeStone_Greater,
            G_ForgeStone_Count = gCount,
            N_ForgeStone_Idx = ItemEnum.ForgeStone_Normal,
            N_ForgeStone_Count = nCount
        };
    }

}
