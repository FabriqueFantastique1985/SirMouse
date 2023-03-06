using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuryScore : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _juryBoardScores = new List<GameObject>();

    [SerializeField]
    private PodiumController _podiumController;
    [SerializeField]
    private CountScore _scoreController;

    [SerializeField]
    private float _scoreDisplayDelay = 1f;

    [SerializeField]
    private int _starMinimum;
    [SerializeField]
    private int _starMaximum;

    private int _score;

    private void Start()
    {
        _podiumController.OnMiniGameEnded += OnMiniGameEnded;
    }

    private void OnMiniGameEnded()
    {
        _score = _scoreController.TotalScore;


        _score = Mathf.Max((int)((float)(_score) / 100f * _starMaximum));
        _score = Mathf.Max(_starMinimum, _score);
        Debug.Log("Juryscore: " + _score);

        StartCoroutine(DisplayScore());
    }

    // Use animation events for this instead?
    private IEnumerator DisplayScore()
    {
        for (int i = 0; i < _juryBoardScores.Count; ++i)
        {
            yield return new WaitForSeconds(_scoreDisplayDelay);
            if (_score - 1  < _juryBoardScores[i].transform.childCount)
            {
                _juryBoardScores[i].transform.GetChild(_score - 1).gameObject.SetActive(true);
            }
        }
    }
}
