using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // 플레이어 정보
    public CharacterNameEnum characterName;
    public CharacterClassEnum characterClass;
    public DifficultyEnum difficulty;

    // 화폐
    public int credit;
    public int gem;

    // 소유 아이템
    [Serializable]
    public class OwnedItem
    {
        public ItemEnum itemType;
        public int count; // 보유 개수
    }
    // 보유 중인 아이템 리스트
    public List<OwnedItem> owned_Items = new List<OwnedItem>();

    [Serializable]
    public class OwnedWeapon
    {
        public WeaponEnum weaponType;
        public short forgeLevel; // 강화 단계
    }
    // 보유 중인 무기 리스트
    public List<OwnedWeapon> owned_Weapons = new List<OwnedWeapon>();

    [Serializable]
    public class OwnedCostume
    {
        public CostumeEnum costumeType;
    }
    // 보유 중인 코스튬 리스트
    public List<OwnedCostume> owned_Costumes = new List<OwnedCostume>();

    // 선택된 아이템
    public WeaponEnum Selected_WeaponDungeon;   // 던전 선택 무기
    public WeaponEnum Selected_WeaponArena1;    // 아레나 선택 무기1
    public WeaponEnum Selected_WeaponArena2;    // 아레나 선택 무기2
    public WeaponEnum Selected_WeaponArena3;    // 아레나 선택 무기3
    public CostumeEnum Selected_Costume;        // 선택 코스튬

    // 던전 정보
    [Serializable]
    public class DungeonPlayInfo
    {
        public static readonly int DungeonCount = Enum.GetValues(typeof(DungeonEnum)).Length;

        public short dungeon_Progress;  // 던전 진행도
        public int[] dungeon_PlayCount; // 각 던전 플레이횟수 배열

        //초기화
        public DungeonPlayInfo()
        {
            dungeon_Progress = 0;
            dungeon_PlayCount = new int[DungeonCount];
        }
    }
    // 던전 플레이정보
    public DungeonPlayInfo playInfo_Dungeon; 


    // 아레나 정보
    [Serializable]
    public class ArenaPlayInfo
    {
        public int arenaWinCount;       // 승리횟수
        public int arenaLoseCount;      // 패배횟수
        public int arenaMMR;            // MMR
        public ArenaTierEnum arenaTier; // 티어

        // 초기화
        public ArenaPlayInfo()
        {
            arenaWinCount = 0;
            arenaLoseCount = 0;
            arenaMMR = 5000;
            arenaTier = ArenaTierEnum.None;
        }
    }
    // 아레나 플레이정보
    public ArenaPlayInfo playInfo_Arena; 

    // 업적 정보
    public List<AchievementEnum> unlockedAchievements = new List<AchievementEnum>();

    // 진행도
    public short progress_Story; // 전체 스토리 진행도

    // 설정 정보
    public LanguageEnum option_Language = LanguageEnum.Korean;  // 설정 언어
    public bool option_FullScreen = true;                       // 전체화면 옵션
    public float option_Volume_BGM = 1f;                        // BGM 볼륨 0~1
    public float option_Volume_SFX = 1f;                        // SFX 볼륨 0~1
    //public bool option_AutoPlay_Dungeon = false;              // 던전 자동플레이 옵션 -> 저장X
    //public bool option_AutoPlay_Arena = false;                // 아레나 자동플레이 옵션 -> 저장X

    // 자동 저장 시간
    public string lastSaveTime = "";

    public SaveData()
    {
        // 기본값 생성
        credit = 0;
        gem = 0;
        lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void Init()
    {
        credit = 0;
        gem = 0;

        characterName = CharacterNameEnum.None;
        characterClass = CharacterClassEnum.None;
        difficulty = DifficultyEnum.None;

        owned_Items.Clear();
        owned_Weapons.Clear();
        owned_Costumes.Clear();

        InitSelectedItems();
        InitDungeon();
        InitArena();

        unlockedAchievements.Clear();

        progress_Story = 0;

        InitOptions();
        UpdateLastSaveTime();
    }

    private void InitSelectedItems()
    {
        Selected_WeaponDungeon = WeaponEnum.None;
        Selected_WeaponArena1 = WeaponEnum.None;
        Selected_WeaponArena2 = WeaponEnum.None;
        Selected_WeaponArena3 = WeaponEnum.None;
        Selected_Costume = CostumeEnum.None;
    }

    private void InitDungeon()
    {
        if (playInfo_Dungeon == null)
            playInfo_Dungeon = new DungeonPlayInfo();
        else
            Array.Clear(playInfo_Dungeon.dungeon_PlayCount, 0, playInfo_Dungeon.dungeon_PlayCount.Length);
        playInfo_Dungeon.dungeon_Progress = 0;
    }

    private void InitArena()
    {
        if (playInfo_Arena == null)
            playInfo_Arena = new ArenaPlayInfo();
        else
        {
            playInfo_Arena.arenaWinCount = 0;
            playInfo_Arena.arenaLoseCount = 0;
            playInfo_Arena.arenaMMR = 5000;
            playInfo_Arena.arenaTier = ArenaTierEnum.None;
        }
    }

    private void InitOptions()
    {
        option_Language = LanguageEnum.Korean;
        option_FullScreen = true;
        option_Volume_BGM = 1f;
        option_Volume_SFX = 1f;
    }

    public void UpdateLastSaveTime()
    {
        lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}