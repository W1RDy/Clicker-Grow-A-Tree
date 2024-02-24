using System;
using UnityEngine;

public class TrunkPart : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    private const string AppearValue = "_Fill";
    private Material _material;
    public float Height { get; private set; }

    public void InitializeTrunk(Action<Transform> _callback)
    {
        Height = transform.localScale.y;
        _material = GetComponent<SpriteRenderer>().material;
        _callback?.Invoke(transform);
    }

    public void Grow(float fillingValue)
    {
        if (fillingValue > 1) fillingValue = 1;
        _material.SetFloat(AppearValue, fillingValue);
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        var fillingValue = _material.GetFloat(AppearValue);
        var fillingHeight = Height * fillingValue;
        return new Vector2(0, (-(Height / 2) + fillingHeight)) / Height;
    }
}
