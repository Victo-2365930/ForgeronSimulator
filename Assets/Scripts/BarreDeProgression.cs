using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pour gérer une barre de progression
/// </summary>
public class BarreDeProgression : MonoBehaviour
{
    [SerializeField, Tooltip("L'image qui doit être remplie selon la progression.")]
    private Image barre;

    /// <summary>
    /// Modifie la progression affichée de la barre.
    /// </summary>
    /// <param name="progression">Le pourcentage de progression.</param>
    public void SetProgression(float progression)
    {
        progression = Mathf.Clamp01(progression);
        barre.fillAmount = progression;
    }
}
