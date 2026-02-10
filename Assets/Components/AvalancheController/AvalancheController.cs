using Components.StateMachine;
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
    private float _currentAvalanche = 0;


    [Header("% de progression automatique de l'avalanche")]
    [SerializeField] private float _pourcentProgressAvalAuto = 1f; // pourcentage de progression de mon avalanche tous les n secondes

    [Header("Progression automatique toutes les n seconde")]
    [SerializeField] private float _timeAutoProgressAvalVerte = 8f;
    [SerializeField] private float _timeAutoProgressAvalBleu = 6f;
    [SerializeField] private float _timeAutoProgressAvalRouge = 4f;
    [SerializeField] private float _timeAutoProgressAvalNoire = 2f;

    [Header("Progression de l'avalanche a la collision en %")]
    [SerializeField] private float _avalColissionDamageVerte = 5f;
    [SerializeField] private float _avalColissionDamageBleu = 10f;
    [SerializeField] private float _avalColissionDamageRouge = 20f;
    [SerializeField] private float _avalColissionDamageNoire = 30f;

    // je declare ces 2 variables current comme valeur au départ du jeu
    // je déclare ces 2 variables, MAIS JE NE PEUX L'INITIALISER ICI
    // C.A.D : JE NE PEUX PAS écrire cela :
    // private float _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte;
    // CAR _timeAutoProgressAvalVerte n’est pas encore initialisé à ce niveau,
    // il sera initialisé qu'a partir du Awake ou du Start
    private float _timeAutoProgressAvalCurrent; 
    private float _avalColissionDamageCurrent;

    private bool _isGameStart = false;  // je créer une variable pour lancer mon score que quand mon jeu est lancé



    private void Start()
    {
        // j'initialise timeAutoProgressAvalCurrent dans le Start, cette variable que j'ai déclarée plus haut
        // JE NE PEUX PAS écrire cela tout en haut du script :
        // private float _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte;
        // CAR _timeAutoProgressAvalVerte; ne s'initialise qu'à partir du Awake ou du Start
        _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte; // et je l'initialise à sa valeur de départ (piste verte)
        _avalColissionDamageCurrent = _avalColissionDamageVerte; // et je l'initialise à sa valeur de départ (piste verte)
    }

    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {
        GameEventService.OnColorPiste += VariablePerPisteColor;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "VariablePerPisteColor" à laquelle est transmisse la valeur contenu dans OnColorPiste
        // GameEventService.OnCollision += IsOnCollision;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "VitesseAvanlancheAndOnCollision" à laquelle est transmisse la valeur contenu dans OnCollision
        GameEventService.OnGameState += GameStart; // je m'abonne à OnGameState pour savoir transmettre son bool, au changement de sa valeur et j'executant GameStart
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isGameStart) // tant que _isGameStarted est false
            return; // je retun au début de mon Update et ne lis pas la suite

        VitesseAvalanche(_pourcentProgressAvalAuto, _timeAutoProgressAvalCurrent); // je recupere la variable PisteColor qui est dans mon script "ScoreController.cs"      

    }

    // je suis obligé de créer une méthode, pour attribuer la valeur de mon bool à _isGameStart (initialisé a false plus haut)
    private void GameStart(bool isGameStart) 
    {
        _isGameStart = isGameStart;
    }


    // méthode qui me donne les bonnes valeurs suivant la couleur de la piste, elle prend argument la couleur de la piste
    // elle me return ces valeurs
    private void VariablePerPisteColor(string pisteColor)
    {

        if (pisteColor == "Verte")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte;
            _avalColissionDamageCurrent = _avalColissionDamageVerte;
        }
        else if (pisteColor == "Bleu")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalBleu;
            _avalColissionDamageCurrent = _avalColissionDamageBleu;

        }
        else if (pisteColor == "Rouge")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalRouge;
            _avalColissionDamageCurrent = _avalColissionDamageRouge;
        }
        else // (pisteColor == "Noire")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalNoire;
            _avalColissionDamageCurrent = _avalColissionDamageNoire;
        }
        Debug.Log("Piste : " + pisteColor);
    }


    // methode calcule la vitesse de progression de mon avalanche
    private void VitesseAvalanche(float pourcentProgressAvalAuto, float timeAutoProgressAvalCurrent)
    {
        // je dois donc calculer la vitesse de mon avanche sachant que la formule mathématique est :
        // vitesse = distance / durée (dans notre ca la distance est de 1%, toutes les 8 secondes)
        // donc 1 / 8 = 0,125%parseconde
        // je vais obtenir une vitesse exprimée en % par seconde
        // vitesse que je devrais * Time.deltaTime pour que ce soit seconde/frame
        float vitesseAutoAvalanche = 0; // je declare et initialise
        vitesseAutoAvalanche = pourcentProgressAvalAuto / timeAutoProgressAvalCurrent * Time.deltaTime; // Time.deltaTime temps ecoulé entre 2 frames, Time.deltaTime est toujours exprimé en secondes.
        Debug.Log("VitesseAval" + vitesseAutoAvalanche);
        _currentAvalanche += vitesseAutoAvalanche; // abreviation pour dire : _currentAvalanche = _currentAvalanche + vitesseAutoAvalanche;

        // je donne _currentAvalanche au GameEventService.cs, je l'Invoke
        GameEventService.OnCurrentAvalanche?.Invoke(_currentAvalanche);
    }






    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnColorPiste -= VariablePerPisteColor; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
        // GameEventService.OnCollision -= CollisionProgressAvanlanche;// je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
        GameEventService.OnGameState -= GameStart; // je m'abonne à OnGameState pour savoir récupérer son bool, a changement de sa valeur j'execute GameStart en lui passant le bool
    }



}

