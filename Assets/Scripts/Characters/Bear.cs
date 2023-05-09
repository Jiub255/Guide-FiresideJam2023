using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    [SerializeField]
    private float _runSpeed = 5f;
    [SerializeField]
    private float _walkSpeed = 1.9f; 

    private NavMeshAgent _navMeshAgent;
    private Vector3 _startingPosition;
    private Vector3 _guideOriginalPosition;
    private bool _triggered = false;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _startingPosition = transform.position;
        _triggered = false;

        AreaTrigger.OnAreaTriggered += GoToHikersPosition;
    }

    private void OnDisable()
    {
        AreaTrigger.OnAreaTriggered -= GoToHikersPosition;
    }
    // Set animator parameter trigger to "attack" or whatever, 
    // and have the whole attack process as one animation. 

    // Or, have the bear's destination set to wherever the hikers enter the area collider. 
    // Then the hikers run away from the bear while the bear chases them. 
    // Then it goes back to its cave? 

    // Called by guide entering area collider. 
    private void GoToHikersPosition(Vector3 position)
    {
        if (!_triggered)
        {
            _navMeshAgent.speed = _runSpeed;
            _navMeshAgent.destination = position;
            _guideOriginalPosition = position;
            _triggered = true;
        }
    }

    private void Update()
    {
        if (_triggered)
        {
            if (Vector3.Distance(transform.position, _guideOriginalPosition) < 1f)
            {
                GoBackToCave();
                _triggered = false;
            }
        }
    }

    // Called by bear reaching original guide position.
    private void GoBackToCave()
    {
        _navMeshAgent.speed = _walkSpeed;
        _navMeshAgent.destination = _startingPosition;
    }
}