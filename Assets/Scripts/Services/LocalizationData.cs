using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationData", menuName = "Data/new LocalizationData")]
public class LocalizationData : ScriptableObject
{
    [SerializeField] private Phrase[] _phrases;

    public Phrase[] Phrases => _phrases;
}

[Serializable]
public class Phrase
{
    public string index;
    public string ruText;
    public string enText;
    public string tuText;
}