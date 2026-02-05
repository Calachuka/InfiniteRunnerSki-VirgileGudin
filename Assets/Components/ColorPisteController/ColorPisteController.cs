using NUnit.Framework;
using System.Net;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreController : MonoBehaviour
{
    // ********** BUT **********
    // vu que dans le jeu score et avalanche dependent de la couleur de la piste
    // je dois créer avant un script qui va me dire quelle couleur est ma piste
    // sachant que je commence en piste verte, à 1 minute, je passe en piste bleu, 2 minute : rouge, 3 minute : noire et jusqu’à la fin de la partie
    // point / seconde suivant les pistes : Verte : 10, Bleue : 25, Rouge : 50, Noire : 100
    // ************************

    // Je déclare une variable _PisteColor qui récupérera la couleur de ma piste, je l'initialise à "Verte"
    [SerializeField] private string _pisteColorCurrent;
    // public string PisteColor => _pisteColorCurrent; // je mon _PisteColor public c.a.d accessible seulement en lecture aux autres scripts, je nomme cette variable "PisteColor"

    [Header("Time track change color, in second")]
    [SerializeField] private float _timeSecondPisteVerte = 0f;
    [SerializeField] private float _timeSecondPisteBleu = 60f;
    [SerializeField] private float _timeSecondPisteRouge = 120f;
    [SerializeField] private float _timeSecondPisteNoire = 180f;



    // Update is called once per frame
    void Update() // pas mettre dans FixedUpdate meme si mon score depend du temps passé, FixedUpdate() est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
    {
        TrackColor();
    }


    // méthode qui me dit en quelle couleur de piste (Track) je suis
    // ---------------------------
    private void TrackColor()
    {
        // Je créer une variable qui recupere le temps depuis le debut du jeu
        float gameTime = Time.time; // par convention le nom d'une variable avec une maj au début, MAIS pas la car ce son des variable dites locale (déclarée dans la fonction)
        // Debug.Log("Temps depuis le debut du jeu : " + gameTime);

        string pisteColorCurrentNew; // je déclare cette variable qui va me servire ensuite pour la comparaison, savoir que j'invoke le GameEventService.cs

        if (gameTime < _timeSecondPisteBleu)
        {
            pisteColorCurrentNew = "Verte";
        }
        else if (gameTime < _timeSecondPisteRouge)
        {
            pisteColorCurrentNew = "Bleu";
        }
        else if (gameTime < _timeSecondPisteNoire)
        {
            pisteColorCurrentNew = "Rouge";
        }
        else // sinon je suis en piste noire
        {
            pisteColorCurrentNew = "Noire";
        }
        // Debug.Log("pisteColorCurrentNew : " + pisteColorCurrentNew);
        // Debug.Log("_pisteColorCurrent : " + _pisteColorCurrent);

        // Un event doit être déclenché que quand quelque chose CHANGE, donc pas a toutes les frame (gros pb de CPU sinon)
        // donc je fais une comparaison pour l'envoyer que quand il change
        if (_pisteColorCurrent != pisteColorCurrentNew)
        {
            // Debug.Log("Invoke");
            GameEventService.OnColorPiste?.Invoke(pisteColorCurrentNew); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés
        }

        // enfin je réinitialise _pisteColorCurrent à valeur de pisteColorCurrentNew
        _pisteColorCurrent = pisteColorCurrentNew;

        // Debug.Log("_pisteColorCurrent : " + _pisteColorCurrent);

    }
}

