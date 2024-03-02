using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationService : IService
{
    private LocalizationData _data;
    private string _languageIndex;
    private Dictionary<string, Phrase> _phrases = new Dictionary<string, Phrase>();

    public LocalizationService(LocalizationData data)
    {
        _data = data;
        InitDictionary();
    }

    private void InitDictionary()
    {
        foreach (var phrase in _data.Phrases)
        {
            _phrases.Add(phrase.index, phrase);
        }
    }

    private void GetLocalizationLanguage()
    {
        _languageIndex = InteractorWithBrowser.GetLanguage();
    }

    public string GetPhrase(string index)
    {
        if (_languageIndex == "" || _languageIndex == null)
        {
            GetLocalizationLanguage();
        }
        var phrase = _phrases[index];
        switch (_languageIndex)
        {
            case "ru":
                return phrase.ruText;
            case "en":
                return phrase.enText;
            case "tr":
                return phrase.tuText;
        }

        throw new System.ArgumentNullException("Phrase with index " + index + " doesn't exist!");
    }
}