using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Touch_Action : MonoBehaviour
{
    protected Touchable _touchableScript;

    protected const string _animPop = "Spawnable_Pop";
    protected const string _animIdle = "Spawnable_Scaler";

    // audio // specifically for events of : tap / let go
    //[Header("Audio")]
    //public List<AudioElement> AudioElements = new List<AudioElement>();

    [Header("Audio")]
    public AudioElement Pickup;
    public AudioElement Drop;
    public AudioElement Disappear;


    protected virtual void Start()
    {
        _touchableScript = GetComponent<Touchable>();
    }



    public virtual void Act()
    {
        //AudioController.Instance.PlayAudio(AudioElements[Random.Range(0, AudioElements.Count)]);
        PlayAudio(Pickup);
        StartCoroutine(EnableInputDetectionAgain());
    }

    public virtual void PlayAudio(AudioElement audioEMToPlay)
    {
        if (audioEMToPlay.Clip != null)
        {
            AudioController.Instance.PlayAudio(audioEMToPlay);
        }
    }

    private IEnumerator EnableInputDetectionAgain()
    {
        yield return new WaitForSeconds(0.1f);

        GameManager.Instance.BlockInput = false;
    }
}
