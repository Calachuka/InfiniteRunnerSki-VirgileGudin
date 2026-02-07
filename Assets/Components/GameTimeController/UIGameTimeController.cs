using TMPro;
using UnityEngine;

public class UIGameTimeController : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameTimeText; // je créer un champ pour lui glisser le TMP_Text qui gere l'affichage de mon Game time dans mon UI


    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {

        GameEventService.OnGameTime += SetGameTime;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "SetGameTime" à laquelle est transmis la valeur contenu dans OnScore
    }
    /*
    // Update is called once per frame
    void Update()
    {
        SetScore(_scoreController.ScoreValue); // je recupere la variable ScoreValue qui est dans mon script "ScoreController.cs"
    }
    */

    // je créer une methode qui ecrit le text du text mesh pro de mon canvas
    private void SetGameTime(float gameTime) // "Set" signifie : définir / attribuer / fixer
    {
        // _scoreText.text = "Score :" + score; // _scoreText.text = le composant text de l'obj _scoreText glissé dans ma case
        //_scoreText.text = "Score : " + Mathf.FloorToInt(score); // Mathf.FloorToInt() : permet d'eviter les decimales, nombre apres la virgule
        _gameTimeText.text = "Time : " + Mathf.FloorToInt(gameTime); // Mathf.FloorToInt() : permet d'eviter les decimales, nombre apres la virgule
    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnGameTime -= SetGameTime; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }

}
