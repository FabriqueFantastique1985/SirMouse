using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.UIElements.UxmlAttributeDescription;


/// <summary>
///  Interactable needy VS Interactable
///                    ----
/// </summary>
public class InteractableNeedy : MonoBehaviour, IDataPersistence
{
    #region Events

    public delegate void InteractableDelegate();

    public event InteractableDelegate OnInteracted;

    #endregion

    [SerializeField]
    private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    // Balloon used to execute an interaction.
    [Header("Balloon components")]
    public Balloon InteractionBalloon;

    // Balloon used to show what items I want.
    public BalloonNeedy NeedyBalloon;

    private bool _gotAllPrerequisites;

    // List of possible interactions with this interactable  ==> this case 1 interaction for donating the said items
    [SerializeField]
    private List<Interaction> _interactions = new List<Interaction>();

    protected int _currentInteractionIndex = 0;

    [Header("Shine Properties")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _shineDelayMin;
    [SerializeField]
    private float _shineDelayMax;
    [SerializeField]
    private float _shineScale = 1;
    [SerializeField]
    private bool _isShineActive;

    #region Properties

    /// <summary>
    /// Reference to the list of possible Interactions of this Interactable.
    /// </summary>
    public List<Interaction> Interactions
    {
        get => _interactions;
    }

    #endregion

    private void Start()
    {
        if (InteractionBalloon != null)
        {
            InteractionBalloon.OnBalloonClicked += OnInteractBalloonClicked;
            InteractionBalloon.gameObject.SetActive(false);
        }

        if (_spriteRenderer)
        {
            StartCoroutine(MoveShine());
        }

        Initialize();
    }

    #region Virtual Functions

    protected virtual void Initialize()
    {
        if (InteractionBalloon != null)
        {
            InteractionBalloon.SetSprite(_interactions[0].SpriteObjectInteractionBalloon);
        }

        if (NeedyBalloon != null)
        {
            NeedyBalloon.gameObject.SetActive(false);
        }
    }
    protected virtual void OnInteractBalloonClicked(Balloon sender, Player player)
    {
        // Execute current interaction
        if (_currentInteractionIndex < 0 || _interactions.Count <= 0)
        {
            Debug.LogError("Tried to execute an interaction that either did not exist or wasn't setup correctly!");
            return;
        }
        _interactions[_currentInteractionIndex].Execute(player);

        OnInteracted?.Invoke();
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);
    }


    // these get overriden in inheriting classes
    protected virtual void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            // if I have all the required objects (bool GotAllPrerequisites)
            if (_gotAllPrerequisites == true)
            {

            }

            // show interactBalloon 
            ShowInteractionBalloon();
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            HideInteractionBalloon();
        }
    }


    #endregion

    #region Private Functions

    private IEnumerator MoveShine()
    {
        // Set scroll time so shine starts out of view
        _spriteRenderer.material.SetFloat("_ScrollTime", -1f);

        float scrollSpeed = _spriteRenderer.material.GetFloat("_ScrollSpeed");
        float showTime = 1f / scrollSpeed;

        Assert.AreNotEqual(_shineScale, 0f);
        _spriteRenderer.material.SetFloat("_Scale", _shineScale);

        // Delay shine at game start
        float shineDelay = UnityEngine.Random.Range(_shineDelayMin, _shineDelayMax);
        yield return new WaitForSeconds(shineDelay);

        while (_isShineActive)
        {
            float timer = -1f;
            while (timer < shineDelay + showTime)
            {
                timer += Time.deltaTime;

                if (timer < showTime)
                {
                    _spriteRenderer.material.SetFloat("_ScrollTime", timer);
                }

                yield return null;
            }
        }
    }



    protected void ShowInteractionBalloon()
    {
        // No balloon required when there are no interactions
        if (_interactions.Count <= 0) return;

        InteractionBalloon.Show();
    }
    protected void ShowNeedyBalloon()
    {
        NeedyBalloon.Show();
    }
    protected void HideInteractionBalloon()
    {
        // Nothing to hide if there are no interactions to begin with
        if (_interactions.Count <= 0) return;

        InteractionBalloon.Hide();
    }
    protected void HideNeedyBalloon()
    {
        NeedyBalloon.Hide();
    }

    #endregion

    #region Public Functions

    public virtual void HideBalloonBackpack()
    {
        HideInteractionBalloon();
    }

    #endregion



    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {
    }
}
