using System;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public static event Action<Vector3, int> OnAreaTriggered;

    [SerializeField]
    private int _enjoymentAmount;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered");
        if (other.GetComponent<PlayerMovement>())
        {
            //Debug.Log("PlayerMovement found");
            OnAreaTriggered?.Invoke(other.transform.position, _enjoymentAmount);
        }
    }
}