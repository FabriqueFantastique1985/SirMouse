using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialTracker : MonoBehaviourSingleton<TutorialTracker>
{
    private List<TutorialData> _tutorialData = new List<TutorialData>();

    protected override void Awake()
    {
        string[] assetNames = AssetDatabase.FindAssets("TutorialData", new[] { "Assets/ScriptableObjects/Tutorial" });
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var data = AssetDatabase.LoadAssetAtPath<TutorialData>(SOpath);
            _tutorialData.Add(data);
        }
    }

    public void ResetTutorialStates()
    {
        foreach (var tutorial in _tutorialData)
        {
            tutorial.IsTutorialFinished = false;
        }
    }
}
