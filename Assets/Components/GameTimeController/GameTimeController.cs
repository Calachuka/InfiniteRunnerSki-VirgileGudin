using UnityEngine;

public class GameTimeController : MonoBehaviour
{
    
    private float _gameTimer = 0f; // 1- je déclare une variable privée
    // puis je la rend public en lecture, pour que les autres scripts puissent la lire et je nomme cette varaible GameTimer
    public float GameTimer => _gameTimer;


    private bool _isGameStart = false;  // je créer une variable pour lancer mon score que quand mon jeu est lancé

    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {
        // je m'abonne dans le OnEnable(), car a partir de maintenant il écoute et va éxécuter Score, dès qu'il y aura un event
        GameEventService.OnGameState += GameStart; // je m'abonne à OnGameState pour savoir récupérer son bool, a changement de sa valeur j'execute GameStart en lui passant le bool
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameStart) // tant que _isGameStarted est false
        return; // je retun au début de mon Update et ne lis pas la suite

        _gameTimer += Time.deltaTime;
    }

    // je suis obligé de créer une méthode, pour attribuer la valeur de mon bool à _isGameStart (initialisé a false plus haut)
    private void GameStart(bool isGameStart)
    {
        _isGameStart = isGameStart;

        if (isGameStart)
        {
            _gameTimer = 0f; // reset au début de la partie
        }
    }


    // penser à se desabonner OnDestroy() c'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnGameState -= GameStart;
    }
}
