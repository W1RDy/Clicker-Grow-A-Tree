using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    private const string AppearValue = "_Fill";
    private Material _material;
    private float _currentGrowValue = 0;
    private Transform _relativeObj;
    private IGrowable _growableParent;
    public float Height { get; private set; }

    public void InitializeBranch(Transform relativeObj)
    {
        _relativeObj = relativeObj;
        _growableParent = _relativeObj.GetComponent<IGrowable>();
        _material = GetComponentInChildren<SpriteRenderer>().material;
        Height = transform.GetChild(0).localScale.y;
    }

    public void Grow(float growValue)
    {
        if (_relativeObj != null && _growableParent.GetFilledTopLocalPoint().y >= _relativeObj.InverseTransformPoint(transform.position).y)
        {
            _currentGrowValue += growValue;
            if (_currentGrowValue > 1) _currentGrowValue = 1;
            _material.SetFloat(AppearValue, _currentGrowValue);
        }
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        var fillingValue = _material.GetFloat(AppearValue);
        var fillingHeight = Height * fillingValue;
        return new Vector2(0, fillingHeight);
    }
}
