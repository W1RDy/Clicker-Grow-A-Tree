using System.Collections;
using System.Collections.Generic;

public class TouchHandler : IService
{
    private TouchZone _touchZone;
    private GrowController _growController;

    public void InitializeHandler()
    {
        _touchZone = ServiceLocator.Instance.Get<TouchZone>();
        _touchZone.Touch += TouchCallback;
        _growController = ServiceLocator.Instance.Get<GrowController>();
    }

    public void TouchCallback()
    {
        _growController.Grow();
    }
}
