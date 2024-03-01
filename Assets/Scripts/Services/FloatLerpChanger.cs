using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatLerpChanger
{
    public static IEnumerator LerpFloatChangeCoroutine(float startValue, float endValue, float duration, Action<float> callback)
    {
        float progress = 0f;

        while (progress < duration)
        {
            progress += Time.deltaTime;

            float progressInPercantage = progress / duration;
            float value = Mathf.Lerp(startValue, endValue, progressInPercantage);
            callback?.Invoke(value);

            yield return null;
        }
        callback?.Invoke(endValue);
    }

    public static IEnumerator LerpFloatChangeCoroutine(float startValue, float endValue, float duration, Action<float> callback, IGrowable growable, Action<IGrowable> endCallback)
    {
        float progress = 0f;

        while (progress < duration)
        {
            progress += Time.deltaTime;

            float progressInPercantage = progress / duration;
            float value = Mathf.Lerp(startValue, endValue, progressInPercantage);
            callback?.Invoke(value);

            yield return null;
        }
        callback?.Invoke(endValue);
        endCallback?.Invoke(growable);
    }
}
