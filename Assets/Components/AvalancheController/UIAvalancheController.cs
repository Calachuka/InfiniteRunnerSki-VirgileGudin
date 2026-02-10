using UnityEngine;
using UnityEngine.UI;

public class UIAvalancheController : MonoBehaviour
{

    [SerializeField] private Slider _sliderBarProgressAvalanche; // je créer un champ pour lui glisser le slider qui gere ma bar de progression de l'avalanche

    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {

        GameEventService.OnCurrentAvalanche += SetAvalanche;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "SetScore" à laquelle est transmis la valeur contenu dans OnScore
    }

    // je créer un méthode SetAvalanche à laquelle je transmet la valeur de progression de mon avalanche, la formate pour l'affichage, 
    // et attribut ce résultat à _sliderBarProgressAvalanche (le slider de mon canvas)
    private void SetAvalanche(float valueAvalanche) // "Set" signifie : définir / attribuer / fixer
    {
        _sliderBarProgressAvalanche.value = valueAvalanche;
    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnCurrentAvalanche -= SetAvalanche;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "SetScore" à laquelle est transmis la valeur contenu dans OnScore
    }
}
