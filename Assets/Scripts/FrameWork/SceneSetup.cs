using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManagerPrefab;

    [SerializeField]
    private Transform _playerStart;

    [SerializeField]
    private MeshCollider _cameraBounds;

    [SerializeField]
    private bool _shouldZoomOnMove;

    private void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(_gameManagerPrefab);
        }

        // Set the min and max bounds of the follow camera with bounds of the collider
        GameManager.Instance.FollowCamera.SetBounds(_cameraBounds);

        // Set if the camera should zoom out while the player is walking in this scene
        GameManager.Instance.ZoomCamera.ShouldZoomOnMove = _shouldZoomOnMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Player.transform.SetPositionAndRotation(_playerStart.transform.position, _playerStart.transform.rotation);
    }
}
