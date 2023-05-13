using System;
using System.Collections;
using UnityEngine;

// Put this on Camera Follower
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _follow;
    [SerializeField]
    private Transform _lookAt;
    [SerializeField, Range(0.1f, 1.0f)]
    private float _smoothTime = 0.3f;
    [SerializeField]
    private float _lookDuration = 2f;

    private Vector3 _velocity = Vector3.zero;
    private Transform _transform;

    /*    [SerializeField]
        private float _maxSpeed = 25f;*/

    private void Start()
    {
        _transform = transform;

        ViewpointTrigger.OnViewpointTriggeredStatic += LookAtWaterfall;
        EndingTrigger.OnEndingTriggeredStatic += EndingView;
    }

    private void OnDisable()
    {
        ViewpointTrigger.OnViewpointTriggeredStatic -= LookAtWaterfall;
        EndingTrigger.OnEndingTriggeredStatic -= EndingView;
    }

    private void EndingView(Transform followTransform, Transform lookAtTransform)
    {
        _follow = followTransform;
        _lookAt = lookAtTransform;
    }

    private void LookAtWaterfall(int _, Transform cameraPosition, Transform lookAt)
    {
        StartCoroutine(LookAtWaterfallCoroutine(cameraPosition, lookAt));
    }

    private IEnumerator LookAtWaterfallCoroutine(Transform cameraPosition, Transform lookAt)
    {
        Transform originalCameraPosition = _follow;
        Transform originalLookAt = _lookAt;
        _follow = cameraPosition;
        _lookAt = lookAt;
        yield return new WaitForSecondsRealtime(_lookDuration);
        _follow = originalCameraPosition;
        _lookAt = originalLookAt;
    }

    private void Update()
    {
        _transform.position = Vector3.SmoothDamp(
            _transform.position,
            _follow.position,
            ref _velocity,
            _smoothTime,
            /*_maxSpeed*/Mathf.Infinity,
            Time.unscaledDeltaTime);
        _transform.LookAt(_lookAt);
    }
}

// Something like: transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime)
// Do it like this instead though:
// But what if the target keeps moving?
/*    float lerpTime = 1f;
    float currentLerpTime;*/
// float perc = currentLerpTime / lerpTime;
// transform.position = Vector3.Lerp(startPos, endPos, perc);
// Or:
// float t = currentLerpTime / lerpTime;
// t = Mathf.Sin(t * Mathf.PI * 0.5f); Or: t = t*t*t * (t * (6f*t - 15f) + 10f);
// Have speed be