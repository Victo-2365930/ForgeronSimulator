using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class GameController: MonoBehaviour
{
    /*
    private XRGrabInteractable _grabInteractable;

    void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        // On s'abonne aux événements de saisie
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        _grabInteractable.selectEntered.RemoveListener(OnGrab);
        _grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // args.interactorObject donne la main qui a saisi l'objet
        Debug.Log("Marteau ramassé par : " + args.interactorObject.transform.name);

        // Ici, tu peux ajouter de la logique (ex: changer de layer, jouer un son)
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Marteau lâché");
    }
    */
}
