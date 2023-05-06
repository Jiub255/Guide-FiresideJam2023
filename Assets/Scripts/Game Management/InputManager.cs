using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls PC { get; private set; }

    private void Awake()
    {
        PC = new PlayerControls();

        PC.World.Enable();
    }
}