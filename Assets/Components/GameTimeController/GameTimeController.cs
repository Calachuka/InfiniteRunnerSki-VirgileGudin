using UnityEngine;
// ****** A FAIRE *****************
// noter les différents maniere de passer des variable d'un script a un autre service.locator, scritable obj, ...
// demander a montreal si bonne solution de m'abonner OnGameState dans mon ScoreContoller.cs pour savoir quand ma scene mon GameState commence
// et ainsi commencer a calculer le score
// n'aurais-je pas du plutot appeler le scoreContoller dans mon GameState d'une maniere ou d'une autre, mais je sais pas trop comment ?
// qu'avez vous fait, lequel vous emble le plus logique ? merci
// voici mon script ScoreContoller.cs
// ********************************
public class GameTimeController : MonoBehaviour
{
    
    private float _gameTime = 0f; // 1- je déclare une variable privée
    // puis je la rend public en lecture, pour que les autres scripts puissent la lire et je nomme cette varaible GameTimer
    public float GameTime => _gameTime;


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

        _gameTime += Time.deltaTime;

        // je donne _score au GameEventService.cs, je l'Invoke
        GameEventService.OnGameTime?.Invoke(_gameTime);
    }

    // je suis obligé de créer une méthode, pour attribuer la valeur de mon bool à _isGameStart (initialisé a false plus haut)
    private void GameStart(bool isGameStart)
    {
        _isGameStart = isGameStart;

        if (isGameStart)
        {
            _gameTime = 0f; // reset au début de la partie
        }
    }


    // penser à se desabonner OnDestroy() c'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnGameState -= GameStart;
    }
}
