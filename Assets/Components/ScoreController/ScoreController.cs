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
    public float ScoreValue => _score; // je mon _score public c.a.d accessible seulement en lecture aux autres scripts, je nomme cette variable "ScoreValue"

    // Je déclare une variable _PisteColor qui récupérera la couleur de ma piste, je l'initialise à "Verte"
    [SerializeField] private string _PisteColor = "Verte";
    public string PisteColor => _PisteColor; // je mon _PisteColor public c.a.d accessible seulement en lecture aux autres scripts, je nomme cette variable "PisteColor"

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

 


    // Update is called once per frame
    void Update() // pas mettre dans FixedUpdate meme si mon score depend du temps passé, FixedUpdate() est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
    {
        Score(TrackColor()); // j'execute ma méthode "Score", qui a besoin d'un parametre string qui est retourné par ma méthode "TrackColor()"
                             // donc autant lui passer directement en parametre la "TrackColor()"

        // j'appelle "OnCollision" (qui me dit si collision ou pas) qui sur mon eventSystem (de type classe static)
        // je n'ai pas besoin d'appler cette classe plus haut car elle existe partout donc elle est directement accessible
        GameEventService.OnCollision?.Invoke(); // j'applle "OnCollision" qui est sur mon script "GameEventSystem.cs" et sa valeur "OnCollision", "?" est neccessaire pour faire une erreur si personne n'est abonné à "OnCollision" | "Invoke" = appeler, déclencher
    }


    // méthode qui me dit en quelle couleur de piste (Track) je suis
    // ---------------------------
    private string TrackColor() // je mets "string" et non pas "void", car ca ne return rien "string", me permet de stocker un string car je fais un return a la fin
    {
        // Je créer une variable qui recupere le temps depuis le debut du jeu
        float gameTime = Time.time; // par convention le nom d'une variable avec une maj au début, MAIS pas la car ce son des variable dites locale (déclarée dans la fonction)
        Debug.Log("Temps depuis le debut du jeu : " + gameTime);

        if (gameTime < _timeSecondPisteBleu)
        {
            Debug.Log("Piste Verte");
            _PisteColor = "Verte";
        }
        else if (gameTime < _timeSecondPisteRouge)
        {
            Debug.Log("Piste Bleu");
            _PisteColor = "Bleu";
        }
        else if (gameTime < _timeSecondPisteNoire)
        {
            Debug.Log("Piste Rouge");
            _PisteColor = "Rouge";
        }
        else // sinon je suis en piste noire
        {
            Debug.Log("Piste Noire");
            _PisteColor = "Noire";     
        }

        Debug.Log("Piste : " + _PisteColor);
        return _PisteColor;
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
        // _score += PointPistePerSecond; // abreviation pour dire : _score = _score + PointPistePerSecond;
        // je ne peux pas appler cette méthode dans un FixedUpdate car il est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
        _score += pointPistePerSecond * Time.deltaTime; // abreviation pour dire : _score = _score + PointPistePerSecond * Time.deltaTime;
        Debug.Log("Score : " + _score);
    }

}
