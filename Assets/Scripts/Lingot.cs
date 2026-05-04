using System.Collections;
using UnityEngine;

/*
 * À faire:
 *  Les mp3 et les intégrer
 *  Le changement de couleur du métal
 *  Changer le bras Gauche pour une pince
 *      Changement de bras dans les options?
 */

public class Lingot : MonoBehaviour
{

    #region Variables

    [SerializeField, Tooltip("Le GameObject GameManager")]
    GameManager gameManager;
    private bool forgeEnCours = false;
    private bool seauEnCours = false;

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

    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'échec avant une destruction de lingot")]
    private int nbErreurLingot = 0;
    private int erreurLingot = 0;

    [SerializeField, Tooltip("temps entre les montées de chaleur lorsque le lingot est dans la forge")]
    private float tempsForge = 1.0f;
    [SerializeField, Tooltip("Augmentation de la chaleur à chaque intervale de temps")]
    private float chaleurParTic = 5.0f;

    [SerializeField, Tooltip("Chaleur minimale")]
    private float chaleurMinimale = 60.0f;
    private float chaleurLingot = 0.0f;

    [SerializeField, Tooltip("Nombre de coup requis par phase de lingot")]
    private int nbCoupRequis = 2;
    private int nbCoup = 0;


    #endregion Variables

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
            //BruitDerreur.mp3
            erreurLingot++;
            if (erreurLingot >= nbErreurLingot) DetruireLingot();
        }
        else {
            //BruitDeForge.mp3
            nbCoup++;
            if (nbCoup % nbCoupRequis == 0) ChangerEtat();
        }
            
    }

    /// <summary>
    /// Pour mettre à jour le UI suite à un changement dans le jeu
    /// </summary>
    private void MiseAJourUI()
    {
        //NbCoup
        //Objectif
    }

    #region GestionDeChaleur


    /// <summary>
    /// Pour chaufer le lingot d'un incrément de chaleurParTic
    /// </summary>
    private void ChaufferLingot()
    {
        if (etatLingot == 5) return;
        chaleurLingot += chaleurParTic;
    }

    /// <summary>
    /// Pour refroidir le lingot de 3 incréments de chaleurParTic
    ///     Change le lingot en épée s'il est en phase 4
    /// </summary>
    private void RefroidirLingot()
    {
        if (etatLingot == 5) return;

        if (chaleurLingot > 0)
        {
            //Bruit ppsshhhh.mp3
            chaleurLingot -= chaleurParTic*3.0f;
            if (chaleurLingot < 0.0f) chaleurLingot = 0.0f;
        }

        if (etatLingot == 4) 
        {
            etatLingot = 5;
            MiseAJourVisuel();
            seauEnCours = false;
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
            StartCoroutine(GestionnaireForge());
        }

        if (other.CompareTag("Seau"))
        {
            seauEnCours = true;
            StartCoroutine(GestionnaireSeau());
        }
    }

    /// <summary>
    /// Pour gérer la sortie de colision du lingot (OnExit)
    /// </summary>
    /// <param name="other">L'autre gameobject sortit</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Forge")) forgeEnCours = false;
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
            yield return new WaitForSeconds(tempsForge);
        }
    }

    /// <summary>
    /// Pour gérer le temps du refroidissement du lingot
    /// </summary>
    private IEnumerator GestionnaireSeau()
    {
        while (seauEnCours && chaleurLingot > 0.0f)
        {
            RefroidirLingot();
            yield return new WaitForSeconds(1.0f);
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
        //TuLasPété.mp3
        gameManager.LingotDetruit();
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);

    }
    #endregion

}
