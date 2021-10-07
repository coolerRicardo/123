using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodySleep : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.Sleep();
    }
}
