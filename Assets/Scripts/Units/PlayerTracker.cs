using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public static PlayerTracker instance;
    public Transform playerTransform;

    private void Awake()
    {
        instance = this;
    }

}
