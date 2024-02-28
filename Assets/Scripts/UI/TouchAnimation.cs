using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TouchAnimation : IAnimation
{
    private Transform _maskTransform;
    private Transform _circleTransform;
    private GameObject _animationView;

    private float _widthPercentege;
    private bool _isActivated;

    private Sequence _spriteSequence;
    private Sequence _maskSequence;

    public TouchAnimation(Transform maskTransform, Transform circleTransform, GameObject animationView)
    {
        _maskTransform = maskTransform;
        _circleTransform = circleTransform;
        _animationView = animationView;
    }

    public void ActivateAnimation()
    {
        _isActivated = true;
        _animationView.SetActive(true);
        StartAnimationCycle();
    }

    private void StartAnimationCycle()
    {
        _spriteSequence = DOTween.Sequence();
        _maskSequence = DOTween.Sequence();

        _spriteSequence
        .Append(_circleTransform.DOScale(0.5f, 0.7f))
        .AppendInterval(0.3f)
        .Append(_circleTransform.DOScale(1f, 0.7f));
        

        _maskSequence
        .Append(DOTween.To(() => _widthPercentege, x => _widthPercentege = x, 0.3f, 0.7f))
        .OnUpdate(() => ChangeMaskScaleByWidth())
        .AppendInterval(.3f)
        .Append(DOTween.To(() => _widthPercentege, x => _widthPercentege = x, 0f, 0.7f))
        .OnUpdate(() => ChangeMaskScaleByWidth());


        _spriteSequence.Play().OnComplete(() => RestartSequence(_spriteSequence));
        _maskSequence.Play().OnComplete(() => RestartSequence(_maskSequence));
    }

    private void RestartSequence(Sequence sequence)
    {
        if (_isActivated)
            sequence.Restart();
    }

    public void DeactivateAnimation()
    {
        _isActivated = false;
        _spriteSequence.Kill();
        _maskSequence.Kill();
        _animationView.SetActive(false);
    }

    private void ChangeMaskScaleByWidth()
    {
        var newScale = _circleTransform.localScale.x - _widthPercentege * _circleTransform.localScale.x;
        _maskTransform.localScale = new Vector2(newScale, newScale);
    }
}