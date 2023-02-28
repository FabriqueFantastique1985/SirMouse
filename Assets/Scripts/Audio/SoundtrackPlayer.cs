using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _soundtracks;
    [SerializeField]
    private bool _playRandomOrder;
    [SerializeField]
    private float _breakSeconds;

    private float _timer;
    private int _soundtrackIndex;

    private void Awake()
    {
        _audioSource.loop = false;     
        _soundtrackIndex = _soundtracks.Length;
    }

    private void Start()
    {
        PlaySoundtrack();

        //add myself to the list of ytacks in the audio controller
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            if (_timer < _breakSeconds)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                PlaySoundtrack();
            }
        }
    }

    private void PlaySoundtrack()
    {
        _timer = 0;

        if (_playRandomOrder && _soundtracks.Length > 1)
        {
            var previousSoundtrackIndex = _soundtrackIndex;

            if (previousSoundtrackIndex == _soundtrackIndex)
            {
                _soundtrackIndex = Random.Range(0, _soundtracks.Length);
            }

        }
        else
        {
            if (_soundtrackIndex < _soundtracks.Length-1)
            {
                _soundtrackIndex++;
            }
            else
            {
                _soundtrackIndex = 0;
            }
        }
        
        _audioSource.clip = _soundtracks[_soundtrackIndex];
        
        _audioSource.Play();

    }
}