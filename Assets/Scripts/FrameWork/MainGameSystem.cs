using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSystem : GameSystem
{
    private int _layerMask;
    public MainGameSystem(Player player, int[] layersToIgnore) : base(player, layersToIgnore)
    {
        for (int i = 0; i < layersToIgnore.Length; i++)
        {
            _layerMask |= (1 << layersToIgnore[i]);
        }
        
        _layerMask = ~_layerMask;
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

                if (hit.collider == GameManager.Instance.PlayField.GroundCollider)
                {
                    _player.SetState(new WalkingState(_player, hit.point));
                }
                else
                {
                    hit.transform.GetComponent<InteractBalloon>()?.Click();
                }
            }
        }
    }
}
