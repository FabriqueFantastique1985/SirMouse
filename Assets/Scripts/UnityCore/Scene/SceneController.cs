
using System.Collections;
using System.Threading.Tasks;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCore
{
    namespace Scene
    {
        public class SceneController : MonoBehaviour
        {
            public delegate void SceneLoadDelegate(SceneType scene);

            public static SceneController SceneControllerInstance;
            public GameManager ManagerInstance;


            private PageController m_Menu;
            private PageController _menu
            {
                get 
                {
                    if (m_Menu == null) { m_Menu = PageController.Instance; }
                    if (m_Menu == null) { Debug.Log("You are trying to access the Pagecontroller, but no instance was found."); }
                    return m_Menu;
                }
            }
            private SceneType m_TargetScene;
            private PageType m_LoadingPage;
            private SceneLoadDelegate m_SceneLoadDelegate;
            private bool m_SceneIsLoading;

            private string CurrentSceneName
            {
                get
                {
                    return SceneManager.GetActiveScene().name;
                }
            }

            private int _nextSceneSpawnLocation;



            #region Unity Functions

            private void Awake()
            {
                if (SceneControllerInstance == null)
                {
                    Configure();
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            private void OnDisable()
            {
                Dispose();
            }

            #endregion


            #region Public Functions

            public void Load(SceneType scene,
                             SceneLoadDelegate sceneLoadDelegate = null,
                             bool reload = false,
                             PageType loadingPage = PageType.None,
                             int spawnLocationValue = 0)
            {
                if (loadingPage != PageType.None && _menu == null)
                {
                    return;
                }

                if (SceneCanBeLoaded(scene, reload) == false)
                {
                    return;
                }

                m_SceneIsLoading = true;
                m_TargetScene = scene;
                m_SceneLoadDelegate = sceneLoadDelegate;
                m_LoadingPage = loadingPage;
                _nextSceneSpawnLocation = spawnLocationValue;

                // newer
                ManagerInstance.Player.transform.SetParent(ManagerInstance.transform);
                // get the PlayerRefs and give it to the manager -----
                //ManagerInstance.PlayerRefs.transform.SetParent(ManagerInstance.transform);
                //Debug.Log(ManagerInstance.PlayerRefs.name + " ] has been parented to [ " + ManagerInstance.transform.name);
                /// --------

                StartCoroutine(LoadScene());
            }

            #endregion


            #region Private Functions

            private void Configure()
            {
                SceneControllerInstance = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            private void Dispose()
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }


            private async void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
            {
                if (m_TargetScene == SceneType.None)
                {
                    return;
                }

                SceneType sceneType = StringToSceneType(scene.name);
                if (m_TargetScene != sceneType)
                {
                    return;
                }

                if (m_SceneLoadDelegate != null)
                {
                    try
                    {
                        m_SceneLoadDelegate(sceneType);
                    }
                    catch (System.Exception)
                    {
                        Debug.Log("Unable to respond with sceneLoadDelegate after scene [" + sceneType + "] loaded.");
                    }
                }


                /////----- transferring the entire player -----
                ManagerInstance.PlayField = FindObjectOfType<PlayField>();
                ManagerInstance.AdjustGameSystem(ManagerInstance.PlayField.GroundColliders);

                ////-------------- only transferring the PlayerRefs component below ------------

                // get the playfield and assign it to the game manager..
                //ManagerInstance.PlayField = FindObjectOfType<PlayField>();
                //ManagerInstance.Player = FindObjectOfType<Player>();
                //ManagerInstance.Character = ManagerInstance.Player.GetComponent<Character>();

                //Debug.Log(ManagerInstance.PlayerRefs + " are the refs on new scene");
                //// assign playref to player
                //ManagerInstance.PlayerRefs.transform.SetParent(ManagerInstance.Player.transform);
                //ManagerInstance.PlayerRefs.transform.localPosition = new Vector3(0, 0, 0);
                //// assign playerref animator to animator on player
                //ManagerInstance.Character.AnimatorRM = ManagerInstance.PlayerRefs.GetComponent<Animator>();


                ////--------------

                // setting the player at the correct position
                SpawnValue[] spawnScripts = FindObjectsOfType<SpawnValue>();
                foreach (SpawnValue spawnScript in spawnScripts)
                {
                    // if the spawnvalues integer == the value on this script....
                    if (spawnScript.ValueOfThisSpawn == _nextSceneSpawnLocation)
                    {
                        // move the player over there
                        ManagerInstance.Agent.enabled = false;
                        ManagerInstance.Player.transform.position = spawnScript.transform.position;
                        ManagerInstance.Agent.enabled = true;
                    }
                }


                if (m_LoadingPage != PageType.None)
                {
                    await Task.Delay(1000);
                    _menu.TurnPageOff(m_LoadingPage);
                }

                m_SceneIsLoading = false;

            }
            private IEnumerator LoadScene()
            {
                if (m_LoadingPage != PageType.None)
                {
                    _menu.TurnPageOn(m_LoadingPage);
                    while (_menu.PageIsOn(m_LoadingPage))
                    {
                        yield return null;
                    }
                }

                string targetSceneName = SceneTypeToString(m_TargetScene);
                SceneManager.LoadScene(targetSceneName);
            }
            private bool SceneCanBeLoaded(SceneType scene, bool reload)
            {
                string targetSceneName = SceneTypeToString(scene);

                if (CurrentSceneName == targetSceneName && reload == false)
                {
                    Debug.Log("You are tring to load a scene [" + scene + "] which is already active.");
                    return false;
                }
                else if (targetSceneName == string.Empty)
                {
                    Debug.Log("The scene you are trying to load [" + scene + "] is not valid.");
                    return false;
                }
                else if (m_SceneIsLoading == true)
                {
                    Debug.Log("Unable to load scene [" + scene + "]. Another scene [" + m_TargetScene + "] is already loading.");
                    return false;
                }

                return true;
            }





            // add extra scene names made for the game to this list of strings ?? 
            private string SceneTypeToString(SceneType scene)
            {
                switch (scene)
                {
                    case SceneType.Koen_Playground_Game: return "Koen_Playground_Game";
                    case SceneType.Koen_Playground_Menu: return "Koen_Playground_Menu";
                    case SceneType.Koen_Playground_Game_1: return "Koen_Playground_Game_1";
                    case SceneType.Koen_Playground_Game_2: return "Koen_Playground_Game_2";
                    default:
                        Debug.Log("Scene [" + scene + "] does not contain a string for a valid scene. ");
                        return string.Empty;
                }
            }
            private SceneType StringToSceneType(string scene)
            {
                switch (scene)
                {
                    case "Koen_Playground_Game": return SceneType.Koen_Playground_Game;
                    case "Koen_Playground_Menu": return SceneType.Koen_Playground_Menu;
                    case "Koen_Playground_Game_1": return SceneType.Koen_Playground_Game_1;
                    case "Koen_Playground_Game_2": return SceneType.Koen_Playground_Game_2;
                    default:
                        Debug.Log("Scene [" + scene + "] does not contain a type for a valid scene. ");
                        return SceneType.None;
                }
            }

            #endregion
        }
    }
}

