using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePathIndicatorsController : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Text _currentValueText;
    [SerializeField] private Text _maxValueText;
    [SerializeField] private Text _upgradeValueText;

    public void InitializeIndicators(float upgradeValue, (float currentParameter, float maxParameter) parameters)
    {
        _maxValueText.text = parameters.maxParameter.ToString();
        UpdateIndicators(upgradeValue, parameters.currentParameter);
    }

    public void UpdateIndicators(float upgradeValue, float currentValue)
    {
        _currentValueText.text = Math.Round(currentValue,3, MidpointRounding.AwayFromZero).ToString();
        if (upgradeValue == 0) _upgradeValueText.text = "";
        else _upgradeValueText.text = "+" + Math.Round(upgradeValue, 3, MidpointRounding.AwayFromZero);

        _progressSlider.value = currentValue / float.Parse(_maxValueText.text);
    }
}
