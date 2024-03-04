using UnityEngine;

namespace RogueApeStudio.Crusader.Player.Visibility
{
    public class MakePlayerVisible : MonoBehaviour
    {
        [SerializeField] private Material _seeThrough;
        [SerializeField] private Material _default;

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<Renderer>().material = _seeThrough;
        }

        private void OnTriggerExit(Collider other)
        {
            other.GetComponent<Renderer>().material = _default;
        }
    }
}