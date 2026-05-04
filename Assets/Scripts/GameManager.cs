using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Variables

    private Lingot lingotActif = null;

    private int nbLingotDetruit;

    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'épée complétés avant de gagner")]
    private int nbEpeeACompleter = 1;
    private int nbEpeeCompletee = 1;
    [SerializeField, Tooltip("Nombre de lingot détruit avant de perdre")]
    private int nbLingotDetruitMax = 4;


    [Header("UI de jeu")]
    [SerializeField, Tooltip("Écran de menu")]
    private Canvas ecranMenu;
    [SerializeField, Tooltip("Écran de gameOver")]
    private Canvas ecranGameOver;

    #endregion Variables

    void Start()
    {



    }

    /// <summary>
    /// Pour mettre à jour le UI
    /// </summary>
    private void MajUI()
    {
        
    }

    /// <summary>
    /// Pour changer le lingot actif
    /// </summary>
    /// <param name="nouveauLingotActif">Le nouveau Lingot qui a été sélectionné</param>
    public void SelectionnerLingot(Lingot nouveauLingotActif)
    {
        //Changer le lingoActif
        lingotActif = nouveauLingotActif;

        MajUI();
    }

    /// <summary>
    /// Pour incrémenter le nombre de lingot détruit
    ///     Vérifie le GameOver
    /// </summary>
    public void LingotDetruit()
    {
        nbLingotDetruit++;
        MajUI();
        if (nbLingotDetruit >= nbLingotDetruitMax) GameOverPerdu();

    }

    /// <summary>
    /// Pour ajouter une épée au compte d'épée complétés
    /// </summary>
    public void EpeeTerminee()
    {
        nbEpeeCompletee++;
        MajUI();
        //Ajouter une épée au ratelier

        if (nbEpeeCompletee >= nbEpeeACompleter) GameOverGagner();

    }

    /// <summary>
    /// Pour arrêter le jeu et mettre le UI de partie terminée (Gagné)
    /// </summary>
    private void GameOverGagner()
    {
        //Arrêter les controles
        //Mettre le ecranGameOver UI
    }

    /// <summary>
    /// Pour arrêter le jeu et mettre le UI de partie terminée (Perdu)
    /// </summary>
    private void GameOverPerdu()
    {
        //Arrêter les controles
        //Mettre le GameOver UI
    }


}
