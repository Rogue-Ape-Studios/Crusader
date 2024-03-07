using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio
{
    public class Javelin : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Invoke(nameof(DestroyThis), 1f);
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }

    }
}
