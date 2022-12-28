using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fabrique
{
    [CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Objective", order = 1)]
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
        private List<ChainAction> _onStartActions = new List<ChainAction>();
        
        [SerializeField]
        private List<ChainAction> _onCompleteActions = new List<ChainAction>();

        [SerializeField]
        private bool _isBlocked = true;
        
        #endregion

        #region Fields

        private bool _isCompleted = false;

        #endregion        

        
        public void Enter()
        {
            _isBlocked = false;
            
            _onStartActions.ForEach(x => GameManager.Instance.Chain.AddAction(x)); 
        }

        public void SetObjectiveReached()
        {
            if (_isBlocked || _isCompleted) return;

            _isCompleted = true;
            
            _onCompleteActions.ForEach(x => GameManager.Instance.Chain.AddAction(x)); 
            
            Debug.Log($"Objective {this.name} is reached");
            ObjectiveCompleteEvent?.Invoke(this);
        }

        public void Exit()
        {
        }
    }
}