using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialTracker : MonoBehaviourSingleton<TutorialTracker>, IDataPersistence
{
    private Dictionary<TutorialData, bool> _isTutorialComplete = new Dictionary<TutorialData, bool>();

    protected override void Awake()
    {
        string[] assetNames = AssetDatabase.FindAssets("TutorialData", new[] { "Assets/ScriptableObjects/Tutorial" });
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var data = AssetDatabase.LoadAssetAtPath<TutorialData>(SOpath);
            _isTutorialComplete.Add(data, false);
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
            return;
        }

        Debug.Log("TutorialData object could not be found in dictionary, is it located in the correct folder?");
    }

    public void LoadData(GameData data)
    {
        // For each tutorial scriptable object, save the IsTutorialFinished boolean
    }

    public void SaveData(ref GameData data)
    {
        // For each tutorial scriptable object, load the IsTutorialFinished boolean
    }
}
