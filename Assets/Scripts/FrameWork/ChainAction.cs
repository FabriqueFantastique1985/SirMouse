using System;
using UnityEngine;

namespace Fabrique
{
    [Serializable]
    public abstract class ChainAction : MonoBehaviour
    {
        #region Enums

        public enum ChainActionType
        {
            Audio,
            Cutscene,
            MovePlayer,
        }

        #endregion
        protected float _maxTime = 0.0f;

        public float MaxTime => _maxTime;

        [HideInInspector]
        public ChainActionType ActionType;

        public virtual void Execute()
        {
        }
    }
}