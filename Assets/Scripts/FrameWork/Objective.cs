using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fabrique
{
    [CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Objective", order = 2)]
    public class Objective : ScriptableObject
    {
        #region Events
        
        public delegate void ObjectiveDelegate(Objective objective);

        public event ObjectiveDelegate ObjectiveCompleteEvent;

        #endregion

        #region EditorFields

        [SerializeField]
        private string _name = "objective_x";

        [SerializeField]
        private bool _isBlocked = true;
        
        #endregion

        #region Fields

        private bool _isCompleted = false;

        #endregion

        #region Properties


        #endregion

        
        public void Enter()
        {
            _isBlocked = false;
            
        }

        public void SetObjectiveReached()
        {
            if (_isBlocked || _isCompleted) return;

            _isCompleted = true;
            
            Debug.Log($"Objective {this.name} is reached");
            ObjectiveCompleteEvent?.Invoke(this);
        }

        public void Exit()
        {
        }

        /*public void AddChainAction(ChainAction.ChainActionType actionType)
        {
            ChainAction newAction = new PlayAudioAction();
            switch (actionType)
            {
                case ChainAction.ChainActionType.Audio:
                    newAction = new PlayAudioAction();
                    break;
                case ChainAction.ChainActionType.Cutscene:
                    break;
                case ChainAction.ChainActionType.MovePlayer:
                    break;
            }

            _onStartActions.Add(newAction);
        }*/
    }
}