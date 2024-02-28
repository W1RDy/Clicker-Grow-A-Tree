using System.Collections;
using System.Collections.Generic;

public class TouchHandler : IService
{
    private TouchZone _touchZone;
    private GrowController _growController;
    private GameController _gameController;

    public void InitializeHandler()
    {
        _touchZone = ServiceLocator.Instance.Get<TouchZone>();
        _touchZone.Touch += TouchCallback;
        _growController = ServiceLocator.Instance.Get<GrowController>();
        _gameController = ServiceLocator.Instance.Get<GameController>();
    }

    public void TouchCallback()
    {
        _growController.Grow();
        _gameController.FinishTouchWaiting();
    }
}
