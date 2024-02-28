using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _cost;
    private IGrowable _growable;
    private CoinsCounter _coinsCounter;
    private Vector2 _positionInGrowableCoord;

    private void Awake()
    {
        _coinsCounter = ServiceLocator.Instance.Get<CoinsCounter>();
    }

    public void ConnectGrowable(IGrowable growable)
    {
        _growable = growable;
        _positionInGrowableCoord = _growable.GetGrowableTransform().InverseTransformPoint(transform.position);
        if (Vector3.Distance(growable.GetFilledTopLocalPoint(), _positionInGrowableCoord) < 0.2f)
        {
            _growable = growable.GetRelativeGrowable();
            _positionInGrowableCoord = _growable.GetGrowableTransform().InverseTransformPoint(transform.position);
        }
        //Debug.Log(_growable.GetGrowableTransform());
    }

    public void Update()
    {
        if (CheckGrowable())
        {
            _coinsCounter.AddCoins(_cost);
            Destroy(gameObject);
        }
    }

    public bool CheckGrowable()
    {
        if (_growable as Tree !=  null)
        {
            return _growable.GetFilledTopGlobalPoint().y >= transform.position.y;
        }
        else
        {
            var growableFilledPos = _growable.GetFilledTopLocalPoint();
            return growableFilledPos.y >= _positionInGrowableCoord.y;
        }
    }
}
