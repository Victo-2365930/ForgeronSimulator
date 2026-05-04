using System.Collections;
using UnityEngine;

public class Marteau : MonoBehaviour
{

    public FeedbackHaptics haptics;
    private bool peutFrapper = true;


    [Header("Paramètres de frappe")]
    [SerializeField, Tooltip("Délai entre deux coups")]
    private float delaiEntreCoups = 1.0f;

    /// <summary>
    /// Pour gérer l'intéraction du marteau avec un lingot
    /// </summary>
    /// <param name="other">Le gameObject touché par le marteau</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!peutFrapper || !other.CompareTag("Lingot")) return;

        Lingot lingo = other.GetComponent<Lingot>();
        lingo.FrappeDeMarteau();

        haptics.OnFrappeLingot();

        StartCoroutine(GestionDelaiFrappe());

    }

    /// <summary>
    /// Pour avoir un délais entre les frappe
    /// </summary>
    private IEnumerator GestionDelaiFrappe()
    {
        peutFrapper = false;
        yield return new WaitForSeconds(delaiEntreCoups);
        peutFrapper = true;
    }
}
