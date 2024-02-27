using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Branch : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    private const string AppearValue = "_Fill";
    private int _branchLevel;
    private Material _material;
    private float _currentGrowValue = 0;
    private Transform _relativeObj;
    private IGrowable _growableParent;
    public float Height { get; private set; }
    public event Action<int, Branch> Destory;
    private Action<float> LerpChangerCallback;

    public void InitializeBranch(Transform relativeObj, int branchLevel)
    {
        _branchLevel = branchLevel;
        _relativeObj = relativeObj;
        _growableParent = _relativeObj.GetComponent<IGrowable>();
        _material = GetComponentInChildren<SpriteRenderer>().material;
        Height = transform.GetChild(0).localScale.y;

        LerpChangerCallback = value =>
        {
            _currentGrowValue = value;
            _material.SetFloat(AppearValue, _currentGrowValue);
        };
    }

    public void Grow(float growValue)
    {
        if (_relativeObj != null && _growableParent.GetFilledTopLocalPoint().y >= _relativeObj.InverseTransformPoint(transform.position).y)
        {
            _currentGrowValue += growValue;
            if (_currentGrowValue > 1) _currentGrowValue = 1;
            StartCoroutine(FloatLerpChanger.LerpFloatChangeCoroutine(_material.GetFloat(AppearValue), _currentGrowValue, 0.2f, LerpChangerCallback));
        }
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        var fillingValue = _material.GetFloat(AppearValue);
        var fillingHeight = Height * fillingValue;
        return new Vector2(0, fillingHeight);
    }

    public IGrowable GetRelativeGrowable()
    {
        if (_relativeObj == null) return null;
        return _relativeObj.GetComponent<IGrowable>();
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        var localPoint = GetFilledTopLocalPoint();
        return _relativeObj.TransformPoint(new Vector2(localPoint.x / _relativeObj.localScale.x, localPoint.y / _relativeObj.localScale.y));
    }

    public override void OnDestroy()
    {
        Destory?.Invoke(_branchLevel, this);
        base.OnDestroy();
    }

    public float GetMaxHeight()
    {
        return Height;
    }

    public Transform GetGrowableTransform()
    {
        return transform;
    }
}
