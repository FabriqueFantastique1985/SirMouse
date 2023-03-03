using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountScore : MonoBehaviour
{
    [SerializeField]
    private int _outfitPercentage;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private InteractionStartPodium _startPodium;

    private int _outfitScore;
    public int OutfitScore
    {
        get { return _outfitScore; }
    }

    private void Start()
    {
        _startPodium.OnMinigameStarted += OnStartMinigame;
    }

    private void OnStartMinigame()
    {
        CountOutfitScore();
        StartCoroutine(DisplayOutfitScore());
    }

    private void CountOutfitScore()
    {
        _outfitScore = SkinsMouseController.Instance.ScoreTotal;
        int outfitAmount = System.Enum.GetValues(typeof(Type_Body)).Length - 1;
        if (outfitAmount != 0)
        {
            _outfitScore = (int)((float)_outfitScore / (float)(outfitAmount * 10f) * (float)_outfitPercentage);
        }
    }

    private IEnumerator DisplayOutfitScore()
    {
        float score = 0f;
        while (score < _outfitScore / 100f)
        {
            yield return new WaitForSeconds(0.01f);
            score += 0.001f;
            _slider.value = score;
        }
    }
}
