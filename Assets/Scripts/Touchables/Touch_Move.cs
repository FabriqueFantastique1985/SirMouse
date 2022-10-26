using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class Touch_Move : Touch_Action
{
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
            base.Act();

            AudioController.Instance.PlayAudio(AudioElements[0]);

            _animationComponent.Play(_animPop);
            _animationComponent.PlayQueued(_animIdle);

            _activatedFollowMouse = true;
            _acted = true;
            this.enabled = true;
        }
    }


    // logic for having object follow the mouse
    private void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AudioController.Instance.PlayAudio(AudioElements[1]);
            LetGoOfMouse();
        }
        else if (_activatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
    }
    private void LetGoOfMouse()
    {
        _animationComponent.Play(_animPop);

        _activatedFollowMouse = false;
        _acted = false;
        this.enabled = false;
    }

    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        transform.position = _mouseWorldPosXY;

        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
        {
            //Debug.DrawRay(transform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);
            _mouseWorldPositionXYZ = _hit.point;
            transform.position = _mouseWorldPositionXYZ;
        }
    }
}
