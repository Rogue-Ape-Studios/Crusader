using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using TMPro;

namespace RogueApeStudio.Crusader.UI.Cooldown
{
    public class AbilityCooldown : MonoBehaviour
    {
        [SerializeField] private GameObject _icon;
        [SerializeField] private GameObject _emptyIcon;
        [SerializeField] private TextMeshProUGUI _counter;

        private CancellationTokenSource _cancellationTokenSource;
        private int _currentCooldown;

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void StartCooldown(int cooldown)
        {
            _currentCooldown = cooldown;
            _icon.SetActive(false);
            _emptyIcon.SetActive(true);
            _counter.text = _currentCooldown.ToString();
            CooldownAsync(_cancellationTokenSource.Token, cooldown);
        }

        private async void CooldownAsync(CancellationToken token, int cooldown)
        {
            try
            {
                for (int i = 0; i < cooldown; i++)
                {
                    await UniTask.WaitForSeconds(1, cancellationToken: token);
                    _currentCooldown--;
                    _counter.text = _currentCooldown.ToString();
                }

                _icon.SetActive(true);
                _emptyIcon.SetActive(false);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Cooldown was cancelled because the operation was cancelled.");
            }
        }
    }
}
