using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{

    [SerializeField]
    private Transform _rootObject;

    [SerializeField]
    private float _fadeTime = .3f;
    [SerializeField]
    private float _fadeAmount = .2f;

    private bool _isFading = false;
    private float _timer = 0;
    private float[] _goalAlphas;
    private float[] _startAlphaValues;
    private float[] _initialAlphaValues;
    private SpriteRenderer[] _spriteRenderers;
    private int _colliderCounter;

    private void Awake()
    {
        _spriteRenderers = _rootObject.GetComponentsInChildren<SpriteRenderer>();

        _goalAlphas = new float[_spriteRenderers.Length];
        _initialAlphaValues = new float[_spriteRenderers.Length];
        _startAlphaValues = new float[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _initialAlphaValues[i] = _spriteRenderers[i].color.a;
        }
    }

    private void Update()
    {
        if (_isFading)
        {
            Fade();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameManager.Instance.Player.gameObject)
        {
            _colliderCounter++;
            StartFade(_fadeAmount);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == GameManager.Instance.Player.gameObject)
        {
            _colliderCounter--;
            if (_colliderCounter == 0)
            {
                StartFade(_initialAlphaValues);
            }
        }
    }

    private void StartFade(float goalAlpha)
    {
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _goalAlphas[i] = goalAlpha;
            _startAlphaValues[i] = _spriteRenderers[i].color.a;
        }
        _isFading = true;
        _timer = 0;
    }
   
    private void StartFade(float[] goalAlphas)
    {
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _goalAlphas[i] = goalAlphas[i];
            _startAlphaValues[i] = _spriteRenderers[i].color.a;
        }
        _isFading = true;
        _timer = 0;
    }

    private void Fade()
    {
        Color[] colors = new Color[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            colors[i] = _spriteRenderers[i].color;
        }

        if (_timer < _fadeTime)
        {

            _timer += Time.deltaTime;

            float normalizedTime = _timer / _fadeTime;

            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                colors[i].a = Mathf.Lerp(_startAlphaValues[i], _goalAlphas[i], normalizedTime);
            }
        }
        else
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                colors[i].a = _goalAlphas[i];
            }
            _isFading = false;
        }

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = colors[i];
        }
    }


}