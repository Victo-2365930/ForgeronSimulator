using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/*
 * À faire:
 *  Les mp3 et les intégrer
 *  Menu ouvrir/fermer/Recommencer
 *	Intéraction ratelier <> Épée terminée
 *	((Bonus)Le métal orangé)
 *	Remettre les valeurs dans l'inspecteur comme valeur par défaut dans le code
 *	Le lingot pété peut se frapper encore pour plus de lingot pété
 */

public class Lingot : MonoBehaviour
{

    #region Variables

    [SerializeField, Tooltip("Le GameObject GameManager")]
    GameManager gameManager;
    private bool forgeEnCours = false;
    private bool seauEnCours = false;
    private Coroutine forgeRoutine;
    private bool estDetruit = false;


    private XRGrabInteractable grabInteractable;

    /*
     * État Lingot
     * 1 = Lingot
     * 2 = Lingot applati
     * 3 = Lame longue
     * 4 = Pré-Épée
     * 5 = Épée terminée
     */
    public int etatLingot = 1;

    [Header("Formes du lingot")]
    [SerializeField, Tooltip("Lingot Forme 1 (Lingot)")]
    public GameObject forme1;
    [SerializeField, Tooltip("Lingot Forme 2 (Lingot applati)")]
    public GameObject forme2;
    [SerializeField, Tooltip("Lingot Forme 3 (Lame longue)")]
    public GameObject forme3;
    [SerializeField, Tooltip("Lingot Forme 4 (Pré-Épée)")]
    public GameObject forme4;
    [SerializeField, Tooltip("Lingot Forme 5 (Épée)")]
    public GameObject forme5;
    [SerializeField] private Color couleurChaude = new Color(1.0f, 0.35f, 0.0f);
    private Color couleurOriginale;
    private MeshRenderer[] renderersFormes;

    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'échec avant une destruction de lingot")]
    private int nbErreurLingot = 5;
    public int erreurLingot = 0;

    [SerializeField, Tooltip("Tick entre les montées et baisses de chaleur")]
    private float tempsChaleur = 0.5f;
    [SerializeField, Tooltip("Augmentation de la chaleur à chaque intervale de temps")]
    private float chaleurParTic = 5.0f;
    [SerializeField, Tooltip("Vitesse du refroidissement naturel à l'air libre")]
    private float vitesseRefroidissementAir = 0.5f;

    [SerializeField, Tooltip("Chaleur minimale pour le frapper")]
    public float chaleurMinimale = 60.0f;
    public float chaleurLingot = 0.0f;
    [SerializeField, Tooltip("Chaleur maximale possible")]
    public float chaleurMaximale = 100.0f;

    [SerializeField, Tooltip("Nombre de coup requis par phase de lingot")]
    private int nbCoupRequis = 3;
    private int nbCoup = 0;

    [Header("Sons")]
    [SerializeField, Tooltip("Son lorsque le marteau frappe correctement")]
    private AudioClip sonForge;

    [SerializeField, Tooltip("Son lorsqu'il y a une erreur")]
    private AudioClip sonErreur;

    [SerializeField, Tooltip("Son lorsque le métal est trempé")]
    private AudioClip sonTrempage;

    [SerializeField, Tooltip("Son lorsque le lingot est détruit")]
    private AudioClip sonDestruction;

    private AudioSource audioSource;


    #endregion Variables

    private void Start()
    {
        renderersFormes = new MeshRenderer[5];
        renderersFormes[0] = forme1.GetComponent<MeshRenderer>();
        renderersFormes[1] = forme2.GetComponent<MeshRenderer>();
        renderersFormes[2] = forme3.GetComponent<MeshRenderer>();
        renderersFormes[3] = forme4.GetComponent<MeshRenderer>();
        renderersFormes[4] = forme5.GetComponent<MeshRenderer>();

        // On mémorise la couleur du matériel de la forme 1 comme base
        if (renderersFormes[0] != null)
        {
            couleurOriginale = renderersFormes[0].material.color;
        }

        StartCoroutine(GestionnaireRefroidissement());

    }

    /// <summary>
    /// Pour réagit à la frappe entre le marteau et le lingot
    ///     Change l'étatLingot ou le nombre d'erreur
    ///     Détruit le Lingot si trop d'erreur
    ///     Change l'état du lingo si assez de coup
    /// </summary>
    public void FrappeDeMarteau()
    {
        if (etatLingot == 0) return;

        if (chaleurLingot < chaleurMinimale || etatLingot == 4)
        {
            if (audioSource != null && sonErreur != null)
            {
                audioSource.PlayOneShot(sonErreur);
            }

            erreurLingot++;
            gameManager.MajUI();
            if (erreurLingot >= nbErreurLingot) DetruireLingot();
        }
        else {

            if (audioSource != null && sonForge != null)
            {
                audioSource.PlayOneShot(sonForge);
            }

            nbCoup++;
            if (nbCoup % nbCoupRequis == 0) ChangerEtat();
        }
            
    }

