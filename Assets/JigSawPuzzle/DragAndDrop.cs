using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DragAndDrop : MonoBehaviour
{
    public Camera CameraPuzzle;

    // NEEDS MORE // assign EVERYTHING BUT the puzzlePiece & board layers in the inspector
    [SerializeField]
    private LayerMask _ignoreMe;

    private bool _clickedPiece;
    [HideInInspector]
    public GameObject SelectedPiece;
    private PuzzlePieceScript _selectedPieceScript;

    private int _orderInLayerSelectedPiece;

    public GameObject _prefabParticleSuccess;



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = CameraPuzzle.ScreenPointToRay(Input.mousePosition); // Ray that represents finger press
            RaycastHit hit; // Object hit by ray

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~_ignoreMe) && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Hit puzzle " + hit.transform.name);

                // NEEDS MORE // assign the puzzlePiece index in this if statement !!!!
                if (hit.collider.gameObject.layer == 13)
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

                if (_selectedPieceScript.CheckLatchOnSpot() && _prefabParticleSuccess != null)
                {
                    Instantiate(_prefabParticleSuccess, SelectedPiece.transform.position, Quaternion.identity);
                }
                
                SelectedPiece = null;
                _selectedPieceScript = null;
            }
        }


        

        if (_clickedPiece == true)
        {
            SelectedPiece.transform.position = CameraPuzzle.ScreenToWorldPoint(Input.mousePosition);
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
