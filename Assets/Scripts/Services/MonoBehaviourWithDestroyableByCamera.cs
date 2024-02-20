using UnityEngine;

public abstract class MonoBehaviourWithDestroyableByCamera : MonoBehaviour
{
    private ObjectsDestoyer _objectsDestoyer;

    private void Start()
    {
        _objectsDestoyer = ServiceLocator.Instance.Get<ObjectsDestoyer>();
        _objectsDestoyer.AddDestroyable(gameObject);
    }
}