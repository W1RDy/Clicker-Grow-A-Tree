using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowService : MonoBehaviour, IService
{
    [SerializeField] private Window[] _windowsMobile;
    [SerializeField] private Window[] _windowsPC;
    private Dictionary<WindowType, Window> _windowsDict = new Dictionary<WindowType, Window>();

    private void Awake()
    {
        Window[] windows;
        if (Screen.width < Screen.height) windows = _windowsMobile;
        else windows = _windowsPC;

        foreach (var window in windows)
        {
            _windowsDict.Add(window.WindowType, window);
        }
    }

    public Window GetWindow(WindowType windowType)
    {
        return _windowsDict[windowType];
    }
}

public enum WindowType
{
    UpgradeWindow
}
