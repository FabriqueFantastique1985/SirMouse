using System;
using UnityEngine;

namespace Fabrique
{
    public abstract class ChainAction : MonoBehaviour
    {
        //private delegate 
        protected float _maxTime = 0.0f;

        public float MaxTime => _maxTime;

        public virtual void Execute()
        {
        }
    }
}