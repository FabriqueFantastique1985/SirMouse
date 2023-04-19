using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.Audio;

public class SoundtrackPlayer : MonoBehaviour
{
    public static SoundtrackPlayer Instance;

    [SerializeField]
    private AudioSource AudioSource;
    [SerializeField]
    private AudioClip[] _soundtracks;
    [SerializeField]
    private bool _playRandomOrder;
    [SerializeField]
    private float _breakSeconds;

    [SerializeField]
    private AudioMixerSnapshot _defaultSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _mutedSnapshot;

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

    public void ChangeTheSoundtrack(AudioClip audioClip, float transitionSeconds)
    {
        AudioController.Instance.StartCoroutine(TransitionMusic(audioClip, transitionSeconds));
        AudioController.Instance.StartCoroutine(TransitionFade(transitionSeconds));
    }

    IEnumerator TransitionFade(float transitionTime)
    {
        if (AudioController.Instance.MutedOst == false)
        {
            _mutedSnapshot.TransitionTo(1.5f);
        }
        
        //yield return new WaitForSeconds(transitionTime);
        yield return new WaitForSeconds(1.5f);

        if (AudioController.Instance.MutedOst == false)
        {
            _defaultSnapshot.TransitionTo(1.5f);
        }    
    }
    IEnumerator TransitionMusic(AudioClip audioClip, float transitionTime)
    {
        //yield return new WaitForSeconds(transitionTime);
        yield return new WaitForSeconds(1.5f);

        AudioSource.clip = audioClip;
        AudioSource.Play();
    }
}