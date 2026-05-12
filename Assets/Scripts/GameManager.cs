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
    private int nbEpeeACompleter = 3;
    private int nbEpeeCompletee = 0;
    [SerializeField, Tooltip("Nombre de lingot détruit avant de perdre")]
    private int nbLingotDetruitMax = 4;


    [Header("Écrans de jeu")]
    [SerializeField, Tooltip("Écran de menu")]
    private Canvas ecranMenu;

    [Header("Écran de GameOver")]
    [SerializeField, Tooltip("Canvas de gameOver")]
    private Canvas ecranGameOver;
    [SerializeField, Tooltip("Titre du GameOver")]
    private TextMeshProUGUI gameOverTitre;
    [SerializeField, Tooltip("Texte de l'écran")]
    private TextMeshProUGUI gameOverTexte;


    [Header("UI du lingot")]
    [SerializeField, Tooltip("UI du lingot")]
    private Canvas UILingot;
    [SerializeField, Tooltip("Barre de progression de chaleur")]
    private BarreDeProgression barreChaleur;
    [SerializeField, Tooltip("Image d'erreur #1")]
    private Image erreur1;
    [SerializeField, Tooltip("Image d'erreur #2")]
    private Image erreur2;
    [SerializeField, Tooltip("Image d'erreur #3")]
    private Image erreur3;
    [SerializeField, Tooltip("Image d'erreur #3")]
    private Image erreur4;
    [SerializeField, Tooltip("Image d'erreur #3")]
    private Image erreur5;
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

    [Header("Canvas Objectif")]
    [SerializeField, Tooltip("Message affichant le nombre d'épées à terminer")]
    private TextMeshProUGUI messageObjectif;
    [SerializeField, Tooltip("Message affichant le nombre d'épées complétées")]
    private TextMeshProUGUI messageEpee;
    [SerializeField, Tooltip("Message affichant le nombre de lingots cassés")]
    private TextMeshProUGUI messageLingot;

    [Header("Ratelier")]
    [SerializeField, Tooltip("Épées affichées dans le ratelier")]
    private GameObject[] epeesRatelier;

    #endregion Variables

    private void Start()
    {
        // Message initialisé pour l'objectif
        if (messageObjectif != null)
            messageObjectif.text = $"Tu dois terminer {nbEpeeACompleter} épée(s)";

        // Message initialisé pour le nombre d'épée terminé
        if (messageEpee != null)
            messageEpee.text = $"Tu as terminé {nbEpeeCompletee} épée(s)";

        // Message initialisé pour le nombre de lingot cassé
        if (messageLingot != null)
            messageLingot.text = $"Tu as cassé {nbLingotDetruit} lingot(s)";
    }
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
            erreur4.color = (nbErreur >= 4) ? Color.red : Color.white;
            erreur5.color = (nbErreur >= 5) ? Color.red : Color.white;

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
        UILingot.gameObject.SetActive(true);
        lingotActif = nouveauLingotActif;
        MajUI();
    }

    public void DeselectionnerLingot()
    {
        UILingot.gameObject.SetActive(false);
        lingotActif = null;
    }

    /// <summary>
    /// Pour incrémenter le nombre de lingot détruit
    ///     Vérifie le GameOver
    /// </summary>
    public void LingotDetruit()
    {
        nbLingotDetruit++;
        MajUI();

        // Message pour lingot cassé
        if (messageLingot != null)
            messageLingot.text = $"Tu as cassé {nbLingotDetruit} lingot(s)";

        if (nbLingotDetruit >= nbLingotDetruitMax) GameOver(false);

    }
    /// <summary>
    /// Pour ajouter une épée au compte d'épée complétés
    /// </summary>
    public void EpeeTerminee(GameObject epeeJoueur)
    {
        nbEpeeCompletee++;
        MajUI();

        // Active une épée dans le ratelier
        for (int i = 0; i < epeesRatelier.Length; i++)
        {
            if (!epeesRatelier[i].activeSelf)
            {
                epeesRatelier[i].SetActive(true);
                break;
            }
        }

        // Détruit l'épée du joueur
        Destroy(epeeJoueur);

        // Message pour épée terminée
        if (messageEpee != null)
            messageEpee.text = $"Tu as terminé {nbEpeeCompletee} épée(s)";

        if (nbEpeeCompletee >= nbEpeeACompleter)
            GameOver(true);
    }

    /// <summary>
    /// Pour mettre l'écran de fin
    /// </summary>
    /// <param name="gagne">
    ///  true = Mettre l'écran de victoire
    ///  false = Mettre l'écran de défaite
    ///  </param>
    private void GameOver(bool gagne)
    {
        if (gagne)
        {
            gameOverTitre.text = "Vous avez gagné!";
            gameOverTexte.text = "Les braises brillent encore après votre triomphe!";

        }
        else
        {
            gameOverTitre.text = "Vous avez perdu";
            gameOverTexte.text = "Le métal était trop faible....comme toi.";
        }
        ecranMenu.gameObject.SetActive(false);
        ecranGameOver.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ratellier"))
        {
            GetComponent<GameManager>().EpeeTerminee(gameObject);
        }
    }


}
