using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : IMovable
{
    private float _speed;
    private Transform _camera;
    private bool _isCanMoving;

    public CameraMoveController(float speed, Transform transform)
    {
        _speed = speed;
        _camera = transform;
    }

    public void Move(Vector2 endPos)
    {
        if (_isCanMoving)
        {
            _camera.position = Vector3.MoveTowards(_camera.position, new Vector3(endPos.x, endPos.y, _camera.position.z), _speed * Time.deltaTime);
        }
    }

    public void ActivateMovement()
    {
        _isCanMoving = true;
    }

    public void DeactivateMovement()
    {
        _isCanMoving = false;
    }

    public bool IsCanMoving()
    {
        return _isCanMoving;
    }
}
