using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Variables

    private Lingot lingotActif = null;

    private int nbLingotDetruit;

    private float pourcentageChaleur;


    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'épée complétés avant de gagner")]
    private int nbEpeeACompleter = 1;
    private int nbEpeeCompletee = 0;
    [SerializeField, Tooltip("Nombre de lingot détruit avant de perdre")]
    private int nbLingotDetruitMax = 4;


    [Header("Écrans de jeu")]
    [SerializeField, Tooltip("Écran de menu")]
    private Canvas ecranMenu;
    [SerializeField, Tooltip("Écran de gameOver")]
    private Canvas ecranGameOver;

    [Header("UI du joueur")]
    [SerializeField, Tooltip("Barre de progression de chaleur")]
    private BarreDeProgression barreChaleur;
    [SerializeField, Tooltip("Image d'erreur #1")]
    private Image erreur1;
    [SerializeField, Tooltip("Image d'erreur #2")]
    private Image erreur2;
    [SerializeField, Tooltip("Image d'erreur #3")]
    private Image erreur3;
    [SerializeField, Tooltip("Texte d'instruction")]
    private TextMeshProUGUI instruction;
    private string[] textesInstruction = {
        "Lingot cassé",
        "Frappez le métal",
        "Frappez le métal",
        "Frappez le métal",
        "Trempez le métal dans le seau",
        "Emmenez l'épée au ratelier",
        "Chauffez le métal"
    };






    #endregion Variables


    void RecommencerJeu()
    {
        //Reset la scène?
    }

    /// <summary>
    /// Pour mettre à jour le UI
    /// </summary>
    public void MajUI()
    {
        if(lingotActif != null)
        {
            //Barre de Chaleur
            float chaleurActuelle = lingotActif.chaleurLingot;
            pourcentageChaleur =  chaleurActuelle / lingotActif.chaleurMaximale;
            barreChaleur.SetProgression(pourcentageChaleur);

            //Carrés d'erreurs
            int nbErreur = lingotActif.erreurLingot;
            erreur1.color = (nbErreur >= 1) ? Color.red : Color.white;
            erreur2.color = (nbErreur >= 2) ? Color.red : Color.white;
            erreur3.color = (nbErreur >= 3) ? Color.red : Color.white;

            //Texte d'instruction
            int etatLingot = lingotActif.etatLingot;
            float chaleurFrappeMin = lingotActif.chaleurMinimale;
            if (etatLingot == 5) instruction.text = textesInstruction[5];
            else if (chaleurActuelle <= chaleurFrappeMin) instruction.text = textesInstruction[6];
            else instruction.text = textesInstruction[etatLingot];

            //Texte en haut du ratelier

        }

    }

    /// <summary>
    /// Pour changer le lingot actif
    /// </summary>
    /// <param name="nouveauLingotActif">Le nouveau Lingot qui a été sélectionné</param>
    public void SelectionnerLingot(Lingot nouveauLingotActif)
    {
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
