using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSystem : GameSystem
{
    private int _layerMask;
    
    /// <summary>
    /// List of ground colliders used in the current scene.
    /// </summary>
    private Collider[] _groundColliders;

    private bool _onCooldown;
    private float _cooldownTimer;
    private float _cooldownLimit = 0.2f;

    public MainGameSystem(Player player, int[] layersToIgnore, Collider[] newGroundColls = null) : base(player, layersToIgnore)
    {
        for (int i = 0; i < layersToIgnore.Length; i++)
        {
            _layerMask |= (1 << layersToIgnore[i]);
        }

        _layerMask = ~_layerMask;

        if (newGroundColls == null)
        {
            _groundColliders = GameManager.Instance.PlayField.GroundColliders;
        }
        else
        {
            _groundColliders = newGroundColls;
        }
        
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButton(0) && _onCooldown == false)
        {
            Vector3 currentTarget = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(currentTarget);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point);

                // Test if the raycats hit one of the ground colliders
                for (int i = 0; i < _groundColliders.Length; i++)
                {
                    if (hit.collider == _groundColliders[i])
                    {
                        _player.SetState(new WalkingState(_player, hit.point));
                        return;
                    }
                }

                Debug.Log("Hit " + hit.transform.name);

                //hit.transform.GetComponent<InteractBalloon>()?.Click(_player);
                if (hit.transform.TryGetComponent(out InteractBalloon balloon))
                {
                    balloon.Click(_player);

                    // enable short cooldown
                    _onCooldown = true;
                }
            }
        }
    }


    public override void Update()
    {
        base.Update();

        if (_onCooldown == true)
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= _cooldownLimit)
            {
                _onCooldown = false;
                _cooldownTimer = 0;
            }
        }
    }
}