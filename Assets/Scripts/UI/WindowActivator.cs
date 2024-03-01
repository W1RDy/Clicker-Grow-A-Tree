using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowActivator : IService
{
    private WindowService _windowService;
    private ADVService _advService;

    public WindowActivator(WindowService windowService)
    {
        _windowService = windowService;
        _advService = ServiceLocator.Instance.Get<ADVService>();
    }

    public void ActivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);
        if (!window.gameObject.activeInHierarchy)
        {
            window.gameObject.SetActive(true);
            _advService.StopADVShowing();
        }
    }

    public void DeactivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);
        if (window.gameObject.activeInHierarchy)
        {
            window.gameObject.SetActive(false);
            _advService.StartADVShowing();
        }
    }
}
