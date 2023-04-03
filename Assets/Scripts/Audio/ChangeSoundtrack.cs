using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeSoundtrack : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField] 
    private AudioMixerSnapshot _audioMixerSnapshot;
    [SerializeField] 
    private AudioMixerSnapshot _audioMixerSnapshot2;
    [SerializeField] 
    private float _transitionTime;
    private void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        _audioMixerSnapshot.TransitionTo(_transitionTime);

        yield return new WaitForSeconds(_transitionTime);
        if (_audioClip != null)
        {
            SoundtrackPlayer.Instance.AudioSource.clip = _audioClip;
            SoundtrackPlayer.Instance.AudioSource.Play();
        }
        else
        {
            Debug.Log("Audioclip missing");
        }
        _audioMixerSnapshot2.TransitionTo(_transitionTime);
    }
}