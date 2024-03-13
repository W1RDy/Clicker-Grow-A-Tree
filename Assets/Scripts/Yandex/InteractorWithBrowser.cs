using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class InteractorWithBrowser
{
    //[DllImport("__Internal")]
    //private static extern void AuthorizeExtern();
    [DllImport("__Internal")]
    private static extern void ShowAdv();
    [DllImport("__Internal")]
    private static extern void AddRewardForADV(float value);
    [DllImport("__Internal")]
    private static extern void SaveScoreInLeaderboardExtern(int score);

    [DllImport("__Internal")]
    private static extern string GetLanguageExtern();

    [DllImport("__Internal")]
    private static extern void SaveDataExtern(string data);
    [DllImport("__Internal")]
    private static extern void LoadDataExtern();
    [DllImport("__Internal")]
    private static extern string PlayerIsInitializedExtern();


    //    public static void Authorize()
    //    {
    //#if !UNITY_EDITOR && UNITY_WEBGL
    //        AuthorizeExtern();
    //#endif
    //    }


    public static void ShowAdversity()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        ShowAdv();
#endif
#if UNITY_EDITOR
        Debug.Log("ADV");
#endif
    }

    public static void GetRewardForAdv(float value)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AddRewardForADV(value);
#endif
#if UNITY_EDITOR
        Debug.Log("ADV");
#endif
    }

    public static void SaveScoreInLeaderboard(int score)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        SaveScoreInLeaderboardExtern(score);
#endif
    }

    public static string GetLanguage()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        return GetLanguageExtern();
#endif
#if UNITY_EDITOR
        return "ru";
#endif  
    }

    public static void SaveData(string data)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        SaveDataExtern(data);
#endif
#if UNITY_EDITOR
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Data.json"), data, Encoding.UTF8);
#endif
    }

    public static void LoadData()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        LoadDataExtern();
#endif
#if UNITY_EDITOR
        var json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Data.json"));
        ServiceLocator.Instance.Get<SaveService>().SetData(json);
#endif
    }

    public static bool PlayerIsInitialized()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        string str = PlayerIsInitializedExtern();
        return System.Convert.ToBoolean(str);
#endif
#if UNITY_EDITOR
        return true;
#endif
    }
}
