using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeSoundtrack : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField] 
    private float _transitionTime;

    private void Start()
    {
        SoundtrackPlayer.Instance.ChangeTheSoundtrack(_audioClip, _transitionTime);
    }
}