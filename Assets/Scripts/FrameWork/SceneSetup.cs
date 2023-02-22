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
    private BoxCollider _cameraBounds;
    
    private void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(_gameManagerPrefab);
        }

        // Set the min and max bounds of the follow camera with bounds of the collider
        GameManager.Instance.FollowCamera.SetBounds(_cameraBounds);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Player.transform.SetPositionAndRotation(_playerStart.transform.position, _playerStart.transform.rotation);
    }
}
