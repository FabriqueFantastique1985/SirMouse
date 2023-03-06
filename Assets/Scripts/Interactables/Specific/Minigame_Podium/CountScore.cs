using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class CountScore : MonoBehaviour
{
    [SerializeField]
    private int _outfitPercentage;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private InteractionStartPodium _startPodium;
    [SerializeField]
    private PodiumController _podiumController;
    [SerializeField]
    private float _sliderSpeed = 1f;
    [SerializeField]
    private Interactable _interactable;

    private int _juryScore;
    private int _poseScore;
    private int _outfitScore;
    public int OutfitScore
    {
        get { return _outfitScore; }
    }

    private void Awake()
    {
        Assert.IsFalse(_sliderSpeed == 0f, "Sliderspeed was 0!");
    }

    private void Start()
    {
        _interactable.OnInteracted += OnPodiumStarted;
        _podiumController.OnPoseTaken += OnPoseTaken;
    }

    private void OnPodiumStarted()
    {
        CountOutfitScore();
        StartCoroutine(DisplayScore(_outfitScore));
        _slider.gameObject.SetActive(true);
    }

    private void OnPoseTaken()
    {
        _poseScore = (int)((float)_podiumController.AmountOfPosesTaken / (float)_podiumController.AmountOfPosesRequired * (100f - (float)_outfitPercentage));
        StartCoroutine(DisplayScore(_poseScore));
    }

    private void CountOutfitScore()
    {
        _outfitScore = SkinsMouseController.Instance.ScoreTotal;
        float outfitAmount = (float)System.Enum.GetValues(typeof(Type_Body)).Length - 1f;
        float maxScoreValue = 10f;
        if (outfitAmount != 0)
        {
            _outfitScore = (int)((float)_outfitScore / (outfitAmount * maxScoreValue) * (float)_outfitPercentage);
        }

        //_juryScore = (int)(_outfitScore / 100f * 3f);
        //_juryScore = Mathf.Max(1, _juryScore);
        //Debug.Log("Juryscore: " + _juryScore);
    }

    private IEnumerator DisplayScore(int scoreAmount)
    {
        while (_slider.value < scoreAmount / 100f)
        {
            yield return new WaitForSeconds(0.01f / _sliderSpeed);
            _slider.value += 0.001f * _sliderSpeed;
        }
    }
}
