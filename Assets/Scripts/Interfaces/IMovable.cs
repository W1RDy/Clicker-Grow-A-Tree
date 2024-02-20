using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public void Move(Vector2 endPos);
    public void ActivateMovement();
    public void DeactivateMovement();
    public bool IsCanMoving();
}
