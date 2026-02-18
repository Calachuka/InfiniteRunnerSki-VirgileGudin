using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public static CollectibleController instance; // variable static (dit : sigleton) qui peut etre appellée dans tous les script du jeu, je mets le nom du script dans lequel on se trouve "FoodManager" et je la nomme instance

    public List<GameObject> foodList; // liste de mes fruit qui vont etre ensuite randomisé par la fonction RandomizeFood()

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

        for (int i = 0; i < 10; i++)
        {
            SpawnFood();
        }


        // j'appel ma fonction SpawnFood() créée plus bas
        // SpawnFood();


    }


    // le but est de regarder si une position de ma surface de jeu est libre (qu'il n'y a pas de mur dessus), je vais instancier aléatoirement des instance de mon ApplePrefab  
    public void SpawnFood()
    {

        // 1) je creer mes coordonnées aléatoire dans mon espace dont j'ai definis les coordonnées
        Vector3Int posSpawn = new Vector3Int(Random.Range(-16, 17), Random.Range(-8, 10), 0);// je creer un "Vector3Int" pour avoir des valeurs entiere sans float que je nomme posSpawn
                                                                                             // avec mon carré de mesure, prendre les coordonnées en haut/gauche (dans mon cas x-16, y9) et en bas/droit (dans mon cas x16, y-8)
                                                                                             // 16 devient 17 car il veux le nombre au dessus et donc 9 devient 10

        // je convertir mon "posSpawn" qui est un Vector3Int en Vector3 car "OverLapCicle" veut un Vector3
        Vector3 worldPos = posSpawn;

        // 2) je verifie que cette position n'est pas sur un mur (sur un obj qui a un collider)
        if (Physics2D.OverlapCircle(worldPos, 0.2f) == false) // ceci grace a Physics2D.OverlapCircle (qui demande les coordonnées de mon espace de jeu qui doit etre un vector3 "posSpawn" et la taille du pointeur de verification ici une taille a 0.2f suffit
                                                              // et il va me renvoyer si oui ou non il a touché quelque chose, si c 'est false, je peux instancier mon obj
        {
            Instantiate(RandomizeFood(), worldPos, Quaternion.identity); // j'instancie un fruit randomisé (grace a la fonction RandomizeFood() créer juste en dessous) a la position worldPos, et rotation, on s'en fiche mettre alors truc de base " Quaternion.identity"
        }

        // si jamais je tombe sur un mur ou mon perso (false), je relance ma fonction SpawnFood()
        else
        {
            SpawnFood();
        }
    }

    public GameObject RandomizeFood()
    {
        return foodList[Random.Range(0, foodList.Count)];  // je fais un return nommé "FoodListe" renvoyant un random un fruit choisir dans ma "foodList", (mettre 0 PAS -1 car Random lui commence bien a compter a 1)  
    }

}
