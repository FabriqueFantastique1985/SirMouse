
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

            public static SceneController Instance;

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



            #region Unity Functions

            private void Awake()
            {
                if (Instance == null)
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
                             PageType loadingPage = PageType.None)
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
                StartCoroutine(LoadScene());
            }

            #endregion


            #region Private Functions

            private void Configure()
            {
                Instance = this;
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

                // page will have been fuly loaded here...

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
                    default:
                        Debug.Log("Scene [" + scene + "] does not contain a type for a valid scene. ");
                        return SceneType.None;
                }
            }

            #endregion
        }
    }
}

