using UnityEngine;
using UnityEngine.UI;

public class UpgradePathView : MonoBehaviour
{
    [SerializeField] private float _colorChangeValue;
    private Image[] _images;
    private Text[] _texts;
    private Button _upgradeButton;

    public void Initialize(Button upgradeButton)
    {
        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<Text>();
        _upgradeButton = upgradeButton;
    }

    public void ActivatePath()
    {
        ActivateDeactivatePath(true);
    }

    public void DeactivatePath()
    {
        ActivateDeactivatePath(false);
    }

    private void ActivateDeactivatePath(bool isActivate)
    {
        var changeValueDir = isActivate ? 1 : -1;
        foreach (Image image in _images)
        {
            image.color = new Color(image.color.r + changeValueDir * _colorChangeValue, image.color.g + changeValueDir * _colorChangeValue, image.color.b + changeValueDir * _colorChangeValue);
        }

        foreach (Text text in _texts)
        {
            text.color = new Color(text.color.r + changeValueDir * _colorChangeValue, text.color.g + changeValueDir * _colorChangeValue, text.color.b + changeValueDir * _colorChangeValue); ;
        }

        _upgradeButton.interactable = isActivate;
    }
}