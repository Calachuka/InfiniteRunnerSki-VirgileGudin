using TMPro;
using UnityEngine;

public class UIScoreController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText; // je créer un champ pour lui glisser le TMP_Text qui gere l'affichage de mon score dans mon UI
    // [SerializeField] private ScoreController _scoreController; // je creer une instance (ou référence) au script ScoreController que je nomme _scoreController 
                                                               // script ScoreController, qui contient la "ScoreValue"
                                                               // ATTENTTION ceci créer une case, je dois y glisser le Game Obj de ma scene contenant mon script "ScoreController.cs"
                                                               // PROBLEME ce Game Obj est dans une autre scene, je dois donc passer par le "GameEventService.cs"

    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {

        GameEventService.OnScore += SetScore;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "SetScore" à laquelle est transmis la valeur contenu dans OnScore
    }
    /*
    // Update is called once per frame
    void Update()
    {
        SetScore(_scoreController.ScoreValue); // je recupere la variable ScoreValue qui est dans mon script "ScoreController.cs"
    }
    */

    // je créer une methode qui ecrit le text du text mesh pro de mon canvas
    private void SetScore(float score) // "Set" signifie : définir / attribuer / fixer
    {
        // _scoreText.text = "Score :" + score; // _scoreText.text = le composant text de l'obj _scoreText glissé dans ma case
        //_scoreText.text = "Score : " + Mathf.FloorToInt(score); // Mathf.FloorToInt() : permet d'eviter les decimales, nombre apres la virgule
        _scoreText.text = "Score : " + score; // Mathf.FloorToInt() : permet d'eviter les decimales, nombre apres la virgule
    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnScore -= SetScore; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }

}
