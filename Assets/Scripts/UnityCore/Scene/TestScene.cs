
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
                //if (Input.GetKeyUp(KeyCode.M))
                //{
                //    //SceneControllerScript.Load(SceneType.Koen_Playground_Menu, (_scene) =>
                //    //{
                //    //   Debug.Log("Scene [" + _scene + "] loaded from test script");
                //    //}, 
                //    //false, 
                //    //PageType.Loading);
                //    SceneControllerScript.Load(SceneType.Koen_Playground_Menu, null, false, PageType.Loading);
                //}

                //if (Input.GetKeyUp(KeyCode.G))
                //{
                //    SceneControllerScript.Load(SceneType.Koen_Playground_Game, null, false, PageType.Loading);
                //}





                if (Input.GetKeyUp(KeyCode.G))
                {
                    SceneControllerScript.Load(SceneType.Koen_Playground_Game_1, null, false, PageType.Loading);
                }

                if (Input.GetKeyUp(KeyCode.H))
                {
                    SceneControllerScript.Load(SceneType.Koen_Playground_Game_2, null, false, PageType.Loading, 0);
                }

                if (Input.GetKeyUp(KeyCode.J))
                {
                    SceneControllerScript.Load(SceneType.Koen_Playground_Game_2, null, true, PageType.Loading, 1);
                }

                // when calling the "Load" function, dont forget to assign a spawnvalue to the interaction (and also have spawnlocations on all the other scenes)
            }
        }
    }
}



