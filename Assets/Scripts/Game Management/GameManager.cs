using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timeScaleMultiplier = 1.5f;

    private void Awake()
    {
        // Can't change mixamo animations, so just speed up game time to speed up animations. 
        // Shouldn't be a problem since nothing else depends on time in game, as long as this value 
        // gets set early and doesn't change too much. Could ruin other animations or the "enjoyment bar" otherwise. 
        Time.timeScale = _timeScaleMultiplier;
    }
}