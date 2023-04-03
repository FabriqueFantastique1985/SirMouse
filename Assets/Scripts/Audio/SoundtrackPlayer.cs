using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class SoundtrackPlayer : MonoBehaviour
{
    public static SoundtrackPlayer Instance;

    public AudioSource AudioSource;
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
        if (Instance == null)
        {
            Instance = this;
        }

        AudioSource.loop = false;     
        _soundtrackIndex = _soundtracks.Length;
    }

    private void Start()
    {
        PlaySoundtrack();

        //add myself to the list of ytacks in the audio controller
    }

    private void Update()
    {
        if (!AudioSource.isPlaying)
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
        
        AudioSource.clip = _soundtracks[_soundtrackIndex];
        
        AudioSource.Play();

    }
}