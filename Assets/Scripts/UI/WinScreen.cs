using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject _winScreen;
    [SerializeField]
    private TextMeshProUGUI _ratingText;

    private void Start()
    {
        EnjoymentMeter.OnWinGame += SetupWinScreen;
    }

    private void OnDisable()
    {
        EnjoymentMeter.OnWinGame -= SetupWinScreen;
    }

    private void SetupWinScreen(float enjoyment)
    {
        _ratingText.text = $"{CalculateGuideRating(enjoyment).ToString()} Stars!";
        _winScreen.SetActive(true);
    }

    private float CalculateGuideRating(float enjoyment)
    {
        return MathF.Round(enjoyment / 200f, 1);
    }
}