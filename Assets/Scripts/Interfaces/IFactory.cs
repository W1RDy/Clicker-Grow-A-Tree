using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory
{
    public void LoadResources();
    public MonoBehaviour Create(Vector2 position, Quaternion rotation, Transform parent);
}
