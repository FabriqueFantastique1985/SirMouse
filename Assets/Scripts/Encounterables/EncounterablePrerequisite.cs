using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterablePrerequisite : Encounterable
{
    [SerializeField]
    private Type_Pickup _pickupTypeNeededToProc = Type_Pickup.None;

    [Header("Balloon Showing Prerequisite")]
    [SerializeField]
    private BalloonNeedy _balloonShowingPrerequisite;

    protected override void Start()
    {
        base.Start();

        _balloonShowingPrerequisite.Hide();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (_pickupTypeNeededToProc != Type_Pickup.None && GameManager.Instance.Player.EquippedPickupType == _pickupTypeNeededToProc
         || _pickupTypeNeededToProc == Type_Pickup.None)
        {
            // use layers so it only detects player entering
            if (_oneTimeUse == false || _oneTimeUse == true && _usedSuccesfully == false)
            {
                // check for cooldown
                if (_onCooldown == false)
                {
                    GenericBehaviour();

                    // if cooldown is present
                    if (_hasACooldown == true)
                    {
                        StartCoroutine(ActivateCooldown());
                    }
                }
            }
        }
        else
        {
            // show the balloon
            if (_oneTimeUse == false || _oneTimeUse == true && _usedSuccesfully == false)
            {
                _balloonShowingPrerequisite.Show();
            }          
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        _balloonShowingPrerequisite.Hide();
    }


}
