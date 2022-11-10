using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinStarterSet : MonoBehaviour
{
    [SerializeField]
    private List<InteractionClosetAdd> _startingSkinPieceInteractions;

    private void Start()
    {
        for (int i = 0; i < _startingSkinPieceInteractions.Count; i++)
        {
            _startingSkinPieceInteractions[i].AddToLists();
        }
    }
}
