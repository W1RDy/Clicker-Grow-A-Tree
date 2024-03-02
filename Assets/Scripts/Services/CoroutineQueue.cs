using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineQueue
{
    private Queue<IEnumerator> _queue;
    private MonoBehaviour _monoBeh;
    private int _maxCounts;
    private bool _isCrowded;
    public bool IsCrowded => _isCrowded;

    public CoroutineQueue(MonoBehaviour monoBehaviour, int maxElementsCount)
    {
        _queue = new Queue<IEnumerator>();
        _monoBeh = monoBehaviour;
        _maxCounts = maxElementsCount;
    }

    public void StartCoroutineWithQueue(IEnumerator routine)
    {
        if (!_isCrowded)
        {
            _isCrowded = _queue.Count >= _maxCounts;
            StartRequiredCoroutineWithQueue(routine);
        }
    }

    public void StartRequiredCoroutineWithQueue(IEnumerator routine)
    {
        _queue.Enqueue(routine);
        if (_queue.Count == 1)
        {
            StartQueueCycle();
        }
    }

    private void StartQueueCycle()
    {
        _monoBeh.StartCoroutine(QueueCycle());
    }

    private IEnumerator QueueCycle()
    {
        while (_queue.Count > 0)
        {
            var routine = _queue.Peek();
            yield return _monoBeh.StartCoroutine(routine);
            _queue.Dequeue();
        }
        _isCrowded = false;
    }
}
