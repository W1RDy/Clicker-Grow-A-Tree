using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private GrowSettings _growSettings;
    [SerializeField] private TouchZone _touchZone;
    [SerializeField] private ScoreIndicator _scoreIndicator;
    [SerializeField] private Tree _tree;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private GameController _gameController;
    [SerializeField] private ObjectsDestoyer _objectsDestoyer;
    [SerializeField] private WindowService _windowService;
    [SerializeField] private ButtonService _buttonService;
    [SerializeField] private CoinsIndicator _coinsIndicator;
    private SettingsChanger _settingsChanger;
    private GrowablesService _growablesService;

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
        BindWindowService();
        BindWindowActivator();
        BindCoinsCounter();
        BindGrowablesService();
        BindGrowController();
        BindSettingsChanger();
        BindButtonService();
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
        var controller = new GrowController(_growSettings);
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

    private void BindSettingsChanger()
    {
        _settingsChanger = new SettingsChanger(_growSettings);
        ServiceLocator.Instance.Register(_settingsChanger);
    }

    private void BindButtonService()
    {
        ServiceLocator.Instance.Register(_buttonService);
    }

    private void BindWindowService()
    {
        ServiceLocator.Instance.Register(_windowService);
    }

    private void BindWindowActivator()
    {
        var windowActivator = new WindowActivator(_windowService);
        ServiceLocator.Instance.Register(windowActivator);
    }

    private void BindGrowablesService()
    {
        var growableService = new GrowablesService();
        ServiceLocator.Instance.Register(growableService);
    }

    private void BindCoinsCounter()
    {
        var coinsCounter = new CoinsCounter(_coinsIndicator);
        ServiceLocator.Instance.Register(coinsCounter);
    }
}
