using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    private static extern string PlayerIsInitialized();


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

    public static bool IsInitialized()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        var str = PlayerIsInitialized();
        return System.Convert.ToBoolean(str);
#endif
#if UNITY_EDITOR
        return true;
#endif  
    }
}
