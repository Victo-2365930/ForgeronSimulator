using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Variables

    private Lingot lingotActif = null;

    private int nbLingotDetruit;

    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'épée complétés avant de gagner")]
    private int nbEpeeACompleter = 1;
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
    /// Pour changer le lingot actif
    /// </summary>
    /// <param name="nouveauLingotActif">Le nouveau Lingot qui a été sélectionné</param>
    public void SelectionnerLingot(Lingot nouveauLingotActif)
    {
        //Changer le lingoActif
        lingotActif = nouveauLingotActif;

        
        //Mettre les UI à jour pour le lingot actif


    }

    /// <summary>
    /// Pour incrémenter le nombre de lingot détruit
    ///     Vérifie le GameOver
    /// </summary>
    public void LingotDetruit()
    {
        nbLingotDetruit++;
        if (nbLingotDetruit >= nbLingotDetruitMax) GameOverPerdu();

    }

    private void GameOverGagner()
    {
        //Mettre le ecranGameOver UI
    }

    private void GameOverPerdu()
    {
        //Mettre le GameOver UI
        //
    }


}
