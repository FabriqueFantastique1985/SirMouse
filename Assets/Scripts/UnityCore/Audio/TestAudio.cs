
using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        public class TestAudio : MonoBehaviour
        {
            public AudioController AudioControl;


            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.T))
                {
                    AudioControl.PlayAudio(AudioType.OST, true, 1.0f);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    AudioControl.StopAudio(AudioType.OST, true);
                }
                if (Input.GetKeyUp(KeyCode.B))
                {
                    AudioControl.RestartAudio(AudioType.OST, true);
                }



                if (Input.GetKeyUp(KeyCode.Y))
                {
                    AudioControl.PlayAudio(AudioType.SFX_World);
                }
                if (Input.GetKeyUp(KeyCode.H))
                {
                    AudioControl.StopAudio(AudioType.SFX_World);
                }
                if (Input.GetKeyUp(KeyCode.N))
                {
                    AudioControl.RestartAudio(AudioType.SFX_World);
                }



                if (Input.GetKeyUp(KeyCode.U))
                {
                    AudioControl.PlayAudio(AudioType.SFX_UI);
                }
                if (Input.GetKeyUp(KeyCode.J))
                {
                    AudioControl.StopAudio(AudioType.SFX_UI);
                }
                if (Input.GetKeyUp(KeyCode.M))
                {
                    AudioControl.RestartAudio(AudioType.SFX_UI);
                }
            }
        }
    }
}

