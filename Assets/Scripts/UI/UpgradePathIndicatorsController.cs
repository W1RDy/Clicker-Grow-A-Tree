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
        _currentValueText.text = currentValue.ToString();
        _upgradeValueText.text = "+" + upgradeValue;

        _progressSlider.value = currentValue / float.Parse(_maxValueText.text);
    }
}
