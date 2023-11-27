using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject Vibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass UnityPlayer;
    public static AndroidJavaObject CurrentActivity;
    public static AndroidJavaObject Vibrator;
#endif

    public static void Vibrate()
    {
        if (IsAndroid())
            Vibrator.Call("vibrate");
        else
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }
            
    }


    public static void Vibrate(long milliseconds)
    {
        if (!GameManager.Instance.VibrateEnable)
            return;

        if (IsAndroid())
            Vibrator.Call("vibrate", milliseconds);
        else
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (IsAndroid())
            Vibrator.Call("vibrate", pattern, repeat);
        else
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }   
    }

    public static bool HasVibrator()
    {
        return IsAndroid();
    }

    public static void Cancel()
    {
        if (IsAndroid())
            Vibrator.Call("cancel");
    }

    private static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}