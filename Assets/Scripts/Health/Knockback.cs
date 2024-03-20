using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace RogueApeStudio.Crusader.HealthSystem.Knockback
{
    public class Knockback : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _wait = false;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rb;

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void AddKnockback(float force, Vector3 direction)
        {
            _rb.isKinematic = false;
            _agent.enabled = false;
            _wait = true;
            Physics.IgnoreLayerCollision(10, 10, true);

            direction.y = 0;

            _rb.AddForce(direction * force, ForceMode.Impulse);

            //play stun animation
            DelayAsync(_cancellationTokenSource.Token);
        }

        private async void DelayAsync(CancellationToken token)
        {
            try
            {
                await UniTask.WaitForSeconds(1, cancellationToken: token);
                _rb.isKinematic = true;
                _agent.enabled = true;
                Physics.IgnoreLayerCollision(10, 10, false);
            }
            catch (OperationCanceledException)
            {

                Debug.LogError("Delay was Canceled because operation was cancelled");
            }
        }
    }
}
