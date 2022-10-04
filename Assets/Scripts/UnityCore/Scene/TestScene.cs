
using UnityCore.Menus;
using UnityEngine;


namespace UnityCore
{
    namespace Scene
    {
        public class TestScene : MonoBehaviour
        {
            public SceneController SceneControllerScript;


            private void Awake()
            {
                if (SceneControllerScript == null)
                {
                    SceneControllerScript = FindObjectOfType<SceneController>();
                }
            }


            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.M))
                {
                    SceneControllerScript.Load(SceneType.Koen_Playground_Menu, (_scene) =>
                    {
                       Debug.Log("Scene [" + _scene + "] loaded from test script");
                    }, 
                    false, 
                    PageType.Loading);
                }

                if (Input.GetKeyUp(KeyCode.G))
                {
                    SceneControllerScript.Load(SceneType.Koen_Playground_Game);
                }
            }
        }
    }
}



