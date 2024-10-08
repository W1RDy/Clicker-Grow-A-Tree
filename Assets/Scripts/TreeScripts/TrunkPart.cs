﻿using System;
using UnityEngine;

public class TrunkPart : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    public int Index { get; set; }
    private const string AppearValue = "_Fill";
    private const float GrowTime = 0.2f;
    private Material _material;
    private float _currentGrowValue;
    private float _previousFillingValue;
    private CoroutineQueue _coroutineQueue;
    public float Height { get; private set; }
    private Action<float> LerpChangerCallback;
    private Action<IGrowable> EndGrowCallback;

    public void InitializeTrunk(Action<Transform> _callback, CoroutineQueue coroutineQueue, float fillingValue, Action<IGrowable> endGrowCallback)
    {
        _coroutineQueue = coroutineQueue;
        EndGrowCallback = endGrowCallback;

        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Height = transform.GetChild(0).localScale.y * spriteRenderer.sprite.bounds.size.y;
        _material = spriteRenderer.material;
        _callback?.Invoke(transform);

        LerpChangerCallback = value =>
        {
            _currentGrowValue = value;
            _material.SetFloat(AppearValue, _currentGrowValue);
        };

        LerpChangerCallback.Invoke(fillingValue);
        _previousFillingValue = fillingValue;
    }

    public void Grow(float fillingValue)
    {
        if (fillingValue > 1) fillingValue = 1;
        var routine = FloatLerpChanger.LerpFloatChangeCoroutine(_previousFillingValue, fillingValue, GrowTime, LerpChangerCallback, this, EndGrowCallback);
        _previousFillingValue = fillingValue;
        if (fillingValue == 1) _coroutineQueue.StartRequiredCoroutineWithQueue(routine);
        else _coroutineQueue.StartCoroutineWithQueue(routine);
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        var fillingValue = _currentGrowValue;
        var fillingHeight = Height * fillingValue;
        return new Vector2(0, fillingHeight);
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        var localPoint = GetFilledTopLocalPoint();
        return transform.TransformPoint(new Vector2(localPoint.x, localPoint.y));
    }

    public float GetMaxHeight()
    {
        return Height;
    }

    public IGrowable GetRelativeGrowable()
    {
        return this;
    }

    public Transform GetGrowableTransform()
    {
        if (transform == null) return null;
        return transform;
    }

    public int GetIndex()
    {
        return Index;
    }
}
