using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fabrique
{
    [CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
    public class Mission : ScriptableObject
    {
        #region Events

        public delegate void MissionDelegate(Mission mission);

        public event MissionDelegate MissionCompleteEvent;

        #endregion

        #region EditorFields

        [SerializeField]
        private string _name = "Mission_x";

        public List<Objective> _objectives = new List<Objective>();

        #endregion

        #region Fields

        private int _currentObjectiveIndex = 0;
        private Objective _currentObjective;

        #endregion


        /// <summary>
        /// Starts the mission and sets the current objective
        /// </summary>
        public void StartMission()
        {
            try
            {
                SetObjective(0);
            }
            catch (Exception e)
            {
                return;
            }
            
            Debug.Log($"Mission {_name} successfully started!");
        }

        private void SetObjective(int newObjectiveIndex)
        {
            if (newObjectiveIndex >= _objectives.Count || newObjectiveIndex < 0)
            {
                Debug.LogError(
                    $"Mission: {_name}\nTried to set an objective that is out of bounds of the Objectives container!",
                    this);
                return;
            }

            if (_currentObjective != null)
            {
                _currentObjective.Exit();
                _currentObjective.ObjectiveCompleteEvent -= OnObjectiveComplete;
            }

            _currentObjectiveIndex = newObjectiveIndex;
            _currentObjective = _objectives[_currentObjectiveIndex];
            _currentObjective.Enter();
            _currentObjective.ObjectiveCompleteEvent += OnObjectiveComplete;
        }

        private void OnObjectiveComplete(Objective objective)
        {
            //  objective.OnCompleteActions.ForEach(x => _chain.AddAction(x));
            //  _chain.StartNextChainAction();

            // When our objectiveIndex is higher than the amount of objectives, we completed the mission.
            if (++_currentObjectiveIndex >= _objectives.Count)
            {
                Debug.Log($"Mission: {_name} was successfully completed.");
                MissionCompleteEvent?.Invoke(this);
                return;
            }

            // Starts the next objective on this mission.
            SetObjective(_currentObjectiveIndex);
        }
    }
}