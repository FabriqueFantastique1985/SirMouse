using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Touch_PuzzlePiece : Touch_Action
{
    public delegate void PuzzlePieceDelegate(Touch_PuzzlePiece piece);
    public event PuzzlePieceDelegate OnPiecePickedUp;

    [SerializeField]
    private float _flyCooldown;

    [SerializeField]
    private Vector3 _endPosition;
    private Vector3 _startPosition;

    [SerializeField]
    private float _flySpeed;

    protected override void Start()
    {
        base.Start();

        _touchableScript.HasACooldown = false;
        _touchableScript.OneTimeUse = true;

        _startPosition = transform.position;
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
        Destroy(gameObject);
    }

    private IEnumerator PickupPiece()
    {
        OnPiecePickedUp?.Invoke(this);

        yield return new WaitForSeconds(_flyCooldown);

        StartCoroutine(MoveToTable());
    }
}
