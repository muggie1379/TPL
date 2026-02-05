using UnityEngine;

public static class SaveDataSystem
{
    public static void Save<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static T Load<T>(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            return default;

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<T>(json);
    }

    public static void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
