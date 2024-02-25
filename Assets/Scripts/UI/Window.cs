using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private WindowType _windowType;
    public WindowType WindowType => _windowType;
}
