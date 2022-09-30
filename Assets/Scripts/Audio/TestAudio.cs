
using UnityEngine;


namespace Audio
{
    public class TestAudio : MonoBehaviour
    {
        public AudioController AudioControl;


        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.T))
            {
                AudioControl.PlayAudio(AudioType.ST_01, true, 1.0f);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                AudioControl.StopAudio(AudioType.ST_01, true);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                AudioControl.RestartAudio(AudioType.ST_01, true);
            }


            if (Input.GetKeyUp(KeyCode.Y))
            {
                AudioControl.PlayAudio(AudioType.SFX_01);
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                AudioControl.StopAudio(AudioType.SFX_01);
            }
            if (Input.GetKeyUp(KeyCode.N))
            {
                AudioControl.RestartAudio(AudioType.SFX_01);
            }
        }
    }
}

