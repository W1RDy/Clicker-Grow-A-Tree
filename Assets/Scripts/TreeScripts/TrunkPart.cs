using System;
using UnityEngine;

public class TrunkPart : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    private const string AppearValue = "_Fill";
    private Material _material;
    private float _currentGrowValue;
    public float Height { get; private set; }
    private Action<float> LerpChangerCallback;

    public void InitializeTrunk(Action<Transform> _callback)
    {
        Height = transform.localScale.y;
        _material = GetComponent<SpriteRenderer>().material;
        _callback?.Invoke(transform);

        LerpChangerCallback = value =>
        {
            _currentGrowValue = value;
            _material.SetFloat(AppearValue, _currentGrowValue);
        };
    }

    public void Grow(float fillingValue)
    {
        if (fillingValue > 1) fillingValue = 1;
        _currentGrowValue = fillingValue;
        StartCoroutine(FloatLerpChanger.LerpFloatChangeCoroutine(_material.GetFloat(AppearValue), _currentGrowValue, 0.2f, LerpChangerCallback));
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        var fillingValue = _material.GetFloat(AppearValue);
        var fillingHeight = Height * fillingValue;
        return new Vector2(0, (-(Height / 2) + fillingHeight)) / Height;
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        var localPoint = GetFilledTopLocalPoint();
        return transform.TransformPoint(new Vector2(localPoint.x / transform.localScale.x, localPoint.y / Height));
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
        return transform;
    }
}
