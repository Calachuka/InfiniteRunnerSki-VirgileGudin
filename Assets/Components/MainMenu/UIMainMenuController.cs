using UnityEngine;
using Component.SceneLoader;

public class UIMainMenuController : MonoBehaviour
{
    private void Start()
    {
        /*
        var save = new SaveData(); // je créer un instance de type SaveData car en c# je ne peux pas mettre la fonction directement que je nomme "save"
        save.PlayerName = "Player"; // Tu assignes une valeur au champ PlayerName de cet objet. cet objet se trouve dans SaveData.cs

        // ecrire une donnée dans mon fichier.json
        SaveService.Save(save); // appelle la fonction "save" qui se trouve "SaveService.cs", qui ecrit mon fichier.json



        /*
        // lire une donnée de mon fichier.json
        var save = SaveService.Load(); // appelle la fonction "Load" qui se trouve "SaveService.cs", qui lit mon fichier.json
                                       // je stock le resultat dans une variable que je nomme "save"
        Debug.Log(save.PlayerName);    // puis j'affiche dans ma console la ligne "PlayerName" qui se trouve dans mon fichier et stoker dans ma variable "Save.json"  que je viens de céréer                      
        */
    }


    public void PlayGame()
    {
        // je verifie si il existe deja une sauvegarde dans mon.json, il elle n'existe pas j'en créer une vide
        // si il n'existe pas de sauvegarde precedente (le "!" devant est un raccourcit pour dire si il est egale a false)
        if (!SaveService.TryLoad(out SaveData saveData)) // ce if, essaie de charger une sauvegarde "TryLoad", TryLoad DOIT obligatoirement donner une valeur à saveData avant de se terminer
                                                         // "out" signifie : Cette méthode "TryLoad" va remplir cette variable "saveData" pour moi, saveData est le paramètre out, TryLoad écrit dedans, Même en cas d’échec, saveData reçoit une valeur
                                                         // if (!SaveService.TryLoad(out SaveData saveData))
                                                         // "SaveData" c'est le type, c.a.d la classe que tu as définie dans SaveData.cs, alors que "saveData" : est la variable
                                                         // SaveService.cs, c'est ce qui gere mes sauvegarde Json - je demande a SaveService si j'arrive a loader SaveData,
                                                         // le "!" devant inverse le bool retourné (Si TryLoad retourne false (il n'a pas trouvé de save.json) -> !false = true (donc c 'est ok j'ececute ce qu'il y a entre les {})
        {
            saveData = new SaveData(); // alors je créer un nouvelle instance de type SaveData(), "new" sert à créer un nouvel objet en mémoire
                                       // du coup il creer une nouvelle sauvegarde vide
        }

        // mainteant je sais que mon fichier existe
        saveData.RunCount ++ ; // j'incremente de 1 RunCount qui est declaré dans SaveData.cs
                               // Le . signifie : accède à un membre (RunCount) de cet objet (saveData)
                               // "saveData" est accessible ici car Il est déclaré dans le if juste avant, sinon il aurait fallut déclarer la variable avant le if
                               // comme ceci :
        /*
         SaveData saveData; // Cette ligne ne crée pas d’objet. Elle déclare juste une variable qui pourra pointer vers un objet SaveData. (SaveData = le type / la classe | saveData = la variable (référence))

         if (!SaveService.TryLoad(out saveData))
         {
             saveData = new SaveData();
         }
        */

        // puis je sauvegarde mon fichier.json
        SaveService.Save(saveData); // et j'apelle la methode "Save" qui se trouve dans SaveService.cs, je peux le faire directement car c'est une class static

        // puis je lance ma scene Level (sui lance le jeu)
        SceneLoader.LoadLevel();  // et j'apelle la methode "LoadLevel" qui se trouve dans SceneLoader.cs, elle charge ma scene "Level" et "LevelUI", je peux le faire directement car c'est une class static
    }


    public void QuitGame()
    {
        // ATTENTION Application.Quit(); NE MARCHE PAS dans l'editor Unity, je dois donc faire une CONDITIONNAL COMPILATION
        // c.a.d mettre une condition de plus haut niveau qui s'écrit comme ceci #if, #else, #endif
#if UNITY_EDITOR // si je suis dans editor Unity
        UnityEditor.EditorApplication.isPlaying = false; // ATTENTION Application.Quit(); NE MARCHE PAS dans l'editor Unity, je dois donc mettre cette ligne

#else // sinon... je suis en build, mon jeu est jouable par tous, Application.Quit(); NE MARCHE
                Application.Quit(); 
        
#endif
    }

}
