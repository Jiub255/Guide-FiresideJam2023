using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Move player to where mouse clicked using NavMeshAgent
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private LayerMask _terrainLayerMask;

	private NavMeshAgent _navMeshAgent;
    private Camera _camera;
    private InputAction _mousePositionAction;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Start()
    {
        _mousePositionAction = S.I.IM.PC.World.MousePosition;

        S.I.IM.PC.World.MovePlayer.performed += MovePlayer;
    }

    private void OnDisable()
    {
        S.I.IM.PC.World.MovePlayer.performed -= MovePlayer;
    }

    private void MovePlayer(InputAction.CallbackContext context)
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