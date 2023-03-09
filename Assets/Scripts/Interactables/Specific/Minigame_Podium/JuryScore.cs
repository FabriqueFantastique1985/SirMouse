﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class JuryScore : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _juryBoardScores = new List<GameObject>();

    [SerializeField]
    private PodiumController _podiumController;
    [SerializeField]
    private CountScore _scoreController;

    [SerializeField]
    private int _starMinimum;
    [SerializeField]
    private int _starMaximum;

    [SerializeField, Tooltip("By how many points do do you want the score on 100 to vary?")]
    private int _randomScoreOffset;

    private int _score;

    private void Start()
    {
        _podiumController.OnMiniGameEnd += OnMiniGameEnded;
    }

    private void OnMiniGameEnded()
    {
        _score = _scoreController.TotalScore;
        DisplayScore();
    }

    // Use animation events for this instead?
    private void DisplayScore()
    {
        for (int i = 0; i < _juryBoardScores.Count; ++i)
        {
            int offset = 0;

            // Only apply offset if score isn't perfect
            if (_score != 100)
            {
                offset = Random.Range(0, _randomScoreOffset);
                offset -= _randomScoreOffset / 2;
            }

            // Calculate how many stars the jury will display
            int score = Mathf.Max((int)((float)(_score + offset) / 100f * _starMaximum));
            score = Mathf.Max(_starMinimum, score);

            if (score - 1  < _juryBoardScores[i].transform.childCount)
            {
                _juryBoardScores[i].transform.GetChild(score - 1).gameObject.SetActive(true);
            }
        }
    }

    public void ResetScore()
    {
        foreach (var juryBoard in _juryBoardScores)
        {
            for (int i = 0; i < juryBoard.transform.childCount; i++)
            {
                juryBoard.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
