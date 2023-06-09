using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HikerFollow : MonoBehaviour
{
    public static event Action OnLoseAnimationOver;

    [SerializeField]
    private float _retreatDistance = 10f;
    [SerializeField]
    private float _retreatTimerLength = 1.5f;
    private float _retreatTimer;
    [SerializeField]
    private float _runSpeed = 5f;
    [SerializeField]
    private float _walkSpeed = 1.9f;
    [SerializeField]
    private Transform _startingPoint;

    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;
    private bool _running = false;
    private Transform _transform;
    private Vector3 _retreatPosition;
    private float _timer = 0.5f;
    private bool _goingHome = false;

    private void OnEnable()
    {
        _transform = transform;
        _retreatTimer = _retreatTimerLength;

        BearTrigger.OnBearTriggeredStatic += Retreat;
        PlayerMovement.OnStart += (playerTransform) => _playerTransform = playerTransform;
        EnjoymentMeter.OnLoseGame += GoHome;
    }

    private void OnDisable()
    {
        BearTrigger.OnBearTriggeredStatic -= Retreat;
        PlayerMovement.OnStart -= (playerTransform) => _playerTransform = playerTransform;
        EnjoymentMeter.OnLoseGame -= GoHome;
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!_goingHome)
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
            else
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _timer = 0.5f;
                    _navMeshAgent.destination = _playerTransform.position;
                }
            }
        }
    }

    private void Retreat(int _)
    {
        _running = true;
        _navMeshAgent.speed = _runSpeed;
        Vector3 retreatDirection = (/*-*/_navMeshAgent.velocity).normalized;
        _retreatPosition = _transform.position + (retreatDirection * _retreatDistance);
        _navMeshAgent.destination = _retreatPosition;
        _retreatTimer = _retreatTimerLength;
    }

    private void GoHome()
    {
        StartCoroutine(GoHomeCoroutine());
    }

    private IEnumerator GoHomeCoroutine()
    {
        _goingHome = true;
        _navMeshAgent.destination = _startingPoint.position;
        yield return new WaitForSecondsRealtime(3f);
        OnLoseAnimationOver?.Invoke();
    }
}