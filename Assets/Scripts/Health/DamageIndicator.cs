using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RogueApeStudio.Crusader.HealthSystem
{
    public class DamageIndicator : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private Health _health;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Transform _canvas;

        [Header("Indicator Settings")]
        [SerializeField] private Color _endColor;
        [SerializeField, Tooltip("Keep Z at 0.")] private Vector3 _movePosition;
        [SerializeField] private float _tweenDuration = .9f;

        private void Start()
        {
            _health.OnDamage += HandleDamageTaken;
        }

        private void OnDestroy()
        {
            _health.OnDamage -= HandleDamageTaken;
            DOTween.KillAll();
            DOTween.Clear();
        }

        private void HandleDamageTaken(float damage)
        {
            var tmp = Instantiate(_text,_canvas,false);
            tmp.text = damage.ToString();
            tmp.enabled = true;
            tmp.DOColor(_endColor, _tweenDuration);
            tmp.transform.DOLocalMove(_movePosition, _tweenDuration);
            Destroy(tmp.gameObject, _tweenDuration + .1f);
        }
    }
}
