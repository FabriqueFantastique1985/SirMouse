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
        _slider.value = 0f;
    }

    public void OnShowSlider()
    {
        CountOutfitScore();
        StartCoroutine(DisplayScore(_outfitScore));
        _slider.gameObject.SetActive(true);
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
    }

    private IEnumerator DisplayScore(int scoreAmount)
    {
        //while (_slider.value < scoreAmount / 100f)
        //{
        //    yield return new WaitForSeconds(0.01f / _sliderSpeed);
        //    _slider.value += 0.001f * _sliderSpeed;
        //}

        // https://answers.unity.com/questions/1817160/how-to-lerp-health-slider.html
        //float displayTime = 5f;
        //float timeScale = 0f;
        //while (timeScale < 1f)
        //{
        //    timeScale += Time.deltaTime / displayTime;
        //    _slider.value = Mathf.Lerp(_slider.value, scoreAmount / 100f, timeScale);
        //    yield return null;
        //}

        // Reference:
        // https://answers.unity.com/questions/966356/move-object-and-slow-down-near-end.html
        float target = scoreAmount / 100f;
        //PlayableDirector obj;
        //obj.duration;
        while (_slider.value < target)
        {
            float speed = _curve.Evaluate(_slider.value / target);
            _slider.value += speed * target * Time.deltaTime;
            yield return null;
        }
    }
}
