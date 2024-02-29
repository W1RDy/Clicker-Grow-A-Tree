using System;
using UnityEngine;

public abstract class MonoBehaviourWithDestroyableByCamera : MonoBehaviour
{
    private ObjectsDestoyer _objectsDestoyer;
    public event Action<Transform> Destroying;

    private void Start()
    {
        _objectsDestoyer = ServiceLocator.Instance.Get<ObjectsDestoyer>();
        _objectsDestoyer.AddDestroyable(gameObject);
    }

    public virtual void OnDestroy()
    {
        Destroying?.Invoke(transform);
    }
}