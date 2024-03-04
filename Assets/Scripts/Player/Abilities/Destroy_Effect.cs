using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Effect : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy;
    void Start()
    {
        Destroy(gameObject, _timeToDestroy);
    }
}
