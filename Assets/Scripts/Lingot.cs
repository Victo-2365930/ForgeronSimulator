using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/*
 */

public class Lingot : MonoBehaviour
{

    #region Variables

    [SerializeField, Tooltip("Le GameObject GameManager")]
    GameManager gameManager;

    /*
     * État Lingot
     * 0 = Détruit (Inutilisable)
     * 1 = Lingot
     * 2 = Lingot applati
     * 3 = Lame longue
     * 4 = Pré-Épée
     * 5 = Épée terminée
     */
    public int etatLingot = 1;

    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'échec avant une destruction de lingot")]
    private int nbErreurLingot = 0;
    private int erreurLingot = 0;

    [SerializeField, Tooltip("Chaleur minimale")]
    private float chaleurMinimale = 60.0f;
    private float chaleurLingot = 0.0f;

    [SerializeField, Tooltip("Nombre de coup requis par phase de lingot")]
    private int nbCoupRequis = 2;
    private int nbCoup = 0;

    #endregion Variables

    void Update()
    {
        

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

    private void MiseAJourUI()
    {
        //NbCoup
        //Objectif
    }

    private void ChaufferLingot()
    {
        if (etatLingot == 0 || etatLingot == 5) return;

        //Coroutine ++chaleurLingot
    }

    private void DetruireLingot()
    {
        etatLingot = 0;
        //Delay
        //Détruire le GameObject;

        //Changer UI
    }

    public void ChangerEtat()
    {
        etatLingot++;
        //Changer le gameobject

    }

    public void TerminerEpee()
    {
        if (etatLingot == 4)
        {
            //BruitPSSSHHHHHH.mp3
            etatLingot = 5;
            //Transformer Épée
        }
               
    }

    public void DestructionLingot()
    {
        gameManager.LingotDetruit();
    }


}
