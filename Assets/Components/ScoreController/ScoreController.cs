using UnityEngine;

public class ScoreController : MonoBehaviour
{
    // ********** BUT **********
    // chaque seconde passée sur la piste mon score augmente de 10
    // ceci de maniere continue est sans à-coup donc dans le Update
    // sachant que je commence en piste verte, à 1 minute, je passe en piste bleu, 2 minute : rouge, 3 minute : noire et jusqu’à la fin de la partie
    // point / seconde suivant les pistes : Verte : 10, Bleue : 25, Rouge : 50, Noire : 100
    // ************************

    // Je déclare une variable Score qui récupérera la valeur de mon score, je l'initialise à 0
    [SerializeField] private float _score = 0f;

    // Je déclare une variable _PisteColor qui récupérera la couleur de ma piste, je l'initialise à "Verte"
    [SerializeField] private string _pisteColorCurrent; 
    // public string PisteColor => _pisteColorCurrent; // je mon _PisteColor public c.a.d accessible seulement en lecture aux autres scripts, je nomme cette variable "PisteColor"

    [Header("Point per track color")]
    [SerializeField] private int _pointSecondPisteVerte = 10;
    [SerializeField] private int _pointSecondPisteBleu = 25;
    [SerializeField] private int _pointSecondPisteRouge = 50;
    [SerializeField] private int _pointSecondPisteNoire = 100;

    [Header("point does the track change color, in second")]
    [SerializeField] private float _timeSecondPisteVerte = 0f;
    [SerializeField] private float _timeSecondPisteBleu = 60f;
    [SerializeField] private float _timeSecondPisteRouge = 120f;
    [SerializeField] private float _timeSecondPisteNoire = 180f;



    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {

        // GameEventService.OnColorPiste += Score;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "Score" à laquelle est transmisse la valeur contenu dans OnColorPiste   
    }

    // Update is called once per frame
    void Update() // pas mettre dans FixedUpdate meme si mon score depend du temps passé, FixedUpdate() est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
    {
        GameEventService.OnColorPiste += Score;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "Score" à laquelle est transmisse la valeur contenu dans OnColorPiste
        // // j'execute la méthode TrackColor qui me dit en quelle couleur doit etre la piste
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


    // méthode qui augmente mon score en fonction de la couleur de la piste
    // ---------------------------
    private void Score(string pisteColor) // parametre qui recupere la couleur de piste actuelle, par convention le nom du parametre s'ecrit sans maj au début
    {

        // Je déclare une variable qui recuperera le nombre de point que vaut une seconde, suivant la couleur de la piste
        int pointPistePerSecond = 0; // par convention le nom d'une variable avec une maj au début, MAIS pas la car ce son des variable dites locale (déclarée dans la fonction)

        // si je suis le score s'incremente de +
        if  (pisteColor == "Verte")
        {
            pointPistePerSecond = _pointSecondPisteVerte;
            Debug.Log("Point par seconde : " + pointPistePerSecond);
        }
        else if (pisteColor == "Bleu")
        {
            pointPistePerSecond = _pointSecondPisteBleu;
            Debug.Log("Point par seconde : " + pointPistePerSecond);
        }
        else if (pisteColor == "Rouge")
        {
            pointPistePerSecond = _pointSecondPisteRouge;
            Debug.Log("Point par seconde : " + pointPistePerSecond);
        }
        else
        {
            pointPistePerSecond = _pointSecondPisteNoire;
            Debug.Log("Point par seconde : " + pointPistePerSecond);
        }

        // J'incremente mon score par le nombre de point correspondant a la couleur de la piste
        // je ne peux pas appler cette méthode dans un FixedUpdate car il est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
        _score += pointPistePerSecond * Time.deltaTime; // abreviation pour dire : _score = _score + PointPistePerSecond * Time.deltaTime;

        // Debug.Log("Score : " + _score);

        GameEventService.OnScore?.Invoke(_score); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés
        
    }


    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnColorPiste -= Score; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }

}
