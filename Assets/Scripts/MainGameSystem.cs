using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSystem : GameSystem
{
    public MainGameSystem(Player player) : base(player)
    {
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentTarget = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(currentTarget);
            RaycastHit hit;

            int layerMask = ~_player.gameObject.layer;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point);

                if (hit.collider == GameManager.Instance.PlayField.GroundCollider)
                {
                    _player.SetState(new WalkingState(_player, hit.point));
                }
                else
                {
                    hit.transform.GetComponent<InteractBalloon>()?.ExecuteInteraction();
                }
            }
        }
    }
}
