using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IService
{
    private Tree _tree;
    private CustomCamera _camera;
    private TutorialController _tutorialController;
    public event Action FinishGame;

    public void InitializeController()
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _camera = ServiceLocator.Instance.Get<CustomCamera>();
        _tutorialController = ServiceLocator.Instance.Get<TutorialController>();
    }

    private void Start()
    {
        _tutorialController.ActivateTutorial();
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
