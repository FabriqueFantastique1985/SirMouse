using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class Touch_Move : Touch_Action
{
    public delegate void TouchMoveDelegate(Touch_Move thisTouchMove);
    public event TouchMoveDelegate OnPickup;
    public event TouchMoveDelegate OnDrop;


    [SerializeField]
    protected Animation _animationComponent;

    private bool _acted;

    // values for following mouse  
    private bool _activatedFollowMouse;

    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosXY;
    private Vector3 _mouseWorldPositionXYZ;

    private RaycastHit _hit;


    protected override void Start()
    {
        base.Start();
        this.enabled = false;
    }
    private void Update()
    {
        FollowMouseLogic();
    }


    public override void Act()
    {
        if (_acted == false)
        {
            //base.Act();

            PlayAudio(Pickup);

            _animationComponent.Play(_animPop);
            _animationComponent.PlayQueued(_animIdle);

            _activatedFollowMouse = true;
            _acted = true;
            this.enabled = true;

            OnPickup?.Invoke(this);
        }
    }


    // logic for having object follow the mouse
    private void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {           
            LetGoOfMouse();
        }
        else if (_activatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
    }
    private void LetGoOfMouse()
    {
        PlayAudio(Drop);
        
        _animationComponent.Play(_animPop);

        _activatedFollowMouse = false;
        _acted = false;

        GameManager.Instance.BlockInput = false;

        this.enabled = false;
        OnDrop?.Invoke(this);
    }

    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        transform.position = _mouseWorldPosXY;

        //Debug.Log(_mouseWorldPosXY + " screeworldpos");

        if (_mouseWorldPosXY.y > 0)
        {
            // this only works if I'm moving things above half the screen (create opposite raycast for bottom half screen)
            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
            {
                Debug.DrawRay(transform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);

                _mouseWorldPositionXYZ = _hit.point;

                transform.position = _mouseWorldPositionXYZ;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, -Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
            {
                Debug.DrawRay(transform.position, -Camera.main.transform.forward * _hit.distance, Color.yellow);

                _mouseWorldPositionXYZ = _hit.point;

                transform.position = _mouseWorldPositionXYZ;
            }
        }

    }
}
