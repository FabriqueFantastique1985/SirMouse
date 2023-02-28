
using System.Collections;
using System.Threading.Tasks;
using UnityCore.Audio;
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


            private PageController m_Menu;
            private PageController _menu
            {
                get
                {
                    var instance = PageController.Instance;
                    if (instance == null) Debug.Log("You are trying to access the Pagecontroller, but no instance was found.");
                    return instance;
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

                // Make the player a child of the GameManager (so it's part of DontDestroy)
                GameManager.Instance.Player.transform.SetParent(GameManager.Instance.transform);

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


                // updating the playfield and colliders I can cast on // TWEAK THIS
                GameManager.Instance.PlayField = FindObjectOfType<PlayField>();
                GameManager.Instance.AdjustGameSystem(GameManager.Instance.PlayField.GroundColliders);
                AudioController.Instance.VerifyAudioTracks();


                // setting the player at the correct position
                SpawnPlayerOnCorrectPosition();

                if (m_LoadingPage != PageType.None)
                {
                    await Task.Delay(1000); 
                    _menu.TurnPageOff(m_LoadingPage); 
                }

                m_SceneIsLoading = false;
            }

            private void SpawnPlayerOnCorrectPosition()
            {
                // change this so we don't iterate over all objects !!!
                InteractionLevelChange[] spawnScripts = FindObjectsOfType<InteractionLevelChange>();

                var player = GameManager.Instance.Player;

                if (spawnScripts != null)
                {
                    foreach (InteractionLevelChange spawnScript in spawnScripts)
                    {
                        // if the spawnvalues integer == the value on this script....
                        if (spawnScript.SpawnValue == _nextSceneSpawnLocation)
                        {
                            // move the player over there
                            player.Agent.enabled = false;
                            player.transform.position = spawnScript.transform.position;
                            player.Agent.enabled = true;
                        }
                    }
                }
                else
                {
                    Debug.Log("Could not find a spawn for the player, setting player to coordinates 0,0,0,");
                    // move the player over there
                    player.Agent.enabled = false;
                    player.transform.position = new Vector3(0, 0, 0);
                    player.Agent.enabled = true;
                }
            }

            private IEnumerator LoadScene()
            {
                if (m_LoadingPage != PageType.None)
                {
                    _menu.TurnPageOn(m_LoadingPage);

                    while (_menu.PageIsOn(m_LoadingPage) == false)
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
                    case SceneType.Koen_Testing_All_Start: return "Koen_Testing_All_Start";
                    case SceneType.Koen_Playground_Menu: return "Koen_Playground_Menu";
                    case SceneType.Koen_Testing_All: return "Koen_Testing_All";
                    case SceneType.Koen_Playground_Game_2: return "Koen_Playground_Game_2";
                    case SceneType.Castle_Basement_Center: return "Castle_Basement_Center";
                    case SceneType.Castle_Basement_Oubilette: return "Castle_Basement_Oubilette";
                    case SceneType.Castle_Basement_Stairs: return "Castle_Basement_Stairs";
                    case SceneType.Castle_Basement_Treasury: return "Castle_Basement_Treasury";
                    case SceneType.Castle_BathHouse: return "Castle_BathHouse";
                    case SceneType.Castle_BedroomKing: return "Castle_BedroomKing";
                    case SceneType.Castle_GateFront: return "Castle_GateFront";
                    case SceneType.Castle_Kitchen: return "Castle_Kitchen";
                    case SceneType.Castle_Library: return "Castle_Library";
                    case SceneType.Castle_ThroneRoom: return "Castle_ThroneRoom";
                    case SceneType.Cave_Bedroom: return "Cave_Bedroom";
                    case SceneType.Cave_Living: return "Cave_Living";
                    case SceneType.ForestLevel: return "ForestLevel";
                    case SceneType.Swamp: return "Swamp";
                    case SceneType.MisterWitch_Int: return "MisterWitch_Int";
                    case SceneType.PrinceTower_C: return "PrinceTower_C";
                    case SceneType.ForestDuplicate: return "ForestLevelDuplicate";
                    default:
                        Debug.Log("Scene [" + scene + "] does not contain a string for a valid scene. ");
                        return string.Empty;
                }
            }
            private SceneType StringToSceneType(string scene)
            {
                switch (scene)
                {
                    case "Koen_Testing_All_Start": return SceneType.Koen_Testing_All_Start;
                    case "Koen_Playground_Menu": return SceneType.Koen_Playground_Menu;
                    case "Koen_Testing_All": return SceneType.Koen_Testing_All;
                    case "Koen_Playground_Game_2": return SceneType.Koen_Playground_Game_2;
                    case "Castle_Basement_Center": return SceneType.Castle_Basement_Center;
                    case "Castle_Basement_Oubilette": return SceneType.Castle_Basement_Oubilette;
                    case "Castle_Basement_Stairs": return SceneType.Castle_Basement_Stairs;
                    case "Castle_Basement_Treasury": return SceneType.Castle_Basement_Treasury;
                    case "Castle_BathHouse": return SceneType.Castle_BathHouse;
                    case "Castle_BedroomKing": return SceneType.Castle_BedroomKing;
                    case "Castle_GateFront": return SceneType.Castle_GateFront;
                    case "Castle_Kitchen": return SceneType.Castle_Kitchen;
                    case "Castle_Library": return SceneType.Castle_Library;
                    case "Castle_ThroneRoom": return SceneType.Castle_ThroneRoom;
                    case "Cave_Bedroom": return SceneType.Cave_Bedroom;
                    case "Cave_Living": return SceneType.Cave_Living;
                    case "ForestLevel": return SceneType.ForestLevel;
                    case "Swamp": return SceneType.Swamp;
                    case "MisterWitch_Int": return SceneType.MisterWitch_Int;
                    case "PrinceTower_C": return SceneType.PrinceTower_C;
                    case "ForestLevelDuplicate": return SceneType.ForestDuplicate;
                    default:
                        Debug.Log("Scene [" + scene + "] does not contain a type for a valid scene. ");
                        return SceneType.None;
                }
            }

            #endregion
        }
    }
}

