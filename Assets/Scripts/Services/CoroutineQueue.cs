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
        _isCrowded = _queue.Count >= _maxCounts;
        if (!_isCrowded)
        {
            _queue.Enqueue(routine);
            if (_queue.Count == 1)
            {
                StartQueueCycle();
            }
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
            Debug.Log(_queue.Count);
            yield return _monoBeh.StartCoroutine(routine);
            _queue.Dequeue();
        }
        _isCrowded = false;
    }
}
