using System;
using System.Collections;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    public static event Action<Transform, Transform> OnEndingTriggeredStatic;
    public static event Action OnWinGame;

    //public event Action<Vector3> OnEndingTriggered;

    // Put camera at position, then slowly rotate it in a circle, then go to win screen. 
    [SerializeField]
    private Transform _endingCameraPosition;
    [SerializeField]
    private Transform _focalPoint;

    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered");
        if (other.GetComponent<PlayerMovement>() && !_triggered)
        {
            _triggered = true;
            //Debug.Log("PlayerMovement found");
            // TODO - Have the game pause, then the camera slowly move to a set viewpoint/direction/zoom.
            // Can just move the focal point and set the _smoothDampTime really low while doing this.
            // Then after a second or so, center on the guide again or put the camera back to where it was before
            // (Transform, _following bool, whatever else). 
           // OnEndingTriggered?.Invoke(other.transform.position);
            OnEndingTriggeredStatic?.Invoke(_endingCameraPosition, _focalPoint);
           // gameObject.SetActive(false);
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(RotateView());
        }
    }

    private IEnumerator RotateView()
    {
        float timer = 0f;
        float duration = 4f;
        Quaternion startValue = Quaternion.Euler(0f, 0f, 0f);
        Quaternion endValue = Quaternion.Euler(0f, -180f, 0f);
        while (timer < duration)
        {
            _endingCameraPosition.rotation = Quaternion.Lerp(startValue, endValue, timer / duration);
            timer += Time.unscaledDeltaTime;
            Debug.Log($"Y-rotation: {_endingCameraPosition.rotation.eulerAngles.y}");
            yield return null;
        }
        _endingCameraPosition.rotation = endValue;

        // Pause game, go to win screen. 
        Debug.Log("You win!");
        OnWinGame?.Invoke();
    }
}