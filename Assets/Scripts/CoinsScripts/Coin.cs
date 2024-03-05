using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviourWithDestroyableByCamera
{
    public int Index { get; set; }
    private CoinsSpawnSettings _settings;
    private IGrowable _growable;
    private CoinsCounter _coinsCounter;
    private Vector2 _positionInGrowableCoord;
    private AudioPlayer _audioPlayer;

    public void Initialize(IGrowable growable, CoinsSpawnSettings coinsSpawnSettings)
    {
        _coinsCounter = ServiceLocator.Instance.Get<CoinsCounter>();
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();

        _settings = coinsSpawnSettings;
        _growable = growable;
        _positionInGrowableCoord = _growable.GetGrowableTransform().InverseTransformPoint(transform.position);
        if (Vector3.Distance(growable.GetFilledTopLocalPoint(), _positionInGrowableCoord) < 0.2f)
        {
            _growable = growable.GetRelativeGrowable();
            _positionInGrowableCoord = _growable.GetGrowableTransform().InverseTransformPoint(transform.position);
        }
    }

    public void Update()
    {
        if (CheckGrowable())
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        _coinsCounter.AddCoins(_settings.CoinsCosts);
        _audioPlayer.PlaySounds("CollectLeaf");
        Destroy(gameObject);
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
