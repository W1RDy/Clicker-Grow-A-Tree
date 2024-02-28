using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PressButtonAnimation : IAnimation
{
    private Button _button;

    private Sequence _scaleSequence;
    private Sequence _colorSequence;

    private bool _isActivated;

    public PressButtonAnimation(Button button)
    {
        _button = button;
    }

    public void ActivateAnimation()
    {
        _isActivated = true;
        StartAnimationCycle();
    }

    private void StartAnimationCycle()
    {
        var startScale = _button.transform.localScale;
        var image = _button.GetComponent<Image>();
        var startColor = image.color;

        _scaleSequence = DOTween.Sequence();
        _colorSequence = DOTween.Sequence();

        _scaleSequence
            .Append(_button.transform.DOScale(new Vector2(_button.transform.localScale.x + 0.3f, _button.transform.localScale.y + 0.3f), 0.4f))
            .AppendInterval(0.1f)
            .Append(_button.transform.DOScale(startScale, 0.4f))
            .AppendInterval(0.1f);

        _colorSequence
            .Append(image.DOColor(Color.green, 0.5f))
            .Append(image.DOColor(startColor, 0.5f));

        _scaleSequence.Play().OnComplete(() => RestartSequence(_scaleSequence));
        _colorSequence.Play().OnComplete(() => RestartSequence(_colorSequence));
    }

    private void RestartSequence(Sequence sequence)
    {
        if (_isActivated) 
            sequence.Restart();
    }

    public void DeactivateAnimation()
    {
        _isActivated = false;
    }
}

