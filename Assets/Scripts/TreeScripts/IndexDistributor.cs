using System.Collections.Generic;
using UnityEngine;

public class IndexDistributor
{
    private Queue<int> _freeIndexes = new Queue<int>();
    private int _maxFreeIndex;

    public void AddFreeIndex(int index)
    {
        _freeIndexes.Enqueue(index);
    }

    public int GetFreeIndex()
    {
        if (_freeIndexes.Count > 0)
        {
            return _freeIndexes.Dequeue();
        }
        else
        {
            var result = _maxFreeIndex;
            _maxFreeIndex += 1;
            return result;
        }
    }
}