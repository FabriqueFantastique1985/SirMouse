using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Touch_Action : MonoBehaviour
{
    protected Touchable _touchableScript;

    // audio
    public List<AudioElement> AudioElements = new List<AudioElement>();

    protected virtual void Start()
    {
        _touchableScript = GetComponent<Touchable>();

        var audioControl = AudioController.Instance;
        // add the possible sound effects to the AudioTable and the correct track
        foreach (AudioElement audioEm in AudioElements)
        {
            if (audioEm.Clip != null)
            {
                // there exists 1 Type more than there are Tracks -> move down by 1
                audioControl.AddAudioElement(audioEm, ((int)audioEm.Type) - 1);
            }
        }
    }



    public virtual void Act()
    {
        // nothing special on the base script
    }
}
