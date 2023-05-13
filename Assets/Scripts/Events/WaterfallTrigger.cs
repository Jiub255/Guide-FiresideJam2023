using System;
using UnityEngine;

public class WaterfallTrigger : MonoBehaviour
{
    // Non-static event to trigger the actual event,
    public event Action<Vector3> OnWaterfallTriggered;
    // and a static event for enjoyment and guide/hikers.
    public static event Action<int> OnWaterfallTriggeredStatic;

    [SerializeField]
    private int _enjoymentAmount;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered");
        if (other.GetComponent<PlayerMovement>())
        {
            //Debug.Log("PlayerMovement found");
            // TODO - Have the game pause, then the camera slowly move to a set viewpoint/direction/zoom.
            // Can just move the focal point and set the _smoothDampTime really low while doing this.
            // Then after a second or so, center on the guide again or put the camera back to where it was before
            // (Transform, _following bool, whatever else). 
            OnWaterfallTriggered?.Invoke(other.transform.position);
            OnWaterfallTriggeredStatic?.Invoke(_enjoymentAmount);
        }
    }
}