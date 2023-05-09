using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Move player to where mouse clicked using NavMeshAgent
public class PlayerMovement : MonoBehaviour
{
    public static event Action<Transform> OnStart;

    [SerializeField]
    private LayerMask _terrainLayerMask;
    [SerializeField]
    private float _retreatDistance = 10f;
    [SerializeField]
    private float _retreatTimerLength = 1.5f;
    private float _retreatTimer;
    [SerializeField]
    private float _runSpeed = 5f;
    [SerializeField]
    private float _walkSpeed = 1.9f; 

	private NavMeshAgent _navMeshAgent;
    private Camera _camera;
    private InputAction _mousePositionAction;
    private EventSystem _eventSystem;
    private bool _pointerOverUI;
    private bool _running = false;
    private Vector3 _retreatPosition;
    private Transform _transform;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
        _mousePositionAction = S.I.IM.PC.World.MousePosition;
        _eventSystem = EventSystem.current;
        _running = false;
        _transform = transform;
        _retreatTimer = _retreatTimerLength;

        // Sends to the hikers so they can have the transform to follow. 
        OnStart?.Invoke(transform);

        AreaTrigger.OnAreaTriggered += Retreat;
        S.I.IM.PC.World.MovePlayer.performed += MovePlayer;
    }

    private void OnDisable()
    {
        AreaTrigger.OnAreaTriggered -= Retreat;
        S.I.IM.PC.World.MovePlayer.performed -= MovePlayer;
    }

    private void Retreat(Vector3 _)
    {
        _running = true;
        _navMeshAgent.speed = _runSpeed;
        Vector3 retreatDirection = (-_navMeshAgent.velocity).normalized;
        Vector3 retreatPosition = _transform.position + (retreatDirection * _retreatDistance);
        _navMeshAgent.destination = retreatPosition;
        _retreatTimer = _retreatTimerLength;
    }

    private void Update()
    {
        if (_running)
        {
            _retreatTimer -= Time.deltaTime;
            if (_retreatTimer < 0)
            {
                _navMeshAgent.speed = _walkSpeed;
                _running = false;
                _retreatTimer = _retreatTimerLength;
            }
        }

        if (_eventSystem.IsPointerOverGameObject())
        {
            _pointerOverUI = true;
        }
        else
        {
            _pointerOverUI = false;
        }
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        if (!_pointerOverUI)
        {
            RaycastHit hit;

            if (Physics.Raycast(
                _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
                out hit,
                1000,
                _terrainLayerMask))
            {
                _navMeshAgent.destination = hit.point;
            }
        }
    }
}