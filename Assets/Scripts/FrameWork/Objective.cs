using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Fabrique
{
    [CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Objective", order = 2)]
    [Serializable]
    public class Objective : ScriptableObject
    {
        #region Events
        
        public delegate void ObjectiveDelegate(Objective objective);

        public event ObjectiveDelegate ObjectiveCompleteEvent;

        #endregion

        #region EditorFields

        public string Name = "objective_x";

        [SerializeField]
        private bool _isBlocked = true;

        [SerializeReference, HideInInspector, SerializeField]
        public List<ChainAction> ChainActions;
        
        [SerializeReference, HideInInspector, SerializeField]
        public List<ChainAction> InstantActions;

       // public List<ChainAction> ChainActions => _chainActions;
       // public List<ChainAction> InstantActions => _instantActions;
        
        #endregion

        #region Fields

        private bool _isCompleted = false;

        #endregion

        #region Properties


        #endregion



        public void Enter()
        {
            _isBlocked = false;
            ChainActions.ForEach(x =>
            {
                GameManager.Instance.Chain.AddAction(x);
            });
            GameManager.Instance.Chain.StartNextChainAction();
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

     //  public void AddAction(ChainAction.ChainActionType actionType, bool isInstantAction)
     //  {
     //      ChainAction newAction = new AudioAction();
     //      switch (actionType)
     //      {
     //          case ChainAction.ChainActionType.Audio:
     //              newAction = new AudioAction();
     //              break;
     //          case ChainAction.ChainActionType.Cutscene:
     //              break;
     //          case ChainAction.ChainActionType.MovePlayer:
     //              break;
     //      }

     //      AddAction(newAction, isInstantAction);
     //  }

        public void AddAction(ChainAction action, bool isInstantAction)
        {
            if (InstantActions == null) InstantActions = new List<ChainAction>();
            if (ChainActions == null) ChainActions = new List<ChainAction>();
            
            if (isInstantAction) InstantActions.Add(action);
            else ChainActions.Add(action);
            
            Debug.Log($"Objective {Name}\n" +
                      $"Instant Actions\t: {InstantActions.Count}\n" +
                      $"Chain Actions\t: {ChainActions.Count}");
        }
        

        public void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/data.dat");
            bf.Serialize(file, this);
            file.Close();
        }

        public void LoadData()
        {
            if (File.Exists(Application.persistentDataPath + "/data.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/data.dat", FileMode.Open);
                Objective data = (Objective)bf.Deserialize(file);
                file.Close();

                ChainActions = data.ChainActions;
                InstantActions = data.InstantActions;
            }
        }
        
        public static Objective FromJson(string json)
        {
            Objective newObjective = new Objective();
            JsonUtility.FromJsonOverwrite(json, newObjective);
            return newObjective;
        }
        

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}