using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.Playables;

public class CountScore : MonoBehaviour
{
    [SerializeField]
    private int _outfitPercentage;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private PodiumController _podiumController;
    [SerializeField]
    private float _sliderSpeed = 1f;
    [SerializeField]
    private Interactable _interactable;

    [SerializeField]
    private int _maxScoreValue = 10;

    [SerializeField]
    AnimationCurve _curve;
    [SerializeField]
    PlayableDirector _cutscene;

    private int _poseScore;
    private int _outfitScore;
    public int OutfitScore
    {
        get { return _outfitScore; }
    }

    public int TotalScore
    {
        get { return _outfitScore + _poseScore; }
    }

    private void Awake()
    {
        Assert.IsFalse(_sliderSpeed == 0f, "Sliderspeed was 0!");
        SkinPieceElement.MaxScore = _maxScoreValue;
    }

    private void Start()
    {
        _podiumController.OnPoseTaken += OnPoseTaken;
        _podiumController.OnMiniGameEnd += OnMiniGameEnded;

        _slider.gameObject.SetActive(false);
        _slider.value = 0f;
    }

    private void OnPoseTaken()
    {
        _poseScore = (int)((float)_podiumController.AmountOfPosesTaken / (float)_podiumController.AmountOfPosesRequired * (100f - (float)_outfitPercentage));
        StartCoroutine(DisplayScore(_outfitScore + _poseScore));
    }

    private void OnMiniGameEnded()
    {
        _slider.gameObject.SetActive(false);
        StopAllCoroutines();
        _slider.value = 0f;
    }

    public void OnShowSlider()
    {
        CountOutfitScore();
        StartCoroutine(DisplayScore(_outfitScore, 0.6f));
        _slider.gameObject.SetActive(true);
    }

    private void CountOutfitScore()
    {
        _outfitScore = SkinsMouseController.Instance.ScoreTotal;
        float outfitAmount = (float)System.Enum.GetValues(typeof(Type_Body)).Length - 1f;
        if (outfitAmount != 0)
        {
            _outfitScore = (int)((float)_outfitScore / (outfitAmount * _maxScoreValue) * (float)_outfitPercentage);
        }
    }

    private IEnumerator DisplayScore(int scoreAmount, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);

        // Reference:
        // https://answers.unity.com/questions/966356/move-object-and-slow-down-near-end.html
        float target = scoreAmount / 100f;
        while (_slider.value < target)
        {
            float speed = _curve.Evaluate(_slider.value / target);
            _slider.value += speed * target * Time.deltaTime * _sliderSpeed / 2f;
            yield return null;
        }
    }
}
