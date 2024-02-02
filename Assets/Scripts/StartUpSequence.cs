using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartUpSequence : MonoBehaviour
{
    [SerializeField]
    private Animation _animationCanvas;
    [SerializeField]
    private AnimationClip _FFLogo;

    [SerializeField]
    private  VideoPlayer _VAFLeaderVideoPlayer;
    [SerializeField]
    private RawImage _VAFLeaderRawImage;
    [SerializeField]
    private GameObject _VAFLeaderCanvas;

    [SerializeField]
    private AnimationClip _VAFPancarte;
    [SerializeField]
    private AnimationClip _gameLogo;

    private IEnumerator _splashScreenCoroutine;

    [SerializeField]
    private Animator _titleCanvas;

    [SerializeField]
    private AudioSource _audioSourceOST;


    private void Start()
    {
        StartCoroutine(FFLogo());
    }

    IEnumerator FFLogo()
    {
        _animationCanvas.transform.gameObject.SetActive(true);
        _animationCanvas.clip = _FFLogo;
        _animationCanvas.Play();
        float seconds = _animationCanvas.clip.length;
        float inBetweenPause = 2;
        yield return new WaitForSeconds(seconds + inBetweenPause);
        _animationCanvas.transform.gameObject.SetActive(false);

        _splashScreenCoroutine = VAFLeader();
        StartCoroutine(_splashScreenCoroutine);
    }

    IEnumerator VAFLeader()
    {
        _VAFLeaderVideoPlayer.Play();
        yield return new WaitForSeconds(0.2f);
        _VAFLeaderRawImage.color = Color.white;
        float seconds = (float)_VAFLeaderVideoPlayer.clip.length;
        float inBetweenPause = 2;
        yield return new WaitForSeconds(seconds + inBetweenPause);
        _VAFLeaderCanvas.transform.gameObject.SetActive(false);

        StartCoroutine(VAFPancarte());
    }

    IEnumerator VAFPancarte()
    {
        _animationCanvas.transform.gameObject.SetActive(true);
        _animationCanvas.clip = _VAFPancarte;
        _animationCanvas.Play();
        float seconds = _animationCanvas.clip.length;
        float inBetweenPause = 2;
        yield return new WaitForSeconds(seconds + inBetweenPause);
        _animationCanvas.transform.gameObject.SetActive(false);
        TitleScreen();
    }

    private void TitleScreen()
    {
        _titleCanvas.transform.gameObject.SetActive(true);
        _audioSourceOST.Play();
    }

    public void SkipVAFLeader()
    {
        StopCoroutine(_splashScreenCoroutine);
        _VAFLeaderVideoPlayer.Stop();
        _VAFLeaderCanvas.SetActive(false);
        StartCoroutine(VAFPancarte());
    }
}