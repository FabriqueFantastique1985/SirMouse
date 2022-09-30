using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] Sounds;

    private void Awake()
    {
        // current syste0000m adds a bunch of audiosources to the soundManager comparable to the amount of sounds
        // -> TOO MANY
        // ---
        // fix it by having a few audiosources (3-ish) and have them get modified depending on the specific sound settings
        for (int i = 0; i < Sounds.Length; i++)
        {
            var sound = Sounds[i];

            sound.Source = gameObject.AddComponent<AudioSource>();

            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
        }
    }
}
