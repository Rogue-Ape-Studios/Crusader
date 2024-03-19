using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogueApeStudio.Crusader.HealthSystem;
using System;

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
            _player.OnDeath += HandleDeath;
            _barMaxWidth = _bar.sizeDelta.x;
        }

        private void OnDestroy()
        {
            _player.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            _bar.sizeDelta = new(0, 0);
        }

        private void Update()
        {
            _barWidth = (_barMaxWidth / _player.MaxHealth) * _player.CurrentHealth;

            _bar.sizeDelta = new Vector2(Mathf.SmoothDamp(_bar.sizeDelta.x, _barWidth, ref _velocity, 25 * Time.deltaTime), _bar.sizeDelta.y);
        }
    }
}
