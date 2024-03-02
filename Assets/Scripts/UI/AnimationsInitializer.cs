using UnityEngine;
using UnityEngine.UI;

public class AnimationsInitializer : MonoBehaviour
{
    [Header("TouchAnimationComponents")]
    [SerializeField] private Transform _touchAnimMaskTransform;
    [SerializeField] private Transform _touchAnimCircleTransform;
    [SerializeField] private GameObject _touchAnimationView;

    [Header("UpgradeWindowButtonAnimationComponents")]
    [SerializeField] private Button _upgradeWindowButton;

    [Header("UpgradeButtonAnimationComponents")]
    [SerializeField] private Button _upgradeButtonMobile;
    [SerializeField] private Button _upgradeButtonPC;
    private Button _upgradeButton;

    public IAnimation[] InitializeAnimations()
    {
        _upgradeButton = Screen.height < Screen.width ? _upgradeButtonPC : _upgradeButtonMobile;

        return new IAnimation[3]
        {
            new TouchAnimation(_touchAnimMaskTransform, _touchAnimCircleTransform, _touchAnimationView),
            new PressButtonAnimation(_upgradeWindowButton),
            new PressButtonAnimation(_upgradeButton)
        };
    }
}

