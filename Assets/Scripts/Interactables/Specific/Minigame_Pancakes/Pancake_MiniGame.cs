using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancake_MiniGame : MiniGame
{
    [SerializeField]
    private List<Transform> _spawnLocations = new List<Transform>();

    [SerializeField]
    private float _spawnDelay;

    [SerializeField]
    private float _spawnAmount;

    private IEnumerator SpawnPancakes()
    {
        for (int i = 0; i < _spawnAmount; ++i)
        {
            yield return new WaitForSeconds(_spawnDelay);

            // pick random lane
            // spawn pancake in lane

            //Instantiate()
        }
    }
}
