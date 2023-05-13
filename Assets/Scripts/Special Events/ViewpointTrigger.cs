using System;
using UnityEngine;

public class ViewpointTrigger : MonoBehaviour
{
    // Non-static event to trigger the actual event,
  //  public event Action<Vector3> OnViewpointTriggered;
    // and a static event for enjoyment and guide/hikers.
    public static event Action<int, Transform, Transform> OnViewpointTriggeredStatic;

    [SerializeField]
    private int _enjoymentAmount;
    [SerializeField]
    private Transform _cameraPositionTransform;
    [SerializeField]
    private Transform _lookAtTransform;

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
           // OnViewpointTriggered?.Invoke(other.transform.position);
            OnViewpointTriggeredStatic?.Invoke(_enjoymentAmount, _cameraPositionTransform, _lookAtTransform);
            gameObject.SetActive(false);
        }
    }
}