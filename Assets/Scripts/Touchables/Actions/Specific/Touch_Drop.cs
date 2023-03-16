using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Touch_Drop : Touch_Action, IDataPersistence
{
    [SerializeField] private GameObject _droppedInteractable;
    [SerializeField] private Animator _animatorDrop;
    private bool _hasFallen = false;

    protected override void Start()
    {
        base.Start();
        _droppedInteractable.SetActive(false);
    }

    public override void Act()
    {
        base.Act();

        if (!_hasFallen)
        {
            _droppedInteractable.SetActive(true);
            _animatorDrop.SetTrigger("Activate");
            //DataPersistenceManager.Instance.SaveGame();
            _hasFallen = true;
        }
    }

    public void LoadData(GameData data)
    {
        // TODO: Implement Load Data

        // if _hasFallen == false
        // if player is holding apple
        // OR has apple in backpack
        // OR if player dropped off apple
        // _hasFallen = true;
        // else _hasFallen = false;
    }

    public void SaveData(ref GameData data)
    {
        //data.HasAppleFallen = _hasFallen;
    }
}
