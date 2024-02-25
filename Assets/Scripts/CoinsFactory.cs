using UnityEngine;

public class CoinsFactory : IFactory
{
    private const string CoinsPath = "Coin";

    private Coin _coinPrefab;
    private Transform _container;

    public CoinsFactory(Transform container)
    {
        _container = container;
    }

    public void LoadResources()
    {
        _coinPrefab = Resources.Load<Coin>(CoinsPath);
    }

    public MonoBehaviour Create(Vector2 position, Quaternion rotation, Transform parent)
    {
        var coin = UnityEngine.Object.Instantiate(_coinPrefab, position, rotation, parent);
        coin.transform.SetParent(_container);
        return coin;
    }
}
