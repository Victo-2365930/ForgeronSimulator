using UnityEngine;

public class Marteau : MonoBehaviour
{

    public GameController controller;
    public FeedbackHaptics haptics;

    /// <summary>
    /// Pour gérer l'intéraction du marteau avec un lingot
    /// </summary>
    /// <param name="other">Le gameObject touché par le marteau</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lingot"))
        {
            Lingot lingo = other.GetComponent<Lingot>();

            lingo.FrappeDeMarteau();
            //Délais entre 2 frappes
            //Faire un feedback haptic

        }
    }

}
