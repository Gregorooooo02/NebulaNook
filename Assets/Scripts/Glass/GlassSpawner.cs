using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GlassSpawner : XRBaseInteractable
{
    [Header("References")]
    [SerializeField] private GameObject glassPrefab;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Spawn a new glass at the position of the spawner
        GameObject newGlass = Instantiate(glassPrefab, transform.position, Quaternion.identity);

        // Get the XRGrabInteractable component from the new glass
        XRGrabInteractable grabInteractable = newGlass.GetComponent<XRGrabInteractable>();

        // Select object into the interactor
        interactionManager.SelectEnter(args.interactorObject, grabInteractable);

        base.OnSelectEntered(args);
    }
}
