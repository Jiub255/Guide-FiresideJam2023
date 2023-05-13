using System;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public static event Action OnGameStart;

    [SerializeField]
    private GameObject _introScreen;

	public void StartGame()
    {
        OnGameStart?.Invoke();
        _introScreen.SetActive(false);
    }
}