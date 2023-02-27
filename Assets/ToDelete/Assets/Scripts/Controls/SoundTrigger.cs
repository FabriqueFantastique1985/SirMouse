using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Audio")] [SerializeField]
    private AudioElement _soundEffect;
    [SerializeField]
    private AudioClip[] _audioClips;
    [SerializeField]
    private bool _playRandomFromList;

    private int _audioClipIndex;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameManager.Instance.Player.gameObject)
        {
            //PlaySound();
        }
    }

    public void PlaySound(AudioElement _testAudioClip)
    {
        if (_playRandomFromList && _audioClips.Length > 0)
        {
            var previousAudioClipIndex = _audioClipIndex;

            if (previousAudioClipIndex == _audioClipIndex)
            {
                _audioClipIndex = Random.Range(0, _audioClips.Length);
            }
        }
        _soundEffect.Clip = _audioClips[_audioClipIndex];
        AudioController.Instance.PlayAudio(_soundEffect);
    }
}
