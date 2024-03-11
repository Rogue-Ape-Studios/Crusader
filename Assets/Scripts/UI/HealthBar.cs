using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogueApeStudio.Crusader.HealthSystem;

namespace RogueApeStudio.Crusader.UI.Healthbar
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _bar;
        [SerializeField] private Health _player;

        private float _barMaxWidth;
        private float _barWidth;
        private float _velocity;

        private void Start()
        {
            _barMaxWidth = _bar.sizeDelta.x;
        }

        private void Update()
        {
            _barWidth = (_barMaxWidth / _player.MaxHealth) * _player.CurrentHealth;

            _bar.sizeDelta = new Vector2(Mathf.SmoothDamp(_bar.sizeDelta.x, _barWidth, ref _velocity, 25 * Time.deltaTime), _bar.sizeDelta.y);
        }
    }
}
