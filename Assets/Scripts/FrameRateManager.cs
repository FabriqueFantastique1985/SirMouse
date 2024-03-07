using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FrameRateManager : MonoBehaviour
{
    public int targetFrameRate = 25;
    [SerializeField]
    private Text _frameRate;
    private float _deltaTime = 0.0f;

    void Start()
    {
        // Disabling VSync by setting it to 0. 
        // This allows Application.targetFrameRate to take effect.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }

    // Optionally, to ensure it keeps applying in the Editor after script recompiles or changes
    void Update()
    {
        if (Application.targetFrameRate != targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }

        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        if (_frameRate)
        {
            _frameRate.text = (1.0f / _deltaTime).ToString();
        }
    }
}