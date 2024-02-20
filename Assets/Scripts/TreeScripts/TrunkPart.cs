using System;
using UnityEngine;

public class TrunkPart : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    [SerializeField] private Transform[] _branchMarkers;
    private const string AppearValue = "_Fill";
    private Material _material;
    public float Height { get; private set; }

    public void InitializeTrunk(Action<Transform[]> _markersCallback)
    {
        Height = transform.localScale.y;
        _material = GetComponent<SpriteRenderer>().material;
        Debug.Log(_markersCallback);
        _markersCallback?.Invoke(_branchMarkers);
    }

    public void Grow(float fillingValue)
    {
        if (fillingValue > 1) fillingValue = 1;
        _material.SetFloat(AppearValue, fillingValue);
    }
}
