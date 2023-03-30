using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "ScriptableObjects/Tutorial")]
public class TutorialData : ScriptableObject, IDataPersistence
{
    private bool _isTutorialFinished = false;
    public bool IsTutorialFinished
    {
        get { return _isTutorialFinished; }
        set 
        { 
            _isTutorialFinished = value;
            DataPersistenceManager.Instance?.SaveGame();
        }
    }

    [field: SerializeField] private GameObject _tutorialObject;
    public GameObject TutorialObject
    {
        get { return _tutorialObject; }
        private set { _tutorialObject = value; }
    }

    public void LoadData(GameData data)
    {
        if (data._isTutorialComplete.ContainsKey(this))
        {
            _isTutorialFinished = data._isTutorialComplete[this];
        }
        else
        {
            _isTutorialFinished = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data._isTutorialComplete.ContainsKey(this))
        {
            data._isTutorialComplete[this] = _isTutorialFinished;
        }
        else
        {
            data._isTutorialComplete.Add(this, _isTutorialFinished);
        }
    }
}
