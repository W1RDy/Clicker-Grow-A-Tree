using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : IMovable
{
    private float _speed;
    private Transform _camera;
    private bool _isCanMoving;
    private float _speedMultiplyer = 1f;

    public CameraMoveController(float speed, Transform transform)
    {
        SetSpeed(speed);
        _camera = transform;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void Move(Vector2 endPos)
    {
        if (_isCanMoving)
        {
            ChangeSpeedByDistance(endPos);
            _camera.position = Vector3.MoveTowards(_camera.position, new Vector3(endPos.x, endPos.y, _camera.position.z), _speed * _speedMultiplyer * Time.deltaTime);
        }
    }

    private void ChangeSpeedByDistance(Vector2 endPos)
    {
        _speedMultiplyer = (endPos.y - _camera.position.y) / _camera.localScale.y;
        _speedMultiplyer = Mathf.Clamp(_speedMultiplyer, 1f, 20f);
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
