using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene
{
    public class TestScene : MonoBehaviour
    {
        public SceneController SceneControllerScript;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.M))
            {
                SceneControllerScript.Load(SceneType.Menu, (_scene) =>
                {
                    Debug.Log("Scene [" + _scene + "] loaded from test script");
                }, false, PageTypeLoading);
            }

            if (Input.GetKeyUp(KeyCode.G))
            {
                SceneControllerScript.Load(SceneType.Game);
            }
        }
    }
}


