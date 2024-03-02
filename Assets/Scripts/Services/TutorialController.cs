using UnityEngine;

public class TutorialController : MonoBehaviour, IService
{
    private AnimationActivator _animationActivator;
    private bool _isActivated;
    private AnimationType _currentAnimationType;

    private CoinsCounter _coinsCounter;
    private WindowService _windowService;
    private GrowSettings _growSettings;

    public void Initialize(GrowSettings growSettings)
    {
        _animationActivator = ServiceLocator.Instance.Get<AnimationActivator>();
        _coinsCounter = ServiceLocator.Instance.Get<CoinsCounter>();
        _windowService = ServiceLocator.Instance.Get<WindowService>();
        _growSettings = growSettings;
    }

    public void ActivateTutorial()
    {
        _isActivated = true;
        ActivateTutorialStep(AnimationType.Touch);
    }

    private void ActivateNextTutorialStep()
    {
        switch (_currentAnimationType)
        {
            case AnimationType.Touch:
                ActivateTutorialStep(AnimationType.OpenUpgradeWindow);
                break;
            case AnimationType.OpenUpgradeWindow:
                ActivateTutorialStep(AnimationType.UpgradeSpeed);
                break;
            case AnimationType.UpgradeSpeed:
                _animationActivator.FinishAnimation(_currentAnimationType);
                _isActivated = false;
                break;
        }
    }

    private void ActivateTutorialStep(AnimationType animationType)
    {
        _animationActivator.FinishAnimation(_currentAnimationType);
        _currentAnimationType = animationType;
        _animationActivator.ActivateAnimation(_currentAnimationType);
    }

    public void Update()
    {
        if (_isActivated)
        {
            if (_currentAnimationType == AnimationType.Touch && _coinsCounter.Coins > 0)
            {
                ActivateNextTutorialStep();
            }
            else if (_currentAnimationType == AnimationType.OpenUpgradeWindow && _windowService.GetWindow(WindowType.UpgradeWindow).gameObject.activeInHierarchy)
            {
                ActivateNextTutorialStep();
            }
            else if (_currentAnimationType == AnimationType.UpgradeSpeed && _growSettings.UpgradeProgress > 0)
            {
                ActivateNextTutorialStep();
            }
        }
    }
}