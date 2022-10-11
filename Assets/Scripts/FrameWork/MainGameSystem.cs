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

    public MainGameSystem(Player player, int[] layersToIgnore) : base(player, layersToIgnore)
    {
        for (int i = 0; i < layersToIgnore.Length; i++)
        {
            _layerMask |= (1 << layersToIgnore[i]);
        }

        _layerMask = ~_layerMask;

        _groundColliders = GameManager.Instance.PlayField.GroundColliders;
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButton(0))
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

                hit.transform.GetComponent<InteractBalloon>()?.Click(_player);
            }
        }
    }
}