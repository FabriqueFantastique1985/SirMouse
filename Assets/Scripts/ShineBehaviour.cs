//#define USE_CONVERT_TEXTURE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

public class ShineBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _shineDelayMin;
    [SerializeField] private float _shineDelayMax;
    [SerializeField] private bool _isShineActive;

    public bool IsShineActive { get; set; }

    private void Awake()
    {
        IsShineActive = _isShineActive;
    }

    private void OnEnable()
    {
        if (_spriteRenderer && _isShineActive)
        {
            StartCoroutine(MoveShine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SetParameters()
    {
        // Set scroll time so shine starts out of view
        _spriteRenderer.material.SetFloat("_ScrollTime", -1f);

        // Set texture parameters
        Texture2D texture = GetSlicedSpriteTexture(_spriteRenderer.sprite);
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        _spriteRenderer.sprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f));
    }

    private IEnumerator MoveShine()
    {
        SetParameters();


        // Delay shine at game start
        float shineDelay = Random.Range(_shineDelayMin, _shineDelayMax);
        float showTime = 1f / _spriteRenderer.material.GetFloat("_ScrollSpeed");

        yield return new WaitForSeconds(shineDelay);

        while (_isShineActive)
        {
            float timer = -1f;
            _spriteRenderer.material.SetFloat("_ScrollTime", timer);

            while (IsShineActive && timer < (shineDelay + showTime))
            {
                timer += Time.deltaTime;
                if (timer < showTime)
                {
                    _spriteRenderer.material.SetFloat("_ScrollTime", timer);
                }
                yield return null;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Convert crunched image to normal image with curnch's measurements
    /// </summary>
    /// <param name="crunchedImage"></param>
    /// <returns></returns>
    public Texture2D Convert(Texture2D crunchedImage)
    {
        // Reference:
        // https://answers.unity.com/questions/1907662/how-to-access-crunched-compressed-texture-in-scrip.html
        Texture2D image = new Texture2D(crunchedImage.width, crunchedImage.height);
        image.SetPixels32(crunchedImage.GetPixels32());
        image.Apply();
        Resources.UnloadAsset(crunchedImage);
        return image;
    }

    /// <summary>
    /// Get a normal texture out of a sliced sprite texture
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    Texture2D GetSlicedSpriteTexture(Sprite sprite)
    {
        // Reference:
        // https://forum.unity.com/threads/c-get-texture-from-sprite-slice-for-animating-mesh-texture.801315/
        Rect rect = sprite.rect;
        Texture2D slicedTex = new Texture2D((int)rect.width, (int)rect.height);
        slicedTex.filterMode = sprite.texture.filterMode;

#if USE_CONVERT_TEXTURE
        // Converts the imgage only if it has been crunch compressed
        Texture2D originalTex;
        switch (sprite.texture.format)
        {
            case TextureFormat.DXT1Crunched:
            case TextureFormat.DXT5Crunched:
            case TextureFormat.ETC_RGB4Crunched:
            case TextureFormat.ETC2_RGBA8Crunched:
                originalTex = Convert(sprite.texture);
                break;
            default:
                originalTex = sprite.texture;
                break;
        }
        slicedTex.SetPixels(0, 0, (int)rect.width, (int)rect.height, originalTex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height));
#else
        Color[] colors;
        switch (sprite.texture.format)
        {
            case TextureFormat.DXT1Crunched:
            case TextureFormat.DXT5Crunched:
            case TextureFormat.ETC_RGB4Crunched:
            case TextureFormat.ETC2_RGBA8Crunched:
                colors = GetPixelsInRect(sprite.texture, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
                break;
            default:
                colors = sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
                break;
        }

        slicedTex.SetPixels(0, 0, (int)rect.width, (int)rect.height, colors);
#endif
        slicedTex.Apply();

        return slicedTex;
    }

    /// <summary>
    /// Gets pixels inside given rectangle. Use GetPixels insted if image is not crunched.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private Color[] GetPixelsInRect(Texture2D texture, int x, int y, int width, int height)
    {
        var colors32 = texture.GetPixels32();
        Color[] finalColors = new Color[width * height];

        int idx = 0;
        for (int j = y; j < y + height; ++j)
        {
            for (int k = x; k < x + width; ++k)
            {
                finalColors[idx] = colors32[texture.width * j + k];
                ++idx;
            }
        }

        return finalColors;
    }
}
