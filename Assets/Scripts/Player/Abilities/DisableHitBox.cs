using Cysharp.Threading.Tasks;
using RogueApeStudio.Crusader.Player.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DisableHitBox : MonoBehaviour
{
    private CancellationTokenSource _cancellationTokenSource;

    [SerializeField] private float _timeToDestroy;
    [SerializeField] private Collider _collider;

    void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    void Start()
    {
        DelayAsync(_cancellationTokenSource.Token);
    }

    private async void DelayAsync(CancellationToken token)
    {
        try
        {
            await UniTask.WaitForSeconds(_timeToDestroy, cancellationToken: token);
            _collider.enabled = false;
        }
        catch (OperationCanceledException)
        {

        }

    }
}
