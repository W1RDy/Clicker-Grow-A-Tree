using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    private const string AppearValue = "_Fill";
    private Material _material;
    private float _currentGrowValue = 0;
    public float Height { get; private set; }

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        Height = transform.localScale.y;
    }

    public void Grow(float growValue)
    {
        _currentGrowValue += growValue;
        if (_currentGrowValue > 1) _currentGrowValue = 1;
        _material.SetFloat(AppearValue, _currentGrowValue);
    }
}
