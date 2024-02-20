using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private TouchZone _touchZone;
    [SerializeField] private ScoreIndicator _scoreIndicator;
    [SerializeField] private Tree _tree;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private GameController _gameController;
    [SerializeField] private ObjectsDestoyer _objectsDestoyer;

    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        BindMonoBehaviours();
        BindServices();
    }

    private void BindMonoBehaviours()
    {
        BindObjectDestroyer();
        BindTouchZone();
        BindTree();
        BindCustomCamera();
    }

    private void BindServices()
    {
        BindGameController();
        BindScoreIndicator();
        BindScoreCounter();
        BindGrowController();
        BindTouchHandler();
    }

    private void BindTouchHandler()
    {
        var handler = new TouchHandler();
        handler.InitializeHandler();
        ServiceLocator.Instance.Register(handler);
    }

    private void BindTouchZone()
    {
        ServiceLocator.Instance.Register(_touchZone);
    }   
    
    private void BindScoreCounter()
    {
        var counter = new ScoreCounter();
        counter.InitializeCounter();
        ServiceLocator.Instance.Register(counter);
    }

    private void BindScoreIndicator()
    {
        ServiceLocator.Instance.Register(_scoreIndicator);
    }

    private void BindTree()
    {
        ServiceLocator.Instance.Register(_tree);
    }

    private void BindGrowController()
    {
        var controller = new GrowController();
        ServiceLocator.Instance.Register(controller);
    }

    private void BindCustomCamera()
    {
        _customCamera.InitializeCamera();
        ServiceLocator.Instance.Register(_customCamera);
    }

    private void BindGameController()
    {
        _gameController.InitializeController();
        ServiceLocator.Instance.Register(_gameController);
    }

    private void BindObjectDestroyer()
    {
        ServiceLocator.Instance.Register(_objectsDestoyer);
    }
}
