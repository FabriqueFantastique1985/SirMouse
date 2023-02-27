using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterSounds : MonoBehaviour
{

    [Header("Footsteps")]
    [SerializeField]
    private AudioElement _footstepsAudioElement;
    [SerializeField]
    private AudioClip[] _footstepsAudioClips;
    private int _footstepsIndex;

    [Header("Yawn")]
    [SerializeField]
    private AudioElement _yawnAudioElement;
    [SerializeField]
    private AudioClip[] _yawnAudioClips;
    private int _yawnIndex;


    private void PlaySound(AudioElement audioElement)
    {
        AudioController.Instance.PlayAudio(audioElement);
    }

    private void PlaySound(AudioElement audioElement, AudioClip[] audioClips, int audioClipIndex, bool randomFromList)
    {
        if (randomFromList && audioClips.Length > 0)
        {
            var previousAudioClipIndex = audioClipIndex;

            if (previousAudioClipIndex == audioClipIndex)
            {
                audioClipIndex = Random.Range(0, audioClips.Length);
            }
        }
        audioElement.Clip = audioClips[audioClipIndex];
        AudioController.Instance.PlayAudio(audioElement);
    }

    public void PlayFootstepSound()
    {
        PlaySound(_footstepsAudioElement, _footstepsAudioClips, _footstepsIndex, true);
    }

    public void PlayYawnSound()
    {
        PlaySound(_yawnAudioElement, _yawnAudioClips, _yawnIndex, true);
    }
}
