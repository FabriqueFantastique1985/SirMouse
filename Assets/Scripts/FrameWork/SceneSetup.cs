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
    
    private void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            Instantiate(_gameManagerPrefab);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Player.transform.SetPositionAndRotation(_playerStart.transform.position, _playerStart.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
