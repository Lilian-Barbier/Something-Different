using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerInteraction : MonoBehaviour
{
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.playerInteractEvent += Interact;
    }

    private void Interact()
    {
        Debug.Log("Interacting with the environment");
    }

}
