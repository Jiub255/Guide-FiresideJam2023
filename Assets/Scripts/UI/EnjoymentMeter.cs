using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnjoymentMeter : MonoBehaviour
{
    public static event Action<float> OnWinGame;
    public static event Action OnLoseGame;

    [SerializeField]
    private Image _enjoymentFillBar;
	[SerializeField]
	private int _enjoymentMax = 1000;
    [SerializeField, Tooltip("In enjoyment units/second")]
    private int _enjoymentLossRate = 2;
    [SerializeField]
    private GameObject _loseScreen;

	private int _enjoyment;
    private float _timer = 1f;
    private Transform _transform;
    private bool _enjoymentPaused = true;

    private void Start()
    {
        _enjoyment = _enjoymentMax;
        _transform = transform;

        BearTrigger.OnBearTriggeredStatic += ChangeEnjoyment;
        ViewpointTrigger.OnViewpointTriggeredStatic += ChangeEnjoymentFilter;
        EndingTrigger.OnWinGame += () => { _enjoymentPaused = true; OnWinGame?.Invoke(_enjoyment); };
        IntroScreen.OnGameStart += () => { _enjoymentPaused = false; };
        HikerFollow.OnLoseAnimationOver += ResetLevel;
    }

    private void OnDisable()
    {
        BearTrigger.OnBearTriggeredStatic -= ChangeEnjoyment;
        ViewpointTrigger.OnViewpointTriggeredStatic -= ChangeEnjoymentFilter;
        EndingTrigger.OnWinGame -= () => { _enjoymentPaused = true; OnWinGame?.Invoke(_enjoyment); };
        IntroScreen.OnGameStart -= () => { _enjoymentPaused = false; };
        HikerFollow.OnLoseAnimationOver -= ResetLevel;
    }

    private void Update()
    {
        if (!_enjoymentPaused)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = 1f;
                ChangeEnjoyment(-_enjoymentLossRate);
            }
        }
    }

    // Called by events from special area colliders. 
    private void ChangeEnjoymentFilter(int amount, Transform _, Transform __)
    {
        ChangeEnjoyment(amount);
    }

    // Called by events from special area colliders. 
    private void ChangeEnjoyment(int amount)
    {
        int startingEnjoyment = _enjoyment;
        _enjoyment += amount;
        //Debug.Log($"Starting enjoyment: {startingEnjoyment}, Ending enjoyment {_enjoyment}");
        if (_enjoyment > _enjoymentMax)
        {
            _enjoyment = _enjoymentMax;
        }
        else if (_enjoyment < 0)
        {
            _enjoyment = 0;
            _enjoymentPaused = true;
            OnLoseGame?.Invoke();
            //ResetLevel();
        }
        int endingEnjoyment = _enjoyment;

        StartCoroutine(UpdateEnjoymentUICoroutine(startingEnjoyment, endingEnjoyment));
    }

    private void ResetLevel()
    {
        // Why null reference exception? Shows reference in inspector. 
        Debug.Log($"Lose screen: {_loseScreen}");
        _loseScreen.SetActive(true);
        Time.timeScale = 0f; 
    }

    private IEnumerator UpdateEnjoymentUICoroutine(int startingEnjoyment, int endingEnjoyment)
    {
        float time = 0;
        float startValue = (float)startingEnjoyment / (float)_enjoymentMax;
        float endValue = (float)endingEnjoyment / (float)_enjoymentMax;

       // Debug.Log($"Update UI Called, Start value {startValue}, End value {endValue}");

        while (time < 1f)
        {
            _enjoymentFillBar.fillAmount = Mathf.Lerp(startValue, endValue, time);
            time += Time.deltaTime;
            yield return null;
        }

        _enjoymentFillBar.fillAmount = endValue;
    }
}