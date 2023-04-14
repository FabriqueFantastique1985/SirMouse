using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstrumentKey_Gatherable : InteractionInstrument
{
    [Header("Chest Object")]
    [SerializeField]
    private GameObject _chestToOpen;
    [SerializeField]
    private GameObject _particlePoofPrefab;

    [SerializeField]
    private List<GameObject> _objectsToSpawn = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < _objectsToSpawn.Count; i++)
        {
            _objectsToSpawn[i].SetActive(false);
        }
    }

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // play animation
        //GameManager.Instance.Player.Character.AnimatorRM.SetTrigger("Swing");

        GameManager.Instance.Player.Agent.SetDestination(GameManager.Instance.Player.gameObject.transform.position);
        GameManager.Instance.BlockInput = true;

        StartCoroutine(SpawnObjectAndEnableInput());
    }

    public override void HideInteraction()
    {
        base.HideInteraction();

        _chestToOpen.SetActive(false);
    }

    private IEnumerator SpawnObjectAndEnableInput()
    {
        //float currentStateLength = GameManager.Instance.Player.Character.AnimatorRM.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSeconds(currentStateLength);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.BlockInput = false;

        for (int i = 0; i < _objectsToSpawn.Count; i++)
        {
            _objectsToSpawn[i].SetActive(true);
        }

        Instantiate(_particlePoofPrefab, _chestToOpen.transform.position, Quaternion.identity);
        HideInteraction();
    }
}
