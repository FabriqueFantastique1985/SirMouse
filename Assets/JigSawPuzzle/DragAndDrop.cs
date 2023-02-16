using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static MiniGame;

public class DragAndDrop : MonoBehaviour
{
    public delegate void DragAndDropDelegate();

    public event DragAndDropDelegate OnPuzzleCompleted;
    public event DragAndDropDelegate OnPuzzleRestarted;

    public Camera CameraPuzzle;

    [SerializeField]
    private LayerMask _ignoreMe;

    private bool _clickedPiece;
    [HideInInspector]
    public GameObject SelectedPiece;
    private PuzzlePieceScript _selectedPieceScript;

    private int _orderInLayerSelectedPiece;

    public GameObject _prefabParticleSuccess;

    [SerializeField]
    private int _rowAmount;
    [SerializeField]
    private int _collumnAmount;

    private int _correctAmount;

    // !! call this on interaction for puzzle game !!
    public void StartMiniGame()
    {
        // call gameManager to block the input OR change to minigamesystem
        GameManager.Instance.BlockInput = true;

        // hide the buttons for the closet and backpack
        GameManager.Instance.PanelUIButtonsClosetAndBackpack.SetActive(false);

        // enable the update on this script
        this.enabled = true;
        gameObject.SetActive(true);

        if (_correctAmount == (_collumnAmount * _rowAmount))
        {
            OnPuzzleRestarted?.Invoke();

            // reset variables
            _correctAmount = 0;
        }
    }

    // call this on button puzzle (DONE)
    public void EndMiniGame()
    {
        // disable the update on this script
        this.enabled = false;
        gameObject.SetActive(false);


        // un-hide the buttons for the closet and backpack
        GameManager.Instance.PanelUIButtonsClosetAndBackpack.SetActive(true);

        // call gameManager to un-block the input OR change to maingameSystem
        GameManager.Instance.BlockInput = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = CameraPuzzle.ScreenPointToRay(Input.mousePosition); // Ray that represents finger press
            RaycastHit hit; // Object hit by ray

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~_ignoreMe) && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Hit puzzle " + hit.transform.name);

                // assign the puzzlePiece index in this if statement !!!!
                if (hit.collider.gameObject.layer == 17)
                {
                    SelectedPiece = hit.transform.gameObject;
                    _selectedPieceScript = SelectedPiece.GetComponent<PuzzlePieceScript>();

                    AdjustOrderPiece(true);

                    _clickedPiece = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_clickedPiece == true)
            {
                _clickedPiece = false;

                AdjustOrderPiece(false);

                if (_selectedPieceScript.CheckLatchOnSpot())
                {
                    ++_correctAmount;
                    if (_prefabParticleSuccess != null)
                    {
                        Instantiate(_prefabParticleSuccess, SelectedPiece.transform.position, Quaternion.identity);
                    }
                }
                
                SelectedPiece = null;
                _selectedPieceScript = null;
            }
        }

        if (_clickedPiece == true)
        {
            SelectedPiece.transform.position = CameraPuzzle.ScreenToWorldPoint(Input.mousePosition);
        }

        if (_correctAmount == (_collumnAmount * _rowAmount))
        {
            EndMiniGame();
            OnPuzzleCompleted?.Invoke();
        }
    }

    private void AdjustOrderPiece(bool increaseOrder)
    {
        if (increaseOrder == true)
        {
            SelectedPiece.GetComponent<SortingGroup>().sortingOrder = 11;
        }
        else
        {
            SelectedPiece.GetComponent<SortingGroup>().sortingOrder = 10;
        }
    }
}
