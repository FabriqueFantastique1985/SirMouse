
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    namespace Audio
    {
        public class TestAudio : MonoBehaviour
        {
            public AudioController AudioControl;

            public List<AudioElement> AudioElementals = new List<AudioElement>();
            public List<AudioElement> AudioOST = new List<AudioElement>();
            public List<AudioElement> AudioUI = new List<AudioElement>();

            private void Start()
            {
                foreach (var em in AudioElementals)
                {
                    if (em != null)
                    {
                        AudioControl.AddAudioElement(em, 2);
                    }                   
                }
                foreach (var em in AudioOST)
                {
                    if (em != null)
                    {
                        AudioControl.AddAudioElement(em, 0);
                    }                    
                }
                foreach (var em in AudioUI)
                {
                    if (em != null)
                    {
                        AudioControl.AddAudioElement(em, 1);
                    }                   
                }
            }


            private void Update()
            {
                /// world sounds testing ///
                if (Input.GetKeyUp(KeyCode.T))
                {
                    AudioControl.PlayAudio(AudioElementals[0].Clip, AudioElementals[0].Type);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    AudioControl.StopAudio(AudioElementals[0].Type);
                }
                if (Input.GetKeyUp(KeyCode.B))
                {
                    AudioControl.RestartAudio(AudioElementals[0].Type);
                }

                if (Input.GetKeyUp(KeyCode.Y))
                {
                    AudioControl.PlayAudio(AudioElementals[1].Clip, AudioElementals[1].Type);
                }


                /// OST testing ///
                if (Input.GetKeyUp(KeyCode.U))
                {
                    AudioControl.PlayAudio(AudioOST[0].Clip, AudioOST[0].Type);
                }
                if (Input.GetKeyUp(KeyCode.J))
                {
                    AudioControl.StopAudio(AudioOST[0].Type);
                }
                if (Input.GetKeyUp(KeyCode.M))
                {
                    AudioControl.RestartAudio(AudioOST[0].Type);
                }

                if (Input.GetKeyUp(KeyCode.I))
                {
                    AudioControl.PlayAudio(AudioOST[1].Clip, AudioOST[1].Type);
                }



                /// UI testing ///
                if (Input.GetKeyUp(KeyCode.E))
                {
                    AudioControl.PlayAudio(AudioUI[0].Clip, AudioUI[0].Type);
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    AudioControl.StopAudio(AudioUI[0].Type);
                }
                if (Input.GetKeyUp(KeyCode.C))
                {
                    AudioControl.RestartAudio(AudioUI[0].Type);
                }

                if (Input.GetKeyUp(KeyCode.R))
                {
                    AudioControl.PlayAudio(AudioUI[1].Clip, AudioUI[1].Type);
                }
            }
        }
    }
}

