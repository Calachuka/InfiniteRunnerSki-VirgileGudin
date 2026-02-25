using UnityEngine;

/// <summary>
/// Class mere de mes collectibles (POO)
/// différents collectibles :
/// - sucre d'orge (+ de score) : Verte : +50, Bleue: +100, Rouge: +200, Noire: +400
/// - cadeau de Noël : protection à la prochaine collision
/// - boule de noel : réduisant immédiatement l'avanche : -10%
/// génération d’un nouveau tronçon :
/// - un collectible de type “Sucre d’orge” doit être généré à une position aléatoire parmi les 3 positions possibles du bonus dans le tronçon.
/// - un collectible de type “Cadeau” est généré avec 25% de probabilité à une position aléatoire parmi les 3 positions possibles du bonus dans le tronçon.
/// - un collectible de type “Boule de Noël” est généré avec 50% de probabilité à une position aléatoire parmi les 3 positions possibles du bonus dans le tronçon.
/// coordonnées apparition collectible :
/// 0, 0, 0
/// 2, 0, 0
/// -2, 0, 0
/// </summary>

public abstract class CollectibleCM : MonoBehaviour
{

    // cette méthode Eated() ressence tous les points communs a tous les gameObject bouffe
    public virtual void Collected() // ajouter le mot "virtual" pour dire que cette fonction puisse etre modifiée par les script enfant
    {
        print("collecté !!!");

        // je fais apparaitre un nouveau collectible aléatoirement grace au code de ma fonction SpawnFood() qui se trouve dans mon script FoodManager.cs
        // car j'y ait definit variable static (dit : singleton) "variable static FoodManager instance;" qui peut etre appellée dans tous les script du jeu, je mets le nom du script dans lequel on se trouve "FoodManager" et je la nomme instance
        // ObstacleGenerator.instance.SpawnCollectible();

        Destroy(gameObject); // puis je detruis l'ancien fruit
    }

    // je pourrais tres bien egalement y mettre un start() et un Update(), qui serait commun a tous mes fruits

}
