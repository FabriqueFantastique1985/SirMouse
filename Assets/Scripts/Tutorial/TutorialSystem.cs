using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : GameSystem
{
    public delegate void TutorialSystemDelegate(RaycastHit hit, Vector3 mousePos);
    public event TutorialSystemDelegate OnClick;

    public TutorialSystem(Player player, LayerMask layerMask) : base(player, layerMask)
    {
        _layerMask = ~layerMask;
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentTarget = Input.mousePosition;
            Ray ray = Camera.allCameras[0].ScreenPointToRay(currentTarget);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                Debug.DrawLine(Camera.allCameras[0].transform.position, hit.point);

                OnClick?.Invoke(hit, currentTarget);
            }
        }
    }
}
