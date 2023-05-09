using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public static event Action<Vector3> OnAreaTriggered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.GetComponent<PlayerMovement>())
        {
            Debug.Log("PlayerMovement found");
            OnAreaTriggered?.Invoke(other.transform.position);
        }
    }

    /*    private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Collider entered with {collision.gameObject.name}");
            if (collision.gameObject.GetComponent<PlayerMovement>())
            {
                Debug.Log("PlayerMovement found");
                OnAreaTriggered?.Invoke(collision.transform.position);
            }
        }*/
}