using System.Collections.Generic;

public class CoinService
{
    private List<Coin> _coins = new List<Coin>();
    private IndexDistributor _indexDistributor;

    public CoinService() 
    {
        _indexDistributor = new IndexDistributor();
    }

    public void AddCoin(Coin coin)
    {
        coin.Index = _indexDistributor.GetFreeIndex();
        _coins.Add(coin);
    }

    public void RemoveCoin(Coin coin)
    {
        _indexDistributor.AddFreeIndex(coin.Index);
        _coins.Remove(coin);
    }

    public List<Coin> GetCoins()
    {
        return _coins;
    }
}