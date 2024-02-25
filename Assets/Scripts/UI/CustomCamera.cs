using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour, IService
{
    [SerializeField] private float _speed;
    [SerializeField] private float heightOffset;
    private Camera _camera;
    private CameraMoveController _moveController;
    private Tree _tree;

    public void InitializeCamera()
    {
        _camera = GetComponent<Camera>();
        _moveController = new CameraMoveController(_speed, transform);
        _tree = ServiceLocator.Instance.Get<Tree>();
        DeactivateMovement();
    }

    private void Update()
    {
        _moveController.Move(new Vector2(0, _tree.GetFilledTopGlobalPoint().y + heightOffset));
    }

    public void ActivateMovement()
    {
        _moveController.ActivateMovement();
    }

    public void DeactivateMovement()
    {
        _moveController.DeactivateMovement();
    }

    public bool IsMoving()
    {
        return _moveController.IsCanMoving();
    }

    public Vector2 GetBottomBorderPoint()
    {
        return new Vector2 (0, transform.position.y - _camera.orthographicSize);
    }
}
