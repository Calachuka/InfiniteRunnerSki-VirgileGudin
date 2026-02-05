/*
using UnityEngine;

public class AvalancheController : MonoBehaviour
{
    // ********** BUT **********
    // mon avalanche est a 100m de mon joueur
    // elle progresse vers mon joueur de 2 manieres :
    // 1) se remplit automatiquement de 1% à chaque intervalle de temps, déterminé selon la couleur de la piste :
    // Verte: 8s, Bleue: 6s, Rouge: 4s, Noire: 2s
    // 2) à chaque collision de mon Player, elle progresse vers mon Player :
    // Verte: 5%, Bleue : 10%, Rouge : 20%, Noire : 30%
    // quand elle est sur le Player c'est GAMEOVER
    // ************************

    [SerializeField] private float _avalancheDistance = 100f;
    [SerializeField] private float _colissionDamage = 10f;
    private float CurrentAvalanche = 0;


    [SerializeField] private ScoreController _scoreController; // pour récupérer la couleur de ma piste
                                                               // je creer une instance (ou référence) au script ScoreController que je nomme _scoreController 
                                                               // script ScoreController, qui contient la "PisteColor" le couleur de ma piste gérer pas sa méthode "TrackColor"
                                                               // ATTENTTION ceci créer une case, je dois y glisser le Game Obj de ma scene contenant mon script "ScoreController.cs"

    [Header("% de progression automatique de l'avalanche")]
    [SerializeField] private float _pourcentProgressAvalAuto = 1f; // pourcentage de progression de mon avalanche tous les n secondes

    [Header("Progression automatique toutes les n seconde")]
    [SerializeField] private float _timeAutoProgressAvalVerte = 8f;
    [SerializeField] private float _timeAutoProgressAvalBleu = 6f;
    [SerializeField] private float _timeAutoProgressAvalRouge = 4f;
    [SerializeField] private float _timeAutoProgressAvalNoire = 2f;

    [Header("Progression de l'avalanche a la collision en %")]
    [SerializeField] private float _progressAvalOnCollisionVerte = 5f;
    [SerializeField] private float _progressAvalOnCollisionBleu = 10f;
    [SerializeField] private float _progressAvalOnCollisionRouge = 20f;
    [SerializeField] private float _progressAvalOnCollisionNoire = 30f;

    // je declare ces 2 variables current comme valeur au départ du jeu
    private float _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte; // et je l'initialise à sa valeur de départ (piste verte)
    private float _progressAvalOnCollisionCurrent = _progressAvalOnCollisionVerte; // et je l'initialise à sa valeur de départ (piste verte)


    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {
        
        GameEventService.OnCollision += CollisionProgressAvanlanche;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "CollisionProgressAvanlanche" à laquelle est transmisse la valeur contenu dans OnCollision
        GameEventService.OnColorPiste += VariablePerPisteColor;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "VariablePerPisteColor" à laquelle est transmisse la valeur contenu dans OnColorPiste
    }


    // Update is called once per frame
    void Update()
    {

        VitesseProgressAvalanche(_pourcentProgressAvalAuto, _timeAutoProgressAvalCurrent); // je recupere la variable PisteColor qui est dans mon script "ScoreController.cs"      

    }




    // méthode qui me donne les bonnes valeurs suivant la couleur de la piste, elle prend argument la couleur de la piste
    // elle me return ces valeurs
    private float VariablePerPisteColor(string pisteColor)
    {

        if (pisteColor == "Verte")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionVerte;
        }
        else if (pisteColor == "Bleu")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalBleu;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionBleu;

        }
        else if (pisteColor == "Rouge")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalRouge;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionRouge;
        }
        else // (pisteColor == "Noire")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalNoire;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionNoire;
        }
        Debug.Log("Piste : " + pisteColor);

        return _timeAutoProgressAvalCurrent;
        return _progressAvalOnCollisionCurrent;
    }



    // methode calcule la vitesse de progression de mon avalanche
    private void VitesseAutoAvalanche(float pourcentProgressAvalAuto, float timeAutoProgressAvalCurrent)
    {
        // je dois calculer la vitesse de mon avanche sachant que la formule mathématique est :
        // vitesse = distance / durée
        // dans notre cas : vitesse = pourcentage / durée (de 1% toutes les 8 seconde) donc 1 / 8 = 0,125%parseconde
        // je vais obtenir une vitesse exprimée en % par 
        // vitesse que je devrais * Time.deltaTime pour que ce soit seconde/frame

        float vitesseAutoAvalanche = 0; // je declare et initialise
        vitesseAutoAvalanche = pourcentProgressAvalAuto / timeAutoProgressAvalCurrent * Time.deltaTime; // Time.deltaTime temps ecoulé entre 2 frames, Time.deltaTime est toujours exprimé en secondes.
        Debug.Log("VitesseAval" + vitesseAutoAvalanche);
    }



    // methode qui additionne progressAvalOnCollisionCurrent et vitesseAutoAvalanche
    private void CollisionProgressAvanlanche(progressAvalOnCollisionCurrent, vitesseAutoAvalanche)
    {
        progressTotalAvalanche = progressAvalOnCollisionCurrent + vitesseAutoAvalanche;

    }





// penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
private void OnDestroy()
    {
        GameEventService.OnColorPiste -= VariablePerPisteColor; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
        GameEventService.OnCollision -= CollisionProgressAvanlanche;// je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }



}
*/

