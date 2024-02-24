using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IService
{
    private Tree _tree;
    private CustomCamera _camera;

    public void InitializeController()
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _camera = ServiceLocator.Instance.Get<CustomCamera>();
    }

    private void Update()
    {
        if (!_camera.IsMoving() && _tree.GetTopTreePoint().y > 0)
            _camera.ActivateMovement();
    }
}
