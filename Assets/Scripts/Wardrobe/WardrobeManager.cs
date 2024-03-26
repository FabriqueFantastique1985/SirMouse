using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WardrobeManager : MonoBehaviour
{
    [Header("Category buttons")]
    [SerializeField]
    private Button _swordCategoryButton;
    [SerializeField]
    private Button _shieldCategoryButton;
    [SerializeField]
    private Button _tailCategoryButton;
    [SerializeField]
    private Button _footCategoryButton;
    [SerializeField]
    private Button _pantsCategoryButton;
    [SerializeField]
    private Button _armCategoryButton;
    [SerializeField]
    private Button _chestCategoryButton;
    [SerializeField]
    private Button _faceCategoryButton;
    [SerializeField]
    private Button _hatCategoryButton;

    [Header("Skin piece buttons")]
    [SerializeField]
    private List<GameObject> _swordButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _shieldButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _tailButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _footButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _pantsButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _armButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _chestButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _faceButtons = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _hatButtons = new List<GameObject>();

    [SerializeField]
    private ScrollRect _scrollView;
    [SerializeField]
    private RectTransform _content;

    private void Awake()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        _swordCategoryButton.onClick.AddListener(SwordCategoryButtonPressed);
        _shieldCategoryButton.onClick.AddListener(ShieldCategoryButtonPressed);
        _tailCategoryButton.onClick.AddListener(TailCategoryButtonPressed);
        _footCategoryButton.onClick.AddListener(FootCategoryButtonPressed);
        _pantsCategoryButton.onClick.AddListener(PantsCategoryButtonPressed);
        _armCategoryButton.onClick.AddListener(ArmsCategoryButtonPressed);
        _chestCategoryButton.onClick.AddListener(ChestCategoryButtonPressed);
        _faceCategoryButton.onClick.AddListener(FaceCategoryButtonPressed);
        _hatCategoryButton.onClick.AddListener(HatCategoryButtonPressed);
        SwordCategoryButtonPressed();
    }

    private void ActivateButtons(List<GameObject> buttons, bool activate = true)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(activate);
        }
    }

    private void SwordCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);
        
        SetScrollField(2100);
    }

    private void ShieldCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);

        SetScrollField(0);
    }

    private void TailCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);

        SetScrollField(0);
    }

    private void FootCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);

        SetScrollField(1200);
    }

    private void PantsCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);
        
        SetScrollField(1400);
    }

    private void ArmsCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);
        
        SetScrollField(1800);
    }

    private void ChestCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons, false);
        
        SetScrollField(1000);
    }

    private void FaceCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons);
        ActivateButtons(_hatButtons, false);
        
        SetScrollField(0);
    }

    private void HatCategoryButtonPressed()
    {
        ActivateButtons(_swordButtons, false);
        ActivateButtons(_shieldButtons, false);
        ActivateButtons(_tailButtons, false);
        ActivateButtons(_footButtons, false);
        ActivateButtons(_pantsButtons, false);
        ActivateButtons(_armButtons, false);
        ActivateButtons(_chestButtons, false);
        ActivateButtons(_faceButtons, false);
        ActivateButtons(_hatButtons);
        
        SetScrollField(1900);
    }

    private void SetScrollField(float size)
    {
        _scrollView.vertical = size > 0;
        _content.sizeDelta = new Vector2(-14.45f, size);
        _content.localPosition = new Vector2(14.45f, 0);
    }
}