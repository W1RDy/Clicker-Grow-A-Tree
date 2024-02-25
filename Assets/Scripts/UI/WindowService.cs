using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowService : MonoBehaviour, IService
{
    [SerializeField] private Window[] _windows;
    private Dictionary<WindowType, Window> _windowsDict = new Dictionary<WindowType, Window>();

    private void Awake()
    {
        foreach (var window in _windows)
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
