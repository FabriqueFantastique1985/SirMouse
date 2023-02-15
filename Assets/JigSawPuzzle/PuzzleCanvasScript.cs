using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCanvasScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _puzzleReference;
    


    // this was put on the OnClick() of a unity button in case the player wants to view the full image reference
    public void ShowPuzzleReference()
    {
        if (_puzzleReference.activeSelf == true)
        {
            _puzzleReference.SetActive(false);
        }
        else
        {
            _puzzleReference.SetActive(true);
        }
    }
}
