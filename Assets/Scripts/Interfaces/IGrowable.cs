using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrowable
{
    public void Grow(float growValue);
    public Vector2 GetFilledTopLocalPoint();
    public Vector2 GetFilledTopGlobalPoint();
    public float GetMaxHeight();
    public IGrowable GetRelativeGrowable();
    public Transform GetGrowableTransform();
    public int GetIndex();
}
