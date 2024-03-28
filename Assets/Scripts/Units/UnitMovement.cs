using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RogueApeStudio.Crusader.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _aimTurnSpeed = 5;
        [SerializeField] private Transform _playerTranform;
        [SerializeField] private float _avoidanceMultiplier = 0.7f;
        [SerializeField] private float _speed = 4;
        private List<GameObject> _unitsNearby = new();

        public Transform PlayerTransform => _playerTranform;

        private void Awake()
        {
            _navMeshAgent.Stop();
        }

        private void Move()
        {
            Vector3 direction = (_navMeshAgent.path.corners[1] - transform.position).normalized;
            if(_unitsNearby.Count > 0)
            {
                Vector3 closestUnitPosition = GetClosestUnitDistance();
                Vector3 avoidanceVector = (transform.position - closestUnitPosition).normalized;
                direction = (direction + avoidanceVector * _avoidanceMultiplier).normalized; 
            }
            _navMeshAgent.velocity = direction * _speed;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag(gameObject.tag))
            {
                Debug.Log("Enter!");
                _unitsNearby.Add(col.gameObject);
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.CompareTag(gameObject.tag))
            {
                Debug.Log("Exit!");
                _unitsNearby.Remove(col.gameObject);

            }
        }

        private Vector3 GetClosestUnitDistance()
        {
            Vector3 closestUnitPosition = _unitsNearby[0].transform.position;
            float oldDistance = 9999;
            foreach (GameObject unit in _unitsNearby)
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, unit.transform.position);
                if (distance < oldDistance)
                {
                    closestUnitPosition = unit.transform.position;
                    oldDistance = distance;
                }
            }
            return closestUnitPosition;
        }

        public void MoveTo(Vector3 location)
        {
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.SetDestination(location);
                Move();
            }
        }

        public void MoveToPlayer()
        {
            MoveTo(PlayerTransform.position);          
        }

        public void SetStopDistance(float stoppingdistance)
        {
            _navMeshAgent.stoppingDistance = stoppingdistance;
        }
        public void SetSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
        }
        public void StopMoving()
        {
            _navMeshAgent.SetDestination(transform.position);
        }
        public void FacePlayer()
        {
            Vector3 direction = (PlayerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _aimTurnSpeed);
        }

    }
}