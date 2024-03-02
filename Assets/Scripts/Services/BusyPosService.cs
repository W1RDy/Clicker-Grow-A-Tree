using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusyPosService
{
    private Dictionary<Transform, List<Vector2>> _busyPositions = new Dictionary<Transform, List<Vector2>>();
    private Action<Transform> RemoveOddPos;

    public BusyPosService()
    {
        RemoveOddPos = transform => RemoveBusyPos(transform);
    }

    public bool CheckPosition(Vector2 spawnPos, Transform relativeObj, float distanceBetweenBranch)
    {
        if (!_busyPositions.ContainsKey(relativeObj)) return true;

        var position = spawnPos;

        foreach (var busyPos in _busyPositions[relativeObj])
        {
            if (Mathf.Abs(busyPos.x - position.x) < 0.00001f / relativeObj.localScale.x && Mathf.Abs(busyPos.y - position.y) < distanceBetweenBranch / relativeObj.localScale.y)
            {
                return false;
            }
        }
        return true;
    }

    public void AddBusyPos(Transform relativeObj, Vector2 position)
    {
        (relativeObj.GetComponent<IGrowable>() as MonoBehaviourWithDestroyableByCamera).Destroying += RemoveOddPos;
        List<Vector2> positions;
        if (!_busyPositions.TryGetValue(relativeObj, out positions))
        {
            _busyPositions[relativeObj] = positions = new List<Vector2>();
        }
        positions.Add(position);
    }

    private void RemoveBusyPos(Transform transform)
    {
        if (_busyPositions.ContainsKey(transform)) _busyPositions.Remove(transform);
    }
}
