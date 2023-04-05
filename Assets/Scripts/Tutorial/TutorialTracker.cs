using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialTracker : MonoBehaviourSingleton<TutorialTracker>, IDataPersistence
{
    [SerializeField] private List<TutorialData> _scriptableObjects = new List<TutorialData>();
    private Dictionary<TutorialData, bool> _isTutorialComplete = new Dictionary<TutorialData, bool>();

    protected override void Awake()
    {
        base.Awake();

        foreach (var scriptableObject in _scriptableObjects)
        {
            _isTutorialComplete[scriptableObject] = false;
        }
    }

    public bool IsTutorialComplete(TutorialData tutorial)
    {
        if (_isTutorialComplete.ContainsKey(tutorial))
        {
            return _isTutorialComplete[tutorial];
        }

        Debug.Log("TutorialData object could not be found in dictionary, is it located in the correct folder?");
        return false;
    }

    public void CompletedTutorial(TutorialData tutorial)
    {
        if (_isTutorialComplete.ContainsKey(tutorial))
        {
            _isTutorialComplete[tutorial] = true;
            DataPersistenceManager.Instance.SaveGame();
            return;
        }

        Debug.Log("TutorialData object could not be found in dictionary, is it located in the correct folder?");
    }

    public void LoadData(GameData data)
    {
        // For each tutorial scriptable object, save the IsTutorialFinished boolean
        foreach (var tutorialData in data.IsTutorialComplete)
        {
            foreach (var tutorial in _isTutorialComplete)
            {
                if (tutorialData.Key == tutorial.Key.name)
                {
                    _isTutorialComplete[tutorial.Key] = tutorialData.Value;
                    break;
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // For each tutorial scriptable object, load the IsTutorialFinished boolean
        data.IsTutorialComplete.Clear();

        foreach (var tutorial in _isTutorialComplete)
        {
            data.IsTutorialComplete[tutorial.Key.name] = tutorial.Value;
        }
    }
}
