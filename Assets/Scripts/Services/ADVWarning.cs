using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ADVWarning : MonoBehaviour
{
    [SerializeField] private Text _warningText;

    public void ActivateWarning()
    {
        _warningText.gameObject.SetActive(true);
        _warningText.DOFade(1, 0.2f);
    }

    public void DeactivateWarning() 
    {
        _warningText.gameObject.SetActive(false);
    }
}