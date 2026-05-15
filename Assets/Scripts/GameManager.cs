using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

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
    private GameObject epeeRatelier;
    [SerializeField, Tooltip("Première épée du ratelier")]
    private GameObject ratelierEpee1;
    [SerializeField, Tooltip("Première épée du ratelier")]
    private GameObject ratelierEpee2;
    [SerializeField, Tooltip("Première épée du ratelier")]
    private GameObject ratelierEpee3;
    [SerializeField, Tooltip("Première épée du ratelier")]
    private GameObject ratelierEpee4;
    [SerializeField, Tooltip("Première épée du ratelier")]
    private GameObject ratelierEpee5;

    [Header("Sons")]
    [SerializeField, Tooltip("Son joué lorsqu'une épée est terminée")]
    private AudioClip sonEpeeTerminee;

    private AudioSource audioSource;

    public enum EtatJeu { Menu, EnJeu, GameOver }
    private EtatJeu etatActuel;


    #endregion Variables

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Le jeu est figé tant que le joueur n'a pas appuyé sur "Jouer"
        Time.timeScale = 0f;

        // Afficher uniquement le menu
        ecranMenu.gameObject.SetActive(true);
        ecranGameOver.gameObject.SetActive(false);

        // Cacher l'UI du lingot au début
        if (UILingot != null)
            UILingot.gameObject.SetActive(false);

        // Réinitialiser les compteurs
        nbEpeeCompletee = 0;
        nbLingotDetruit = 0;

        // Mettre à jour les textes
        if (messageObjectif != null)
            messageObjectif.text = $"Tu dois terminer {nbEpeeACompleter} épée(s)";

        if (messageEpee != null)
            messageEpee.text = $"Tu as terminé {nbEpeeCompletee} épée(s)";

        if (messageLingot != null)
            messageLingot.text = $"Tu as cassé {nbLingotDetruit} lingot(s)";
    }


    public void CommencerJeu()
    {
        Time.timeScale = 1f; // Le jeu reprend

        ecranMenu.gameObject.SetActive(false);

        nbEpeeCompletee = 0;
        nbLingotDetruit = 0;

        MajUI();
    }


    public void Rejouer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// Pour mettre à jour le UI
    /// <IA>Aide sur le safety avec Gemini</IA>
    /// </summary>
    public void MajUI()
    {

        //Texte en haut du ratelier
        if (messageEpee != null) messageEpee.text = $"Tu as terminé {nbEpeeCompletee} épée(s)";
        if (messageLingot != null) messageLingot.text = $"Tu as cassé {nbLingotDetruit} lingot(s)";

        if (lingotActif == null) return;

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
    /// Vérifie le GameOver
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
    public void EpeeTerminee()
    {
        nbEpeeCompletee++;

        MajUI();
        RatelierEpee();

        if (audioSource != null && sonEpeeTerminee != null)
        {
            audioSource.PlayOneShot(sonEpeeTerminee);
        }

        if (nbEpeeCompletee >= nbEpeeACompleter) GameOver(true);

    }

    private void RatelierEpee()
    {
        if (nbEpeeCompletee <= 0) return;
        if (ratelierEpee1 != null) ratelierEpee1.SetActive(nbEpeeCompletee >= 1);
        if (ratelierEpee2 != null) ratelierEpee2.SetActive(nbEpeeCompletee >= 2);
        if (ratelierEpee3 != null) ratelierEpee3.SetActive(nbEpeeCompletee >= 3);
        if (ratelierEpee4 != null) ratelierEpee4.SetActive(nbEpeeCompletee >= 4);
        if (ratelierEpee5 != null) ratelierEpee5.SetActive(nbEpeeCompletee >= 5);
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
        Time.timeScale = 0f; // Stoppe tout

        if (gagne)
        {
            gameOverTitre.text = "Vous avez gagné!";
            gameOverTexte.text = "Les braises brillent encore après votre triomphe!";
        }
        else
        {
            gameOverTitre.text = "Vous avez perdu";
            gameOverTexte.text = "Le métal était trop faible... comme toi.";
        }

        // Positionner l'écran devant la caméra
        Transform cam = Camera.main.transform;
        ecranGameOver.transform.position = cam.position + cam.forward * 1.2f;
        ecranGameOver.transform.rotation = Quaternion.LookRotation(cam.forward);

        ecranGameOver.gameObject.SetActive(true);
    }

}
