using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ID))]
public class Touch_PuzzlePiece : Touch_Action, IDataPersistence
{
    public delegate void PuzzlePieceDelegate(Touch_PuzzlePiece piece);
    public event PuzzlePieceDelegate OnPiecePickedUp;
    public event PuzzlePieceDelegate OnPieceClicked;

    [SerializeField] private float _flyCooldown;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private float _flySpeed;

    [SerializeField] private ID _id;

    private bool _isGathered = false;

    public Vector3 TargetDestination
    {
        set { _endPosition = value; }
    }

    private void Awake()
    {
        if (_id == null)
        {
            _id = GetComponent<ID>();
        }
    }

    protected override void Start()
    {
        base.Start();

        _touchableScript.HasACooldown = false;
        _touchableScript.OneTimeUse = true;
    }

    public override void Act()
    {
        base.Act();

        StartCoroutine(PickupPiece());
    }

    private IEnumerator MoveToTable()
    {
        // Code from Unity documentation

        while (Vector3.Distance(transform.position, _endPosition) > 0.001f)
        {
            // Move our position a step closer to the target.
            var step = _flySpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _endPosition, step);

            yield return null;
        }
        OnPiecePickedUp?.Invoke(this);
        _isGathered = true;
    }

    private IEnumerator PickupPiece()
    {
        OnPieceClicked?.Invoke(this);
 
        GetComponent<ShineBehaviour>().IsShineActive = false;

        yield return new WaitForSeconds(_flyCooldown);

        _touchableScript.Animator.SetTrigger("Activate");

        StartCoroutine(MoveToTable());
    }

    private IEnumerator SetPieceToEnd()
    {
        // Wait for next frame so listeners can be added to appropriate scripts
        yield return null;
        OnPieceClicked?.Invoke(this);
        GetComponent<ShineBehaviour>().IsShineActive = false;
        transform.position = _endPosition;
        OnPiecePickedUp?.Invoke(this);
        _isGathered = true;
    }

    public void LoadData(GameData data)
    {
        if (_id != null && data.GatherablePuzzlePieces.ContainsKey(_id))
        {
            _isGathered = data.GatherablePuzzlePieces[_id];
            if (_isGathered)
            {
                StartCoroutine(SetPieceToEnd());
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (_id == null || string.IsNullOrEmpty(_id.IDName))
        {
            Debug.LogWarning("No id yet made! Please generate one!");
            return;
        }

        data.GatherablePuzzlePieces[_id] = _isGathered;
    }
}
