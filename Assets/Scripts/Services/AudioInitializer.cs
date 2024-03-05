using System.Collections;
using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField] private string _musicIndex;

    private void Start()
    {
        StartCoroutine(WaitWhileRegistered());
    }

    private IEnumerator WaitWhileRegistered()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.IsRegistered);
        {
            var audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
            if (_musicIndex != "") audioPlayer.PlayMusic(_musicIndex);
        }
    }
}
