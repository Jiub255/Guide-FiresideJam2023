using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HikerFollow : MonoBehaviour
{
	private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;

    private void OnEnable()
    {
        PlayerMovement.OnStart += (playerTransform) => _playerTransform = playerTransform;
    }

    private void OnDisable()
    {
        PlayerMovement.OnStart -= (playerTransform) => _playerTransform = playerTransform;
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(WaitThenResetDestination());
    }

    // Don't need to update their destination every frame, every half second or so should be fine. Maybe. 
    private IEnumerator WaitThenResetDestination()
    {
        yield return new WaitForSeconds(0.5f);

        _navMeshAgent.destination = _playerTransform.position;

        StartCoroutine(WaitThenResetDestination());
    }
}