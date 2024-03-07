using UnityEngine;
    
namespace RogueApeStudio.Crusader.Camera
{

    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _playerBodyTransform;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private float _rotationAngle = 45f;

        private Vector3 _offset;

        private void Start()
        { 
            _offset = Quaternion.Euler(_rotationAngle, 0, 0) * Vector3.back * _distance;
        }

        private void Update()
        {
            transform.position = _playerBodyTransform.position + _offset;
        }
    }

}
