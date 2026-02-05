using UnityEngine;

namespace Test
{
    public class ScoreController : MonoBehaviour
    {
        // ********** BUT **********
        // chaque seconde passée sur la piste mon score augmente
        // ceci de maniere continue et sans à-coup donc dans le Update
        // sachant que je gagne 10 point / seconde si je suis en piste Verte, 25 en piste Bleue, 50 en piste Rouge, 100 en piste Noire
        // ************************

        // Je déclare une variable Score qui récupérera la valeur de mon score, je l'initialise à 0
        [SerializeField] private float _score = 0f; // je declare mon score et l'initialise à 0

        [Header("Point per track color")]
        [SerializeField] private int _pointSecondPisteVerte = 10;
        [SerializeField] private int _pointSecondPisteBleu = 25;
        [SerializeField] private int _pointSecondPisteRouge = 50;
        [SerializeField] private int _pointSecondPisteNoire = 100;

        private int _pointSecondPisteCurrent; // je déclare ma variable _pointSecondPisteCurrent MAIS JE NE PEUX L'INITIALISER ICI
                                              // C.A.D : JE NE PEUX PAS écrire cela :
                                              // private int _pointSecondPisteCurrent = _pointSecondPisteVerte;
                                              // CAR _pointSecondPisteVerte n’est pas encore initialisé à ce niveau,
                                              // il sera initialisé qu'a partir du Awake ou du Start



        private void OnEnable() // "OnEnable()" est lu avant le "Update()"
        {
            // je m'abonne dans le OnEnable(), car a partir de maintenant il écoute et va éxécuter Score, dès qu'il y aura un event
            GameEventService.OnColorPiste += VariablePerPisteColor;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "Score" à laquelle est transmisse la valeur contenu dans OnColorPiste   
        }


        void Start()
        {
            _pointSecondPisteCurrent = _pointSecondPisteVerte; // j'initialise _pointSecondPisteCurrent dans le Start, cette variable que j'ai déclarée plus haut
                                                               // JE NE PEUX PAS écrire cela (tout en haut du script) :
                                                               // private int _pointSecondPisteCurrent = _pointSecondPisteVerte;
                                                               // CAR _pointSecondPisteVerte ne s'initialise qu'à partir du Awake ou du Start
        }

        // Update is called once per frame
        void Update() // pas mettre dans FixedUpdate meme si mon score depend du temps passé, FixedUpdate() est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
        {

            // VariablePerPisteColor(); // j'execute la méthode VariablePerPisteColor qui me return la valeur de la variable suivant la couleur de la piste
            Score(_pointSecondPisteCurrent); // j'execute la méthode Score, et lui donne comme argument _pointSecondPisteCurrent
        }



        // méthode qui me donne le nombre de points a ajouter, suivant la couleur de la piste, (elle prend argument la couleur de la piste)
        // --------------------------------------
        private void VariablePerPisteColor(string pisteColor)
        {
            if (pisteColor == "Verte")
            {
                _pointSecondPisteCurrent = _pointSecondPisteVerte;
            }
            else if (pisteColor == "Bleu")
            {
                _pointSecondPisteCurrent = _pointSecondPisteBleu;
            }
            else if (pisteColor == "Rouge")
            {
                _pointSecondPisteCurrent = _pointSecondPisteRouge;
            }
            else // (pisteColor == "Noire")
            {
                _pointSecondPisteCurrent = _pointSecondPisteNoire;
            }
            Debug.Log("Piste : " + pisteColor);
            Debug.Log("_pointSecondPisteCurrent : " + _pointSecondPisteCurrent);
        }



        // méthode qui augmente mon score
        // ---------------------------
        private void Score(int pointSecondPisteCurrent) // parametre qui recupere la couleur de piste actuelle, par convention le nom du parametre s'ecrit sans maj au début
        {

            // J'incremente mon score par le nombre de point correspondant a la couleur de la piste
            // je ne peux pas appler cette méthode dans un FixedUpdate car il est réservé à la physique, utiliser "Time.deltaTime" pour compenser cela
            _score += pointSecondPisteCurrent * Time.deltaTime; // abreviation pour dire : _score = _score + PointPistePerSecond * Time.deltaTime;

            // Debug.Log("Score : " + _score);

            GameEventService.OnScore?.Invoke(_score); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés

        }


        // penser à se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
        private void OnDestroy()
        {
            GameEventService.OnColorPiste -= VariablePerPisteColor; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
        }

    }
}