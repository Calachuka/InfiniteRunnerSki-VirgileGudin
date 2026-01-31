using UnityEngine;
using Component.SceneLoader;

public class UIGameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel; // créer une casse pour y glisser mon UIGameOverController qui se trouve dans ma hierarchy
    private void Start()
    {
        // je m'abonne a mon game event system (classe : static)
        GameEventService.OnGameOverState += HandleGameOver; // appelle ma methode crer en dessous
    }

    private void OnDestroy()
    {
        // je me desabonne de mon game event system (classe : static)
        GameEventService.OnGameOverState += HandleGameOver; // appelle ma methode crer en dessous
    }

    private void HandleGameOver(bool enterState)
    {
        _gameOverPanel.SetActive(enterState); // active dans ma scene mon gameobj Panel qui contient l'affichage de mon countdown
    }

    public void BackToMainMenu()
    {
        SceneLoader.LoadMainMenu(); // je vais lire "LoadMainMenu" qui est dans mon script "SceneLoader"
    }

}
