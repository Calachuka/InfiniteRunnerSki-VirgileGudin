using UnityEngine;

public class TestCode : MonoBehaviour
{
        int vie = 50;

        void Start()
        {
            Debug.Log("Vie de départ : " + vie);

            PrendreDegats(20);
            Soigner(10);
            PrendreDegats(60);
        }

        void PrendreDegats(int degats)
        {
            vie -= degats; // abreviation pour dire : vie = vie - degats;

            //  j'aurais pu ecrire directement cela et me passer de la fonction EstMort() (creée en dessous)
            //  if (vie <= 0) 
            if (EstMort()) // EstMort() (fonction créer en dessous) est true 
            {
                vie = 0; // sécurité pour éviter les valeurs négatives
                Debug.Log("Le joueur est mort");
            }
            else
            {
                Debug.Log("Vie restante : " + vie);
            }
        }


        void Soigner(int soin)
        {
            vie = vie + soin;
            Debug.Log("Le joueur se soigne de " + soin + ". Vie actuelle : " + vie);
        }

        // je créer EstMort()
        // car si tu veux savoir si le joueur est mort ailleurs dans le code, tu as juste appeler EstMort()
        bool EstMort() // Donc cette fonction ne fait pas une action, elle répond à une question (oui/non).
        {
            return vie <= 0; // return true si la vie est inférieur ou = à 0
        }



}
