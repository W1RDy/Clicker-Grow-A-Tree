using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicator : MonoBehaviour, IService
{
    [SerializeField] private Text _scoreText;

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}
