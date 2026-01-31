using System;
using System.IO;
using UnityEngine;

public static class SaveService // en faire une class static
{
    private const string FILE_NAME = "save.json"; // je créer un constante, pour le nom de mon fichier de sauvegarde de données "save.json", car il ne changerara jamais , je la nomme FILE_NAME (en MAJ car c'est un constante)
    private static string FilePath => Path.Combine(Application.persistentDataPath, FILE_NAME); // je créer un getter (sorte de raccourcis pour executer directement une fonction (ou methode)
                                                                                               // Application.persistentDataPath, fonction qui me renvoit un chemin vers lequel je pourrais toujours ecrire mon fichier, et ceci quelque soit le device ou l'ordi de la personne MAGIQUE !!!
                                                                                               // Path.Combine, fonction qui permet de combiner un chemin avec plusieur variable


    // écrire des données dans mon fichier.json
    // -----------------
    public static void Save(SaveData saveData) // créer des méthode static car je suis dans une class static comme argument je lui dit qu'il dois recevoir une var de type "SaveData" que je vais nommer "saveData"
    {

        string json = JsonUtility.ToJson(saveData); // JsonUtility (librairie) qui en appelant sa Fonction .ToJson, a laquelle je donne comme argument ma variable "saveData", il me renvoit un string que je stock dans la variable nommée par ex "json"

        // fonction qui permet de créer le fichier et d'y mettre la donnée
        // si un fichier existe dejà il est complètement écrasé.
        // File.WriteAllText("SaveDataGame/Save.json", "Hello world !"; // autre exemple, chemin vers le fichier en dur et ecris directement ce texte "Hello world !" dans le fichier
        File.WriteAllText(FilePath, json); // File.WriteAllText(); permet d'ecrire et cérer un fichier,
                                           // il demande path (le chemin par defaut il s'enregiste a la racine des fichiers de mon jeu) ou enregistrer et le content (la donnée) ici un string "json"
        Debug.Log("Save player data at " + FilePath);
    }


    // lire les données qui sont sur mon fichier.json
    // -----------------
    public static bool TryLoad(out SaveData saveData) // verifiez si le fichier.json existe, si oui me le stoker
                                                      // grace a cett methode particuliere qui combiné au "out" renvoie un bool (true si le composant est trouvé) et qui permet en meme temps de recupérer ce composant qui grace au "out" permet de stoker une variable de retour directement dans les argument (choses entre parenthe)
                                                      // dans notre cas il va nous renvoyer "SaveData" si il existe
                                                      // je suppose que'on aurait pu le faire en plusieurs étapes (verifier si il exite, puis aller le lire) mais que celle-ci le fait directement ?
    {
        string json;

        // juste pour affichier le texte que le fichier n'existe pas
        // je vais encapsuler mon code, dans un "try catch" outil a utiliser dans des zones qui ne sont pas safe (pour les quelles on est pas sur du résultat) pour faire des test dans la console unity
        try // il essait de lire
        {
            json = File.ReadAllText(FilePath); // File.ReadAllText(); permet de lire un fichier, il demande comme argument le path, le chemin du fichier a lire
        }
        catch (Exception e) // si il y arrive pas il renvoie un erreur contenue dans "e"
        {
            Debug.LogError("Unable to read the save file ! - detail : " + e ); // afficher cette erreur dans la console "Impossible de lire le fichier de sauvegarde ! " et afficher l'erreur "e"
            saveData = null; 
            return false;
        }

        

        if (string.IsNullOrEmpty(json)) // j'ajoute une secu pour vérifier si la valeur de la variable json en mémoire == NULL si la chaine est vide ""
                                        // NE vérifie PAS si un fichier JSON existe, vérifie uniquement la valeur de la variable json en mémoire.
                                        // distinction entre la variable "json" en mémoire et l’existence du fichier
                                        // IsNullOrEmpty renvoie true si la chaîne est null ou si la chaine est vide ""
                                        // Pourquoi “string” est obligatoire : IsNullOrEmpty est une méthode statique de la classe string
                                        // En C#, string (ou System.String) est une classe, pas juste un mot clé, IsNullOrEmpty n’existe que dans cette classe.
                                        // C# n’a pas de “fonction globale” IsNullOrEmpty, Tout est toujours dans une classe en C#
                                        // Alternative si tu ne veux pas écrire string. tout le temps, Tu pourrais créer ta propre fonction helper
                                        /*
                                            bool IsNullOrEmpty(string s) => string.IsNullOrEmpty(s);

                                            // puis l'utiliser
                                            if (IsNullOrEmpty(json))
                                            {
                                                ...
                                            }
                                            // Mais derrière, ça fait toujours appel à string.IsNullOrEmpty.
                                        */
        {

            Debug.LogError("No save data found at path : " + FilePath); // si oui j'affiche cette erreur dans ma console
            saveData = null;
            return false;

        }  
            var result = JsonUtility.FromJson<SaveData>(json);  // JsonUtility (librairie) qui en appelant sa fonction .FromJson, a la quelle je donne (json) me créer un Obj de type <SaveData> que stock dans ma variable "result"
        
        saveData = result;
        return true;
    }

}
