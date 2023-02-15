using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Touch_PuzzlePiece : Touch_Action
{
    public delegate void PuzzlePieceDelegate(Touch_PuzzlePiece piece);
    public event PuzzlePieceDelegate OnPiecePickedUp;

    [SerializeField]
    private float _destroyCooldown;

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

    private IEnumerator PickupPiece()
    {
        OnPiecePickedUp?.Invoke(this);

        yield return new WaitForSeconds(_destroyCooldown);

        Destroy(gameObject);
    }
}
