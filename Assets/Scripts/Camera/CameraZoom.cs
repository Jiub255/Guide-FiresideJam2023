using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Camera Leader
public class CameraZoom : MonoBehaviour
{
    [SerializeField, Range(0f, 40f)]
    private float _zoomSpeed = 20f;

    [SerializeField, Range(0f, 35f)]
    private float _zoomMinDist = 25f;

    [SerializeField, Range(35f, 255f)]
    private float _zoomMaxDist = 150f;

    [SerializeField, Range(35f, 60f)]
    private float _defaultZoomDist = 35f;

    private Transform _transform;
    private InputAction _zoomAction;

    private void Awake()
    {
        _transform = transform;

        CameraMoveRotate.OnCenterOnPlayer += CenterOnPC;
    }

    private void Start()
    {
        _zoomAction = S.I.IM.PC.World.Zoom;

        S.I.IM.PC.World.Zoom.performed += Zoom;
    }

    private void OnDisable()
    {
        S.I.IM.PC.World.Zoom.performed -= Zoom;

        CameraMoveRotate.OnCenterOnPlayer -= CenterOnPC;
    }

    private void CenterOnPC()
    {
        // Set zoom to "default" setting.
        _transform.localPosition = new Vector3(0f, 0f, -_defaultZoomDist);
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        float wheelMovement = _zoomAction.ReadValue<float>();

        Vector3 cameraZoomMovement = Vector3.forward * wheelMovement * _zoomSpeed * Time.unscaledDeltaTime;

        float modifiedLocalZ = (_transform.localPosition + cameraZoomMovement).z;

        // Clamp zoom between min and max distances
        if (modifiedLocalZ < -_zoomMinDist && modifiedLocalZ > -_zoomMaxDist)
        {
            // Move camera leader (main camera follows it smoothly)
            _transform.localPosition += cameraZoomMovement;
        }
    }
}