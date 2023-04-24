using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

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
            if (texture)
            {
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                var sprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f), 100, 5);
                renderer.sprite = sprite;
            }
        }
    }

    private IEnumerator MoveShine(SpriteRenderer renderer)
    {
        // Wait until end of frame to not disrupt textures that are being rendered right now
        yield return new WaitForEndOfFrame();
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
        // Does sprite exist
        if (!sprite)
        {
            return null;
        }

        // Is texture sliced or not?
        Rect rect = sprite.rect;
        if (rect.width == sprite.texture.width && rect.height == sprite.texture.height)
        {
            return null;
        }

        // Reference:
        // https://forum.unity.com/threads/c-get-texture-from-sprite-slice-for-animating-mesh-texture.801315/
        
        // Set texture size and parameters
        Texture2D slicedTex = new Texture2D((int)rect.width, (int)rect.height);
        slicedTex.filterMode = sprite.texture.filterMode;
#if UNITY_IOS
        Color[] colors = GetPixelsInRect(sprite.texture, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        slicedTex.SetPixels(0, 0, (int) rect.width, (int) rect.height, colors);
#else
        // Get texture colors from appropriate location on original texture
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

        // Set pixels
        slicedTex.SetPixels(0, 0, (int)rect.width, (int)rect.height, colors);
#endif
        slicedTex.Apply();

        return ScaleTexture(sprite.texture, slicedTex);
    }

    /// <summary>
    /// Fixes the issue where setting a different max size of an image, makes these cut out textures smaller.
    /// Issue stems off of pixel size now being smaller than the original which we read in
    /// </summary>
    private Texture2D ScaleTexture(Texture2D parentTexture, Texture2D slicedTexture)
    {
        //https://forum.unity.com/threads/getting-original-size-of-texture-asset-in-pixels.165295/

        float percentage = 1f;

        // Get the asset path to texture and get its original measurements
        string assetPath = AssetDatabase.GetAssetPath(parentTexture);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        if (importer)
        {
            object[] args = new object[2] { 0, 0 };
            MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(importer, args);

            int originalWidth = (int)args[0];
            percentage = (float)originalWidth / parentTexture.width;
        }

        if (Equals(percentage, 1f))
        {
            return slicedTexture;
        }

        // Scale texture based on original texture size
        Texture2D scaledTex = new Texture2D((int)(slicedTexture.width * percentage), (int)(slicedTexture.height * percentage));
        Graphics.ConvertTexture(slicedTexture, scaledTex);

        return scaledTex;
    }

    /// <summary>
    /// Gets pixels inside given rectangle. Use GetPixels insted if image is not crunched.
    /// </summary>
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
