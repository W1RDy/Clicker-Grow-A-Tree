using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomText : MonoBehaviour
{
    [SerializeField] private string _index;
    public Text Text { get; private set; }
    private LocalizationService _localizationService;

    private void Start()
    {
        Text = GetComponent<Text>();
        _localizationService = ServiceLocator.Instance.Get<LocalizationService>();
        SetText(_index);
    }

    public void SetText(string index)
    {
        var message = _localizationService.GetPhrase(index);
        Debug.Log(message);
        Text.text = message;
    }
}
