using System;
using UnityEngine;

public class TrunkPart : MonoBehaviourWithDestroyableByCamera, IGrowable
{
    //[SerializeField] private SpawnBranchSettings[] _branchMarkers;
    private const string AppearValue = "_Fill";
    private Material _material;
    public float Height { get; private set; }

    public void InitializeTrunk(Action<Transform> _callback)
    {
        Height = transform.localScale.y;
        _material = GetComponent<SpriteRenderer>().material;
        
        //for (int i = 0; i < _branchMarkers.Length; i++)
        //{
        //    var newSettings = new SpawnBranchSettings(transform.TransformPoint(_branchMarkers[i].SpawnPos), _branchMarkers[i].SpawnRot, false);
        //    _branchMarkers[i] = newSettings;
        //}
        _callback?.Invoke(transform);
    }

    public void Grow(float fillingValue)
    {
        if (fillingValue > 1) fillingValue = 1;
        _material.SetFloat(AppearValue, fillingValue);
    }
}
