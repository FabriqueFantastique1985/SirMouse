using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIngredient : MonoBehaviour
{
    // PUBLIC
    [HideInInspector]
    public bool cookingGameActive = false;

    // PRIVATE
    private GameObject _IngredientClone;
    private bool _pickedUp = false;
    private Ray _ray;
    private float _offset;

    [SerializeField]
    private LayerMask _ignoreMe;

    private void Start()
    {
        this.enabled = false;
    }

    private void Update()
    {
        // calculate ray as long as i hold down
        if (Input.GetMouseButton(0))
        {
            CalculateRay(Input.mousePosition);
        }

        // the frame i click
        if (Input.GetMouseButtonDown(0))
        {
            SingleFinger(Input.mousePosition);
        }

        // the frame i let go
        if (Input.GetMouseButtonUp(0))
        {
            if (_pickedUp && !cookingGameActive)
            {
                //playerControls.walkingEnabled = true;
                //GameManager.Instance.BlockInput = false;
            }

            _pickedUp = false;

            if (_IngredientClone)
            {
                _IngredientClone.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                _IngredientClone.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            }

            _IngredientClone = null;
        }

        // if i have a veggie in my grasp
        if (_pickedUp)
        {
            _offset = Mathf.Clamp((_ray.origin.y - 7) / 2, -1, 2.7f);
            _IngredientClone.transform.position = _ray.origin + _IngredientClone.transform.GetChild(0).forward * (12f + _offset);
        }
    }


    private void CalculateRay(Vector2 position)
    {
        _ray = Camera.main.ScreenPointToRay(position);
    }



    private void SingleFinger(Vector2 position)
    {
        if (_IngredientClone)
        {
            _IngredientClone.transform.position = _ray.origin;
        }

        RaycastHit hit; // Object hit by ray

        if (Physics.Raycast(_ray, out hit, Mathf.Infinity, ~_ignoreMe))
        {
            // if i click an ingredient bag
            if (hit.collider.GetComponent<Ingredient>()) // hitting ground (make it ignore ground)
            {
                if (!cookingGameActive)
                {
                    //playerControls.walkingEnabled = false;
                    GameManager.Instance.BlockInput = true;
                }
                _IngredientClone = Instantiate(hit.collider.GetComponent<Ingredient>().ingredientPrefab, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                _pickedUp = true;
            }
        }
    }
}