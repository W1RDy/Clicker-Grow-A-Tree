using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsIndicator : MonoBehaviour
{
    [SerializeField] Text _coinsText;

    public void SetCoins(int coins)
    {
        _coinsText.text = coins.ToString();
    }
}
