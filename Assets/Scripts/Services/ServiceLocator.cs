using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private Dictionary<Type, IService> _servicesDictionary = new Dictionary<Type, IService>();
    public static ServiceLocator Instance;
    public bool IsRegistered { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    public void Register<T>(T service) where T : IService
    {
        var type = service.GetType();
        if (_servicesDictionary.ContainsKey(type)) throw new ArgumentException("Service with type " + typeof(T) + " already exists!");
        _servicesDictionary.Add(type, service);
    }

    public T Get<T>() where T : IService
    {
        if (_servicesDictionary.TryGetValue(typeof(T), out var result))
        {
            return (T)result;
        }
        throw new ArgumentException("Service with type " + typeof(T) + " doesn't exists!");
    }
}
