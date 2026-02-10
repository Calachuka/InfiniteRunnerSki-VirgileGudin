using TMPro;
using UnityEngine;

public class UIScoreController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText; // je créer un champ pour lui glisser le TMP_Text qui gere l'affichage de mon score dans mon UI
    

    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {

        GameEventService.OnScore += SetScore;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "SetScore" à laquelle est transmis la valeur contenu dans OnScore
    }

    // je créer un méthode SetScore à laquelle je transmet le score, le formate pour l'affichage, 
    // et attribut ce résultat à _scoreText.text (le text du text mesh pro de mon canvas)
    private void SetScore(float score) // "Set" signifie : définir / attribuer / fixer
    {
        // _scoreText.text = "Score :" + score; // _scoreText.text = le composant text de l'obj _scoreText glissé dans ma case
        //_scoreText.text = "Score : " + Mathf.FloorToInt(score); // Mathf.FloorToInt() : permet d'eviter les decimales, nombre apres la virgule
        _scoreText.text = "Score : " + Mathf.FloorToInt(score); // Mathf.FloorToInt() : permet d'eviter les decimales, nombre apres la virgule
    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnScore -= SetScore; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }

}
