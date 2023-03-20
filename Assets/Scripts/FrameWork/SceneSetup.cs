using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManagerPrefab;

    public List<InteractionLevelChange> PlayerSpawns = new List<InteractionLevelChange>();

    [Header ("Camera parameters")]
    [SerializeField]
    private MeshCollider _cameraBounds;

    [SerializeField]
    private bool _shouldZoomOnMove = false;

    [SerializeField]
    private float _orthographicSize = 5f;

    private void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(_gameManagerPrefab);
        }

        GameManager.Instance.CurrentSceneSetup = this;

        // Set the min and max bounds of the follow camera with bounds of the collider
        GameManager.Instance.FollowCamera.SetBounds(_cameraBounds);

        // Set if the camera should zoom out while the player is walking in this scene
        GameManager.Instance.ZoomCamera.ShouldZoomOnMove = _shouldZoomOnMove;
        GameManager.Instance.ZoomCamera.OrthographicSize = _orthographicSize;
    }



    //// Start is called before the first frame update
    //void Start()
    //{
    //    GameManager.Instance.Player.transform.SetPositionAndRotation(_playerStart.transform.position, _playerStart.transform.rotation);
    //}
}
