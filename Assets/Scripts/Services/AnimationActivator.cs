public class AnimationActivator : IService
{
    private TouchAnimation _touchAnimation;
    private PressButtonAnimation _pressUpgradeWindowButtonAnim;
    private PressButtonAnimation _pressUpgradeButtonAnim;

    public AnimationActivator(IAnimation[] animations)
    {
        _touchAnimation = animations[0] as TouchAnimation;
        _pressUpgradeWindowButtonAnim = animations[1] as PressButtonAnimation;
        _pressUpgradeButtonAnim = animations[2] as PressButtonAnimation;
    }

    public void ActivateAnimation(AnimationType animationType)
    {
        var animation = GetAnimation(animationType);
        animation.ActivateAnimation();
    }

    public void FinishAnimation(AnimationType animationType)
    {
        var animation = GetAnimation(animationType);
        animation.DeactivateAnimation();
    }

    private IAnimation GetAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Touch:
                return _touchAnimation;
            case AnimationType.OpenUpgradeWindow:
                return _pressUpgradeWindowButtonAnim;
            case AnimationType.UpgradeSpeed:
                return _pressUpgradeButtonAnim;
        }
        throw new System.ArgumentNullException("Animation with type " + animationType + " doesn't exist!");
    }
}

public enum AnimationType
{
    Touch,
    OpenUpgradeWindow,
    UpgradeSpeed
}
