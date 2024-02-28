using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private Sprite[] _buttonsImages;
    private Button _button;
    private AudioPlayer _audioPlayer;
    private bool _isAudio = true;
    private Image _image;

    private void Start()
    {
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _button.onClick.AddListener(ChangeAudioSettings);
    }

    private void ChangeAudioSettings()
    {
        _isAudio = !_isAudio;

        if (_isAudio) _image.sprite = _buttonsImages[0];
        else _image.sprite = _buttonsImages[1];

        _audioPlayer.SetSettings(_isAudio);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ChangeAudioSettings);
    }
}
