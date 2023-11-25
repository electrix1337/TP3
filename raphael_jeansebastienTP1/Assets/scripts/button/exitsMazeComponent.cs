using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class exitsMazeComponent : MonoBehaviour
{
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] InputActionAsset assetAction;
    [SerializeField] GameObject endGameCanvas;
    bool canPress = false;
    bool interating = false;

    Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        textMesh.enabled = false;

        InputActionMap actionMap = assetAction.FindActionMap("player");
        InputAction interact = actionMap.FindAction("interact");

        interact.performed += Interact;
    }

    void Interact(InputAction.CallbackContext action)
    {
        interating = true;
    }
    void Update()
    {
        if (canPress && interating) // Changer la couleur lorsqu'il peut appuyer et qu'il appuie
        {
            renderer.material.SetColor("_Color", renderer.material.color == Color.red ? Color.blue : Color.red);
            interating = false;
            endGameCanvas.SetActive(true);
        }
        else
        {
            interating = false;
        }
    }
    private void OnTriggerEnter(Collider other) // Le joueurs est assez proche, il peut voir le text et appuyer
    {
        textMesh.enabled = true;
        canPress = true;
    }

    private void OnTriggerExit(Collider other) // Le joueurs n'est pas assez proche, il ne voit pas le text et ne peut pas appuyer
    {
        textMesh.enabled = false;
        canPress = false;
    }
}
