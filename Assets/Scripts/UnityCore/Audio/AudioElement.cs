
using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        [System.Serializable]
        public class AudioElement
        {
            public AudioClip Clip;
            public AudioType Type;

            [Range(0f, 1f)]
            public float Volume = 1f;
            [Range(-3f, 3f)]
            public float Pitch = 1f;

            public bool RandomizePitchSlightly;
        }
    }

}
