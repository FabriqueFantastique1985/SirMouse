using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

public class ShineBehaviour : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    [SerializeField] private float _shineDelayMin = 2f;
    [SerializeField] private float _shineDelayMax = 4f;
    [SerializeField] private bool _isShineActive = true;

    private float _shineDelay;

    public bool IsShineActive { get; set; }

    private void Awake()
    {
        IsShineActive = _isShineActive;
        _shineDelay = Random.Range(_shineDelayMin, _shineDelayMax);
        foreach (var renderer in _spriteRenderers)
        {
            if (renderer.flipX)
            {
                renderer.material.SetInt("_IsFlippedX", 1);
            }
            if (renderer.flipY)
            {
                renderer.material.SetInt("_IsFlippedY", 1);
            }
        }
    }

    private void OnEnable()
    {
        if (_isShineActive)
        {
            foreach (var renderer in _spriteRenderers)
            {
                StartCoroutine(MoveShine(renderer));
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SetParameters()
    {
        foreach (var renderer in _spriteRenderers)
        {
            // Set scroll time so shine starts out of view
            renderer.material.SetFloat("_ScrollTime", -1f);

            // Set texture parameters
            Texture2D texture = GetSlicedSpriteTexture(renderer.sprite);
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f), 100, 5);
            renderer.sprite = sprite;
        }
    }

    private IEnumerator MoveShine(SpriteRenderer renderer)
    {
        SetParameters();

        // Delay shine at game start
        float scrollSpeed = renderer.material.GetFloat("_ScrollSpeed");
        Assert.IsFalse(scrollSpeed == 0, "Shine material was not correctly set in object:" + gameObject.name);
        float showTime = 1f / scrollSpeed;

        yield return new WaitForSeconds(_shineDelay);

        while (_isShineActive)
        {
            float timer = -1f;
            renderer.material.SetFloat("_ScrollTime", timer);

            while (IsShineActive && timer < (_shineDelay + showTime))
            {
                timer += Time.deltaTime;
                if (timer < showTime)
                {
                    renderer.material.SetFloat("_ScrollTime", timer);
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
#if UNITY_IOS
        Color[] colors = GetPixelsInRect(sprite.texture, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        slicedTex.SetPixels(0, 0, (int) rect.width, (int) rect.height, colors);
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
