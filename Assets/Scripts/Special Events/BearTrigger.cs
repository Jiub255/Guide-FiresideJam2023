using System;
using UnityEngine;

public class BearTrigger : MonoBehaviour
{
    // Non-static event to trigger the actual event,
    public event Action<Vector3> OnBearTriggered;
    // and a static event for enjoyment and guide/hikers.
    public static event Action<int> OnBearTriggeredStatic;

    [SerializeField]
    private int _enjoymentAmount;
    [SerializeField]
    AudioClip _bearClip;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered");
        if (other.GetComponent<PlayerMovement>())
        {
            //Debug.Log("PlayerMovement found");
            // TODO - Have the camera center on player when the bear gets triggered. 
            OnBearTriggered?.Invoke(other.transform.position);
            OnBearTriggeredStatic?.Invoke(_enjoymentAmount);
            S.I.AudioManager.PlaySoundEffect(_bearClip);
        }
    }
}