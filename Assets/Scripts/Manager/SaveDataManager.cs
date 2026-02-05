using UnityEngine;
using System;
using System.Collections;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private const string SLOT_KEY = "current_slot";
    private const string SAVE_KEY_PREFIX = "save_slot_";

    private static int SlotMin = 1;
    private static int SlotMax = 3;


    public int CurrentSlot { get; private set; } = 0; // 선택 전
    public SaveData Data { get; private set; }

    public event Action<SaveData> OnSaveDataLoaded;

    [Header("자동 저장 설정")]
    public float autosaveDelay = 5f;
    public bool autoSaveEnabled = true;

    protected override void Awake()
    {
        base.Awake();
        CurrentSlot = 0; // 게임 시작 시 슬롯 선택되지 않음
        Data = null;
    }

    private void Start()
    {
        StartCoroutine(AutoSaveRoutine());
    }

    // ============================================================
    // 슬롯 선택
    // ============================================================
    public void SelectSlot(int slotIndex)
    {
        if (slotIndex < SlotMin || slotIndex > SlotMax)
        {
            Debug.LogError("잘못된 슬롯 번호입니다. (1~3)");
            return;
        }

        CurrentSlot = slotIndex;

        if (!SlotExists(slotIndex))
        {
            Data = new SaveData();
            Data.Init();
            Save();
        }
        else
        {
            Load();
        }

        Debug.Log($"슬롯 선택됨 : Slot {CurrentSlot}");
    }

    // 선택된 슬롯 초기화 (타이틀로 돌아가거나 종료 시)
    public void ResetCurrentSlot()
    {
        if (CurrentSlot <= 0 || Data == null) return;
        if (CurrentSlot >= SlotMin && CurrentSlot <= SlotMax)
        {
            Save();
        }
        CurrentSlot = 0;
        Data = null;
        PlayerPrefs.DeleteKey(SLOT_KEY);
    }

    private string GetSlotKey() => SAVE_KEY_PREFIX + CurrentSlot;

    // ============================================================
    // Load / Save
    // ============================================================
    public void Load()
    {
        if (CurrentSlot <= 0)
        {
            Debug.LogWarning("Load 실패: 선택된 슬롯 없음");
            return;
        }

        Data = SaveDataSystem.Load<SaveData>(GetSlotKey());

        if (Data == null)
        {
            Data = new SaveData();
            Data.Init();
            Save();
        }

        OnSaveDataLoaded?.Invoke(Data);
    }

    public void Save()
    {
        if (CurrentSlot <= 0)
        {
            Debug.LogWarning("저장 시도했지만 CurrentSlot이 0입니다. 저장하지 않음.");
            return;
        }

        if (Data == null)
        {
            Debug.LogWarning("저장 시도했지만 Data가 null입니다. 저장하지 않음.");
            return;
        }

        Data.lastSaveTime = GetCurrentTimeString();
        SaveDataSystem.Save(GetSlotKey(), Data);
        Debug.LogWarning($"Slot {CurrentSlot} 저장 완료!");
    }

    public bool SlotExists(int slotIndex)
        => PlayerPrefs.HasKey(SAVE_KEY_PREFIX + slotIndex);

    // ============================================================
    // 자동 저장
    // ============================================================
    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(autosaveDelay);

            if (autoSaveEnabled && CurrentSlot != 0)
                Save();
        }
    }

    private void OnApplicationQuit()
    {
        if (CurrentSlot != 0)
            Save();
    }

    private string GetCurrentTimeString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
