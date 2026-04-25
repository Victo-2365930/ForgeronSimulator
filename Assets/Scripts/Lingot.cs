using UnityEngine;

public class Lingot : MonoBehaviour
{

    #region Variables

    [Header("Variables de jeu")]
    [SerializeField, Tooltip("Nombre d'échec avant une destruction de lingot")]
    private int nombreErreurLingot = 0;
    private int erreurLingot = 0;
    [SerializeField, Tooltip("Chaleur minimale")]
    private float chaleurMinimale = 60.0f;
    private float chaleurLingot = 0.0f;
    //Chaleur Maximale?

    #endregion Variables

    void Update()
    {
        if(erreurLingot >= nombreErreurLingot)
        {
            //Destruction du lingot
        }
    }


    public void FrappeDeMarteau()
    {
        //
    }


}
