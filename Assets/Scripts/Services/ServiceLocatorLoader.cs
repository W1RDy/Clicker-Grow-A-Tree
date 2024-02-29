using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private GrowSettings _growSettings;
    [SerializeField] private CoinsSpawnSettings _coinsSpawnSettings;
    [SerializeField] private BranchSpawnSettingsConfig[] _branchSpawnSettingsConfigs;
    [SerializeField] private AudioData _audioData;
    private GrowSettings _growSettingsInstance;
    private CoinsSpawnSettings _coinsSpawnSettingsInstance;

    [SerializeField] private TouchZone _touchZone;
    [SerializeField] private ScoreIndicator _scoreIndicator;
    [SerializeField] private Tree _tree;
    [SerializeField] private CustomCamera _customCamera;
    [SerializeField] private GameController _gameController;
    [SerializeField] private ObjectsDestoyer _objectsDestoyer;
    [SerializeField] private WindowService _windowService;
    [SerializeField] private ButtonService _buttonService;
    [SerializeField] private CoinsIndicator _coinsIndicator;
    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private SettingsChanger _settingsChanger;
    private GrowablesService _growableService;

    [SerializeField] private AnimationsInitializer _animationsInitializer;
    [SerializeField] private UpgradePathsController _upgradePathsController; 

    private void Awake()
    {
        _growSettingsInstance = Instantiate(_growSettings);
        _coinsSpawnSettingsInstance = Instantiate(_coinsSpawnSettings);
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
        BindUpgradePathController();
        BindTouchZone();
        BindTree();
        BindCustomCamera();
    }

    private void BindServices()
    {
        BindAudioService();
        BindAudioPlayer();

        BindAnimationActivator();

        BindGameController();
        BindScoreIndicator();
        BindScoreCounter();

        BindWindowService();
        BindWindowActivator();

        BindCoinsCounter();

        BindGrowablesService();
        BindGrowController();

        BindFactoryController();

        BindSettingsChanger();
        BindButtonService();
        BindTouchHandler();
    }

    private void BindUpgradePathController()
    {
        _upgradePathsController.InitializeUpgradePaths(_growSettingsInstance);
        ServiceLocator.Instance.Register(_upgradePathsController);
    }

    private void BindAnimationActivator()
    {
        var animations = _animationsInitializer.InitializeAnimations();
        var animationActivator = new AnimationActivator(animations);
        ServiceLocator.Instance.Register(animationActivator);
    }

    private void BindAudioPlayer()
    {
        var audioPlayer = Instantiate(_audioPlayerPrefab);
        ServiceLocator.Instance.Register(audioPlayer);
    }

    private void BindAudioService()
    {
        var audioService = new AudioService(_audioData);
        ServiceLocator.Instance.Register(audioService);
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
        var controller = new GrowController(_growSettingsInstance);
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
        _settingsChanger = new SettingsChanger(_growSettingsInstance, _coinsSpawnSettingsInstance);
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
        _growableService = new GrowablesService();
        ServiceLocator.Instance.Register(_growableService);
    }

    private void BindCoinsCounter()
    {
        var coinsCounter = new CoinsCounter(_coinsIndicator);
        ServiceLocator.Instance.Register(coinsCounter);
    }

    private void BindFactoryController()
    {
        var factoriesController = new FactoriesController();
        _growableService.InitializeService(factoriesController);
        factoriesController.InitializeController(_branchSpawnSettingsConfigs, _coinsSpawnSettingsInstance);
        ServiceLocator.Instance.Register(factoriesController);
    }
}
