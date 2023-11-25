using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class interrupteur : MonoBehaviour
{
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] List<GameObject> triggerObjects = new List<GameObject>();
    [SerializeField] InputActionAsset assetAction;
    List<ITriggerAction> triggerActions = new List<ITriggerAction>();
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
        for (int i = 0; i < triggerObjects.Count; i++)
        {
            triggerActions.Add(triggerObjects[i].GetComponent<ITriggerAction>());
        }
    }

    void Interact(InputAction.CallbackContext action)
    {
        interating = true;
    }
    void TriggerActions()
    {
        for (int i = 0; i < triggerActions.Count;i++)
        {
            triggerActions[i].Trigger();
        }
    }
    void Update()
    {
        if (canPress && interating) // Changer la couleur lorsqu'il peut appuyer et qu'il appuie
        {
            renderer.material.SetColor("_Color", renderer.material.color == Color.red ? Color.blue : Color.red);
            TriggerActions();
            interating = false;
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
