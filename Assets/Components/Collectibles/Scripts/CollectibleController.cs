using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] private ChunkController[] _chunkPrefabs; // créer une instance de ChunkController.cs pour récupérer sa variable publique "EndAnchor" dans un array, des chunk en cours
                                                                // et je e
    public static CollectibleController instance; // ce créer un sigleton qui peut etre appellée dans tous les script du jeu, je mets le nom du script dans lequel on se trouve "CollectibleController" et je la nomme instance

    public List<GameObject> collectibleList; // liste de mes Collectibles qui vont etre ensuite randomisé par la fonction RandomizeCollectible()

    public List<Vector3> positionCollectibleList; // liste de mes Position qui vont etre ensuite randomisé par la fonction RandomizePositionCollectible()

    private void Start()
    {


        // je verifie si ma variable static (sigleton) "instance" existe et la crée si elle n'existe pas
        // si l'instance n'existe pas 
        if (instance == null)
        {
            instance = this; // je la créer
        }
        else // si instance n'est pas null c'est qu'il a eu un pb, je ne suis pas censé exister, je me destroy
        {
            Destroy(this);
        }

        // j'appel ma fonction SpawnFood() créée plus bas
        // je l'execute 1 fois, pour qu'il y ai 1 instances de collectible pas chunk
        for (int i = 0; i < 1; i++)
        {
            SpawnCollectible(); // j'appel ma fonction SpawnFood() créée plus bas
        }
    }

    // le but est d'instancier aléatoirement une des instances de collectible listée plus haut à aléatoirement une des postion coordonnées aussi listée plus haut
    public void SpawnCollectible()
    {
        Instantiate(RandomizeCollectible(), RandomizePositionCollectible(), Quaternion.identity); // j'instancie un collectible randomisé (grace a la fonction RandomizeCollectible() créer juste en dessous) a la position RandomizePositionCollectible(), et rotation, on s'en fiche mettre alors truc de base " Quaternion.identity"
    }
    
    
    public GameObject RandomizeCollectible()
    {
        return collectibleList[Random.Range(0, collectibleList.Count)];  // sachant que Instantiate (fonction executée apres) a besoin d'une variable de type GameObject qui se trouve dans ma liste
                                                                         // et pour designer une ligne de liste ca s'ecrit par ex comme ceci : collectibleList[2]
                                                                         // je Génère une ligne avec un num random
                                                                         // dans Random.Range mettre 0, car il va générer un nombre entre : 0 inclus et Count exclus, Donc si Count = 5. Les index possibles seront : 0, 1, 2, 3, 4
    }

    public Vector3 RandomizePositionCollectible()
    {
        return positionCollectibleList[Random.Range(0, positionCollectibleList.Count)];  // sachant que Instantiate (fonction executée apres) a besoin d'un Vector3 pour la position du GameObject a instancier
                                                                                         // et pour designer une ligne de liste ca s'ecrit par ex comme ceci : positionCollectibleList[2]
                                                                                         // je Génère une ligne avec un num random
                                                                                         // dans Random.Range mettre 0, car il va générer un nombre entre : 0 inclus et Count exclus, Donc si Count = 5. Les index possibles seront : 0, 1, 2, 3, 4
    }


}
