using UnityEngine;

public class ColorPisteController : MonoBehaviour
{
    /// <summary>
    /// vu que dans le jeu score et avalanche dependent de la couleur de la piste
    /// je dois créer avant un script qui va me dire quelle couleur est ma piste
    /// sachant que je commence en piste verte, à 1 minute, je passe en piste bleu, 2 minute : rouge, 3 minute : noire et jusqu’à la fin de la partie
    /// point / seconde suivant les pistes : Verte : 10, Bleue : 25, Rouge : 50, Noire : 100
    /// </summary>
   


    // Je déclare une variable _PisteColor qui récupérera la couleur current de ma piste, je l'initialise à "Verte"
    [SerializeField] private string _pisteColorCurrent;
    public string PisteColorCurrent => _pisteColorCurrent; // je mon _PisteColor public c.a.d accessible seulement en lecture aux autres scripts, je nomme cette variable "PisteColor"
                                                           // ceci va me servir pour la faire récupérer par exemple, par mes prefabs de chunk, qui doivent changer de material de piste

    [Header("Time track change color, in second")]
    [SerializeField] private float _timeSecondPisteVerte = 0f;
    [SerializeField] private float _timeSecondPisteBleu = 60f;
    [SerializeField] private float _timeSecondPisteRouge = 120f;
    [SerializeField] private float _timeSecondPisteNoire = 180f;

    private bool _isGameStart = false;  // je créer une variable savoir quand quand mon jeu est lancé, pour caluler mes secondes qu'a partir de ce moment là
    private float _isGameTime = 0f;  // je créer une variable pour initialiser, mon GameTime à 0



    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {
        // je m'abonne dans le OnEnable(), car a partir de maintenant il écoute et va éxécuter Score, dès qu'il y aura un event
        GameEventService.OnGameState += GameStart; // je m'abonne à OnGameState pour savoir récupérer son bool, a changement de sa valeur j'execute GameStart en lui passant le bool
        GameEventService.OnGameTime += GameTime; // je m'abonne à OnGameState pour savoir récupérer le temps en float, a changement de sa valeur j'execute GameStart en lui passant le bool
    }


    // Update is called once per frame
    void Update() // pas mettre dans FixedUpdate meme si mon score depend du temps passé, FixedUpdate() est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
    {
        if (!_isGameStart) // tant que _isGameStarted est false
            return; // je retun au début de mon Update et ne lis pas la suite

        TrackColor();
    }


    // je suis obligé de créer une méthode, juste pour attribuer la valeur de mon bool à _isGameStart (initialisé a false plus haut)
    private void GameStart(bool isGameStart)
    {
        _isGameStart = isGameStart;
    }
    
    // je suis obligé de créer une méthode, juste pour attribuer la valeur de mon float à _isGameTime (initialisé a 0 plus haut)
    private void GameTime(float isGameTime)
    {
        _isGameTime = isGameTime;
    }


    // méthode qui me dit en quelle couleur de piste (Track) je suis
    // ---------------------------
    private void TrackColor()
    {
        // Je créer une variable qui recupere le temps depuis le debut de ma partie
        float gameTime = _isGameTime;
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
        /*
        if (_pisteColorCurrent != pisteColorCurrentNew)
        {
            // Debug.Log("Invoke");
            Debug.Log("gameTime : " + gameTime);
            GameEventService.OnColorPiste?.Invoke(pisteColorCurrentNew); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés
        }
        */
        GameEventService.OnColorPiste?.Invoke(pisteColorCurrentNew); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés

        // enfin je réinitialise _pisteColorCurrent à valeur de pisteColorCurrentNew
        _pisteColorCurrent = pisteColorCurrentNew;

        // Debug.Log("_pisteColorCurrent : " + _pisteColorCurrent);

    }
}

