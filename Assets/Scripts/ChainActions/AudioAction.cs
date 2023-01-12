using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;

public class AudioAction : ChainAction
{
    [SerializeField]
    private AudioClip _clip;
    
    public AudioAction()
    {
        ActionType = ChainActionType.Audio;
    }

    public override void Execute()
    {
        base.Execute();
        // Code to play an audio clip
    }
}
