using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelClothing : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _childrenObjects = new List<GameObject>();

    private float delay;

    private void OnEnable()
    {
        delay = 1f;
        SkinControllerMainMenu.Instance.StartCoroutine(ShowSkins1By1());
    }


    private IEnumerator ShowSkins1By1()
    {
        while (_childrenObjects.Count > 0)
        {
            int random = Random.Range(0, _childrenObjects.Count);
            GameObject randomChild = _childrenObjects[random];

            randomChild.SetActive(true);

            _childrenObjects.Remove(randomChild);

            yield return new WaitForSeconds(delay);

            delay -= 0.07f;
        }
        
    }
}
