using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDestoyer : MonoBehaviour, IService
{
    private CustomCamera _customCamera;
    private List<GameObject> _destroyables = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(WaitWhileRegistered());
    }

    private IEnumerator WaitWhileRegistered()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.IsRegistered);
        {
            _customCamera = ServiceLocator.Instance.Get<CustomCamera>();
        }
    }

    public void AddDestroyable(GameObject destroyable)
    {
        _destroyables.Add(destroyable);
    }

    private void Update()
    {
        CheckDestroyables();
    }

    private void CheckDestroyables()
    {
        foreach (var destroyable in _destroyables)
        {
            if (destroyable == null)
            {
                _destroyables.Remove(destroyable);
                break;
            }

            var spriteRenderer = destroyable.transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (spriteRenderer.bounds.max.y < _customCamera.GetBottomBorderPoint().y)
            {
                Destroy(destroyable);
                _destroyables.Remove(destroyable);
                break;
            }
        }
        //var destroyable = _destroyables[0];
        //if (destroyable.transform.position.y + destroyable.transform.localScale.y < _customCamera.GetBottomBorderPoint().y)
        //{
        //    Destroy(destroyable);
        //    _destroyables.Remove(destroyable);
        //}
    }
}
