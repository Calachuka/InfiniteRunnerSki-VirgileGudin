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

    [SerializeField] private ScoreController _scoreController; // pour récupérer la couleur de ma piste
                                                               // je creer une instance (ou référence) au script ScoreController que je nomme _scoreController 
                                                               // script ScoreController, qui contient la "PisteColor" le couleur de ma piste gérer pas sa méthode "TrackColor"
                                                               // ATTENTTION ceci créer une case, je dois y glisser le Game Obj de ma scene contenant mon script "ScoreController.cs"

    [SerializeField] private int _pourcentProgressAvalAuto = 1; // pourcentage de progression de mon avalanche tous les n secondes

    [Header("Progression automatique de 1% de l'avalanche, toutes les n seconde" )]
    [SerializeField] private float _timeAutoProgressAvalVerte = 8f;
    [SerializeField] private float _timeAutoProgressAvalBleu = 6f;
    [SerializeField] private float _timeAutoProgressAvalRouge = 4f;
    [SerializeField] private float _timeAutoProgressAvalNoire = 2f;

    [Header("Progression de l'avalanche a la collision en %")]
    [SerializeField] private int _progressAvalOnCollisionVerte = 5;
    [SerializeField] private int _progressAvalOnCollisionBleu = 10;
    [SerializeField] private int _progressAvalOnCollisionRouge = 20;
    [SerializeField] private int _progressAvalOnCollisionNoire = 30;


    // Update is called once per frame
    void Update()
    {

        AvalancheProgress(_scoreController.PisteColor); // je recupere la variable PisteColor qui est dans mon script "ScoreController.cs"

        // j'ecoute dans mon GameEventService si il y a collision de mon player, si oui je lis la fonction "AvalancheProgress()"
        GameEventService.OnCollision += AvalancheProgress;
    }


    // je créer une methode qui calcule la progression de mon avalanche vers mon Player
    // ca progression varie en fonction de la couleur de la piste
    private void AvalancheProgress(string pisteColor)
    {
        // Je créer une variable qui recupere le temps depuis le debut du jeu
        float gameTime = Time.time; // par convention le nom d'une variable avec une maj au début, MAIS pas la car ce son des variable dites locale (déclarée dans la fonction)
        Debug.Log("Temps depuis le debut du jeu : " + gameTime);

        float _timeAutoProgressAvalNoireCurrent;

        if (pisteColor == "Verte")
        {
            // coroutine 
            yield return new WaitForSeconds(_timeAutoProgressAvalVerte);
            _timeAutoProgressAvalNoireCurrent = _timeAutoProgressAvalNoire;
        }
        else if (pisteColor == "Bleu")
        {
            // coroutine 
            yield return new WaitForSeconds(_timeAutoProgressAvalBleu);
            _timeAutoProgressAvalNoireCurrent = _timeAutoProgressAvalNoire;

        }
        else if (pisteColor == "Rouge")
        {
            // coroutine 
            yield return new WaitForSeconds(_timeAutoProgressAvalRouge);
            _timeAutoProgressAvalNoireCurrent = _timeAutoProgressAvalNoire;
        }
        else // (pisteColor == "Noire")
        {
            // coroutine 
            yield return new WaitForSeconds(_timeAutoProgressAvalNoire);
            _timeAutoProgressAvalNoireCurrent = _timeAutoProgressAvalNoire;
        }

        // l'avalanche progresse de _pourcentProgressAvalAuto
        for (int i = 1f; i <= 100f; i+ _timeAutoProgressAvalNoireCurrent) // valeur de depart de ma variable ; "i < 100" = condition de fin d'incrementation ;  "i++" =  valeur de l'incrementation
        {
            Debug.Log(i);        
        }


        _avalancheDistance += _colissionDamage; // _avalancheDistance = _avalancheDistance - _colissionDamage;
                                                // ex : 100-10=90

        // Je créer une variable qui va directement me convertir cette valeur en % de progression de ma barre avalance dans mon UIPlayer
        // elle progresse proportionellemnt de (_colissionDamage * _avalancheDistance) / 100
        float ProgressionAvanlancePourcent = (_colissionDamage * _avalancheDistance) / 100;

    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnCollision -= AvalancheProgress;
    }



}
