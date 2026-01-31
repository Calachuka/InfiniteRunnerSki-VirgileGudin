using UnityEngine;

public class TestCode2 : MonoBehaviour
{
    float tempsEntreAttaques = 10f;
    float dernierTempsAttaque;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attaquer();
        }

        // Affiche combien de secondes il reste avant la prochaine attaque
        // le temps qu'il reste a attendre ne s'affiche que pendant le cooldown
        if (PeutAttaquer() == false)
        { 
        float rechargeArme = (dernierTempsAttaque + tempsEntreAttaques) - Time.time; // Time.time = temps écoulé depuis le lancement du jeu
        // Debug.Log("temps restant avant la prochaine attaque" + rechargeArme);
        }


    }

    void Attaquer()
    {
        if (PeutAttaquer())
        {
            Debug.Log("Attaque lancée ");
            dernierTempsAttaque = Time.time; // Time.time = temps écoulé depuis le lancement du jeu, donc dernierTempsAttaque est le moment (en secondes) où la dernière attaque a été faite
                                            // ex : une attaque a étée lancé a 31.534 seconde
            Debug.Log("dernierTempsAttaque" + dernierTempsAttaque);
        }
        else
        {
            Debug.Log("Attaque en recharge ");


        }
    }

    bool PeutAttaquer()
    {
        return Time.time >= dernierTempsAttaque + tempsEntreAttaques; // Time.time = temps écoulé depuis le lancement du jeu
    }

}
