using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Camera Focal Point
public class CameraMoveRotate : MonoBehaviour
{
    public static event Action OnCenterOnPlayer;

    [SerializeField, Range(0f, 50f), Header("Keyboard Movement")]
    private float _movementSpeed = 20f;

    private Vector3 _forward;
    private Vector3 _right;

    [SerializeField, Range(0f, 2f), Header("Rotate")]
    private float _rotationSpeed = 0.15f;
    [SerializeField, Range(0f, 40f)]
    private float _rotationXMin = 5f;
    [SerializeField, Range(20f, 90f)]
    private float _rotationXMax = 30f;

    [SerializeField, Range(0f, 50f), Header("Edge Scrolling")]
    private float _edgeScrollingSpeed = 20f;
    [SerializeField, Range(0f, 30f), Tooltip("Calculated using percent of width")]
    private float _percentDistanceFromEdges = 10f;

    private float _screenWidth;
    private float _screenHeight;
    private float _edgeDistance;

    [SerializeField, Header("For Centering on Guide")]
    private Transform _guideTransform;
    [SerializeField, Range(0f, 75f)]
    private float _cameraXRotation = 35f;
    [SerializeField, Range(0f, 360f)]
    private float _cameraYRotation = 225f;

    private InputAction _moveCameraAction;
    private InputAction _mouseDeltaAction;
    private Transform _transform;

    [SerializeField, Header("For Following Guide"), Range(0.1f, 1.0f)]
    private float _smoothTime = 0.3f;

    private Vector3 _velocity = Vector3.zero;
    private bool _following = true;

    private void Start()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _edgeDistance = _screenWidth * (_percentDistanceFromEdges / 100);
        _moveCameraAction = S.I.IM.PC.World.MoveCamera;
        _mouseDeltaAction = S.I.IM.PC.World.MouseDelta;
        _transform = transform;
        _following = true;

        S.I.IM.PC.World.CenterOnPlayer.performed += CenterOnGuide;

        // Center on player. 
        _transform.position = _guideTransform.position;
        _transform.rotation = Quaternion.Euler(new Vector3(_cameraXRotation, _cameraYRotation, 0f));
        // CameraZoom listens and sets zoom distance to default. 
        OnCenterOnPlayer?.Invoke();
    }

    private void OnDisable()
    {
        S.I.IM.PC.World.CenterOnPlayer.performed -= CenterOnGuide;
    }

    private void LateUpdate()
    {
        // Zoom overrides everything else. Not noticeable since this action gets called only during
        // isolated frames, but it helps resolve some issues with moving while zooming.
        if (!S.I.IM.PC.World.Zoom.WasPerformedThisFrame())
        {
            GetVectors();

            KeyboardMove();

            if (S.I.IM.PC.World.RotateCamera.IsPressed())
            {
                RotateCamera();
            }
            // Don't edge scroll if holding down mouse wheel button.
            // Gets annoying when you accidentally edge scroll because you moved the mouse too far while rotating.
            else
            {
               // EdgeScroll();
            }
        }

        if (_following)
        {
            _transform.position = Vector3.SmoothDamp(
                _transform.position,
                _guideTransform.position,
                ref _velocity,
                _smoothTime,
                /*_maxSpeed*/Mathf.Infinity,
                Time.unscaledDeltaTime);
        }
    }

    private void CenterOnGuide(InputAction.CallbackContext context)
    {
        _following = true;
        // TODO - Lerp quickly instead of instantly move there? Looks jumpy when you center on PC as is. 
        _transform.position = _guideTransform.position;
        _transform.rotation = Quaternion.Euler(new Vector3(_cameraXRotation, _cameraYRotation, 0f));
        // CameraZoom listens and sets zoom distance to default. 
        OnCenterOnPlayer?.Invoke();
    }

    private void GetVectors()
    {
        _forward = _transform.forward;
        _right = _transform.right;

        // Project the forward and right vectors onto the horizontal plane (y = 0)
        _forward.y = 0f;
        _right.y = 0f;

        // Normalize them
        _forward.Normalize();
        _right.Normalize();
    }

    private void KeyboardMove()
    {
        // Get input
        Vector2 movement = _moveCameraAction.ReadValue<Vector2>();

        if (movement.sqrMagnitude > 0.1f)
        {
            // Set following bool to false. 
            _following = false;

            // Translate movement vector to world space
            Vector3 keyboardMovement = (_forward * movement.y) + (_right * movement.x);

            // Move
            _transform.position += keyboardMovement * _movementSpeed * Time.unscaledDeltaTime;
        }
    }

    private void RotateCamera()
    {
        // Rotation around y-axis
        float deltaX =
            _mouseDeltaAction.ReadValue<Vector2>().x *
            _rotationSpeed;

        _transform.RotateAround(_transform.position, Vector3/*transform*/.up, deltaX);

        // Rotation around axis parallel to your local right vector, this axis always parallel to xz-plane.
        Vector3 axis = new Vector3(
            -Mathf.Cos(Mathf.Deg2Rad * _transform.rotation.eulerAngles.y),
            0f,
            Mathf.Sin(Mathf.Deg2Rad * _transform.rotation.eulerAngles.y));

        float deltaY =
            _mouseDeltaAction.ReadValue<Vector2>().y *
            _rotationSpeed;

        // Clamp x-rotation between min and max values (at most 0 - 90).
        if (_transform.rotation.eulerAngles.x - deltaY > _rotationXMin && _transform.rotation.eulerAngles.x - deltaY <= _rotationXMax)
        {
            _transform.RotateAround(_transform.position, axis, deltaY);
        }
    }

    private void EdgeScroll()
    {
        // Get mouse screen position
        Vector3 mousePos =
            S.I.IM.PC.World.MousePosition.ReadValue<Vector2>();

        int mouseX = 0;
        int mouseY = 0;

        // Check if mouse screen position is near the edges
        if (mousePos.x > _screenWidth - _edgeDistance)
        {
            mouseX = 1;
        }
        else if (mousePos.x < _edgeDistance)
        {
            mouseX = -1;
        }

        if (mousePos.y > _screenHeight - _edgeDistance)
        {
            mouseY = 1;
        }
        else if (mousePos.y < _edgeDistance)
        {
            mouseY = -1;
        }

        // Direction we want to move
        Vector3 edgeScrollMovement = (_forward * mouseY) + (_right * mouseX);
        edgeScrollMovement.Normalize();

        // Move camera
        _transform.position += edgeScrollMovement * _edgeScrollingSpeed * Time.unscaledDeltaTime;
    }
}