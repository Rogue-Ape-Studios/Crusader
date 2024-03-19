using RogueApeStudio.Crusader.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueApeStudio.Crusader.UI
{
    public class GameOverCanvas : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Canvas _canvas;

        void Start()
        {
            _health.OnDeath += HandleDeath;
        }

        private void OnDestroy()
        {
            _health.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            _canvas.gameObject.SetActive(true);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _canvas.gameObject.SetActive(false);
        }
    }
}
