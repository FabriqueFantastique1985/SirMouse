using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour, IClickable
{
    #region EditorFields

    [SerializeField]
    private NavMeshAgent _agent;

    [SerializeField]
    private Character _character;

    [SerializeField]
    private CharacterRigReferences _characterRigReferences;
    [SerializeField]
    private CharacterGeoReferences _characterGeoReferences;

    [SerializeField] private List<Material> _outfitMaterials = new List<Material>();
    [SerializeField] private float _explosionCountdownDuration = 5f;
    [SerializeField] private float _explosionStayDuration = 5f;
    #endregion

    #region Properties

    public SirMouseState State => _stateStack?.Peek();
    public Character Character => _character;
    public NavMeshAgent Agent => _agent;
    public Interactable EquippedItem => _equippedItem;
    public Type_Pickup EquippedPickupType => _equippedPickupType;
    
    #endregion

    #region Fields

    private Stack<SirMouseState> _stateStack = new Stack<SirMouseState>();
    private Interactable _equippedItem;
    private Type_Pickup _equippedPickupType;
    [SerializeField]
    private float _explosionCooldown;
    private bool _canExplode;
    private float _explosionTickCount, _explosionTimer, _degradeTickTimer;
    private float _explosionTickGoal = 5;
    private float _degradeTickGoal = 2;

    #endregion


    //private void Awake()
    //{
    //    Initialize();
    //}

    private void Start()
    {
        Initialize();

        foreach (var material in _outfitMaterials)
        {
            material.SetFloat("_GrungeOpacity", 0f);
        }
    }

    public void Initialize()
    {
        _stateStack.Push(new IdleState(this));
        
        //// reset material texture
        //_characterGeoReferences.SirMouseBody.mainTexture = _characterGeoReferences.DefaultTex; // dated closet logic
        //_characterGeoReferences.SirMouseHands.mainTexture = _characterGeoReferences.HandsLight;
    }
    
    public void SetTarget(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    private void Update()
    {
        var currentState = _stateStack?.Peek();

        if (currentState != null) currentState.Update(this);



        if (_canExplode == false)
        {
            _explosionTimer += Time.deltaTime;

            if (_explosionTimer >= _explosionCooldown)
            {
                _explosionTimer = 0;               
                _canExplode = true;
            }
        }
        if (_explosionTickCount > 0)
        {
            _degradeTickTimer += Time.deltaTime;

            if (_degradeTickTimer >= _degradeTickGoal)
            {
                // degrade the redhead and tickCount
                DegradeRedHeadAndTickCount();
                _degradeTickTimer = 0;
            }            
        }
    }

    public void SetState(SirMouseState newState)
    {
        var currentState = _stateStack?.Pop();
        if (newState == currentState) return;
        currentState?.OnExit(this);
        
        _stateStack.Push(newState);
        newState.OnEnter(this);
    }

    public void PushState(SirMouseState newState)
    {
        // Exit current state
        var currentState = _stateStack.Peek();
        currentState?.OnExit(this);

        _stateStack.Push(newState);
        newState.OnEnter(this);
    }

    public void PopState()
    {
        var currentState = _stateStack.Pop();
        currentState?.OnExit(this);
    }

    public void Equip(Interactable itemToEquip, bool outOfBackpack = false)
    {
        _equippedItem = itemToEquip;
        _equippedPickupType = itemToEquip.MyPickupType;

        _equippedItem.gameObject.SetActive(true);
        _equippedItem.transform.parent = _characterRigReferences.HandRightTransform;
        _equippedItem.transform.localPosition = Vector3.zero;

        //_equippedItem.transform.localRotation = Quaternion.identity;
        if (outOfBackpack == true)
        {
            // fix rotation of interactable
            itemToEquip.transform.localEulerAngles = new Vector3(60, 117.618f, 0);
        }

    }

    public void Drop()
    {
        if (_equippedItem != null)
        {
            _equippedItem.transform.parent = null;
            var playerPos = transform.position;

            _equippedItem.transform.position = new Vector3(playerPos.x, 0.0f, playerPos.z);
            _equippedItem.transform.localScale = new Vector3(1, 1, 1); // always the same orientation

            // activate collider again
            _equippedItem.GetComponent<Collider>().enabled = true;
            _equippedItem.InteractionBalloon.BalloonTrigger.enabled = true;
        }

        _equippedItem = null;
        _equippedPickupType = Type_Pickup.None;
    }

    public void Click(Player player)
    {
        if (_equippedItem == null)
        {
            if (State.GetType() != typeof(IdleState))
            {
                return;
            }
            else
            {
                if (_canExplode == true)
                {
                    IncreaseExplosionTickCount();
                }          
                return;
            }           
        }

        SetState(new DropState(this));
    }


    private void IncreaseExplosionTickCount()
    {
        // increase tick count
        _explosionTickCount += 1;
        // reset timer on degrader
        _degradeTickTimer = 0;
        // update redHead color 
        IncreaseRedHead();

        // if goal is reached...
        if (_explosionTickCount >= _explosionTickGoal)
        {
            ExplodeSirMouse();
            return;
        }

        // if certain threshholds are reach => play sound grunt
    }

    private void DegradeRedHeadAndTickCount()
    {
        switch (_explosionTickCount)
        {
            case 4:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.7f);
                _explosionTickCount -= 1;
                break;
            case 3:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.4f);
                _explosionTickCount -= 1;
                break;
            case 2:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.2f);
                _explosionTickCount -= 1;
                break;
            case 1:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0);
                _explosionTickCount -= 1;
                break;
        }
    }
    private void IncreaseRedHead()
    {
        switch (_explosionTickCount)
        {
            case 4:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.9f);
                break;
            case 3:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.6f);
                break;
            case 2:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.4f);
                break;
            case 1:
                _characterGeoReferences.RedHeadOverlay.color = new Color(255, 255, 255, 0.2f);
                break;
        }
    }

    public void ExplodeSirMouse()
    {
        // => play explosion, set new material, set cooldown, reset redhead alpha, return;
        Character.PlayExplosion();
        _characterGeoReferences.RedHeadOverlay.color = new Color(255,255,255,0);       
        _canExplode = false;

        StartCoroutine(ExplosionCountdown());
    }

    private IEnumerator ExplosionCountdown()
    {
        float time = 0f;
        float value = 1f;

        // Reset grunge opacity to 1
        foreach (var material in _outfitMaterials)
        {
            material.SetFloat("_GrungeOpacity", value);
        }

        yield return new WaitForSeconds(_explosionStayDuration);

        // Slide grunge opacity from 1 to 0 based off of countdown duration
        while (time < _explosionCountdownDuration)
        {
            time += Time.deltaTime;
            value = Mathf.Max(1f - time / _explosionCountdownDuration, 0f);
            foreach (var material in _outfitMaterials)
            {
                material.SetFloat("_GrungeOpacity", value);
            }
            yield return null;
        }

        _explosionTickCount = 0;
    }
}