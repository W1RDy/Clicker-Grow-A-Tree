using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowActivator : IService
{
    private WindowService _windowService;

    public WindowActivator(WindowService windowService)
    {
        _windowService = windowService;
    }

    public void ActivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);
        if (!window.gameObject.activeInHierarchy) window.gameObject.SetActive(true);
    }

    public void DeactivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);
        if (window.gameObject.activeInHierarchy) window.gameObject.SetActive(false);
    }
}