    #region XR GRAB
    /*
     * Pour gérer autrement le XR Grab Interactable
     * Pour gérer l'apparition et disparition du UI Lingot
     *<IA> Modification par Gemini
    */
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(AssignerCommeActif);
        grabInteractable.selectExited.AddListener(RetirerCommeActif);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(AssignerCommeActif);
        grabInteractable.selectExited.RemoveListener(RetirerCommeActif);
    }

    private void AssignerCommeActif(SelectEnterEventArgs args)
    {
        if (gameManager != null) gameManager.SelectionnerLingot(this);
    }

    private void RetirerCommeActif(SelectExitEventArgs args)
    {
        if (gameManager != null) gameManager.DeselectionnerLingot();
    }

    #endregion

    #region GestionDeChaleur


    /// <summary>
    /// Pour chaufer le lingot
    /// </summary>
    private void ChaufferLingot()
    {
        if (etatLingot == 5) return;
        chaleurLingot += chaleurParTic;
        if (chaleurLingot >= chaleurMaximale) chaleurLingot = chaleurMaximale;

        AppliquerCouleurChaleur();
        gameManager.MajUI();
    }

    /// <summary>
    /// Pour appliquer la couleur sur le lingot
    /// <IA> Généré par Gemini</IA>
    /// </summary>
    private void AppliquerCouleurChaleur()
    {
        float pourcentage = chaleurLingot / chaleurMaximale;

        // On transitionne de la couleur d'origine vers le rouge-orange
        Color couleurActuelle = Color.Lerp(couleurOriginale, couleurChaude, pourcentage);

        int index = etatLingot - 1;
        if (index >= 0 && index < renderersFormes.Length && renderersFormes[index] != null)
        {
            renderersFormes[index].material.color = couleurActuelle;
        }
    }

    /// <summary>
    /// Pour gérer la colision du lingot (OnEnter)
    /// </summary>
    /// <param name="other">L'autre gameobject touché</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Forge"))
        {
            forgeEnCours = true;

            if (forgeRoutine == null)
                forgeRoutine = StartCoroutine(GestionnaireForge());
        }

        if (other.CompareTag("Seau"))
        {
            seauEnCours = true;

            if (etatLingot == 4 && chaleurLingot >= chaleurMinimale)
            {
                etatLingot = 5;
                chaleurLingot = 0;
                MiseAJourVisuel();
                gameManager.MajUI();

                if (audioSource != null && sonTrempage != null)
                {
                    audioSource.PlayOneShot(sonTrempage);
                }
            }
        }

        if (other.CompareTag("Ratelier"))
        {
            if (etatLingot != 5 || estDetruit) return;
            estDetruit = true;
            gameManager.EpeeTerminee();
            if (grabInteractable != null) GetComponent<XRGrabInteractable>().enabled = false;

            Destroy(gameObject, 0.1f);
            
        }

    }

    /// <summary>
    /// Pour gérer la sortie de colision du lingot (OnExit)
    /// </summary>
    /// <param name="other">L'autre gameobject sortit</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Forge"))
        {
            forgeEnCours = false;

            if (forgeRoutine != null)
            {
                StopCoroutine(forgeRoutine);
                forgeRoutine = null;
            }
        }
        if (other.CompareTag("Seau")) seauEnCours = false;

    }

    /// <summary>
    /// Pour gérer le temps du chauffage du lingot
    /// </summary>
    private IEnumerator GestionnaireForge()
    {
        while (forgeEnCours)
        {
            ChaufferLingot();
            yield return new WaitForSeconds(tempsChaleur);
        }
    }

    /// <summary>
    /// Pour gérer le temps du refroidissement du lingot
    /// </summary>
    private IEnumerator GestionnaireRefroidissement()
    {
        while (true)
        {
            if (!forgeEnCours && chaleurLingot > 0 && etatLingot < 5)
            {
                float vitesse = seauEnCours ? (vitesseRefroidissementAir * 5f) : vitesseRefroidissementAir;

                chaleurLingot -= vitesse * tempsChaleur;
                if (chaleurLingot < 0) chaleurLingot = 0;

                AppliquerCouleurChaleur();
                gameManager.MajUI();
            }

            yield return new WaitForSeconds(tempsChaleur);
        }
    }

    #endregion


    #region ChangementEtat

    /// <summary>
    /// Pour changer l'état du lingot
    /// </summary>
    public void ChangerEtat()
    {
        etatLingot++;
        nbCoup = 0;
        MiseAJourVisuel();
        gameManager.MajUI();
    }

    /// <summary>
    /// Pour changer le visuel du lingot par rapport à son état
    /// </summary>
    private void MiseAJourVisuel()
    {
        forme1.SetActive(etatLingot == 1);
        forme2.SetActive(etatLingot == 2);
        forme3.SetActive(etatLingot == 3);
        forme4.SetActive(etatLingot == 4);
        forme5.SetActive(etatLingot == 5);

        AppliquerCouleurChaleur();
    }

    /// <summary>
    /// Pour détruire le lingot
    /// </summary>
    public void DetruireLingot()
    {
        StartCoroutine(GestionnaireDestruction());
    }

    /// <summary>
    /// Coroutine pour détruire le lingot 
    ///     Émet un son qui montre au joueur qu'il a pété le lingot
    ///     Attends 2 secondes que le clip jour
    ///     Détruit le lingot
    /// </summary>
    private IEnumerator GestionnaireDestruction()
    {
        if (audioSource != null && sonDestruction != null)
        {
            audioSource.PlayOneShot(sonDestruction);
        }

        gameManager.LingotDetruit();

        float dureeSon = 2.0f;

        if (sonDestruction != null)
        {
            dureeSon = sonDestruction.length;
        }

        yield return new WaitForSeconds(dureeSon);

        Destroy(gameObject);
    }

    #endregion

}
