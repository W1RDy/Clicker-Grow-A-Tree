using System;
using System.Collections;
using UnityEngine;

public abstract class MonoBehaviourWithDestroyableByCamera : MonoBehaviour
{
    private ObjectsDestoyer _objectsDestoyer;
    public event Action<Transform> Destroying;

    private void Start()
    {
        StartCoroutine(WaitWhileRegistered());
    }

    private IEnumerator WaitWhileRegistered()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.IsRegistered);
        {
            _objectsDestoyer = ServiceLocator.Instance.Get<ObjectsDestoyer>();
            _objectsDestoyer.AddDestroyable(gameObject);
        }
    }

    public virtual void OnDestroy()
    {
        Destroying?.Invoke(transform);
    }
}