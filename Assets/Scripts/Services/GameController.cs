using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IService
{
    private Tree _tree;
    private CustomCamera _camera;
    private TouchHandler _touchHandler;
    private AnimationActivator _animationActivator;
    public event Action FinishGame;

    public void InitializeController()
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _camera = ServiceLocator.Instance.Get<CustomCamera>();
        _animationActivator = ServiceLocator.Instance.Get<AnimationActivator>();
    }

    private void Start()
    {
        _animationActivator.ActivateAnimation(AnimationType.Touch);
    }

    public void FinishTouchWaiting()
    {
        _animationActivator.FinishAnimation(AnimationType.Touch);
        _animationActivator.ActivateAnimation(AnimationType.OpenUpgradeWindow);
    }

    private void Update()
    {
        if (!_camera.IsMoving() && _tree.GetFilledTopGlobalPoint().y > 0)
            _camera.ActivateMovement();
    }

    private void OnDestroy()
    {
        FinishGame?.Invoke();
    }
}
