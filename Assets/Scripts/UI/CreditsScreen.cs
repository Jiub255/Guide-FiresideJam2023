using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject _creditsScreen;
	[SerializeField]
	private GameObject _winScreen;
	[SerializeField]
	private GameObject _loseScreen;

	public void OpenCredits()
    {
		_loseScreen.SetActive(false);
		_winScreen.SetActive(false);
		_creditsScreen.SetActive(true);
    }
}