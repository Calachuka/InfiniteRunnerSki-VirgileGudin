using TMPro;
using UnityEngine;

// script qui va efficher le nom et score du joueur
public class UIPlayerNameController : MonoBehaviour
{

    [SerializeField] private TMP_Text _playerNameText; // je créer un casse de type pour y glisser mon composant PlayerName
    [SerializeField] private TMP_Text _playerRunCountText; // je créer un casse de type pour y glisser mon composant RunCountText
    [SerializeField] private TMP_InputField _playerNameInputField; // je créer un casse de type pour y glisser mon composant InputField

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePlayerName();
        UpdatePlayerRunCount();


    }

    public void SetPlayerName() // methode pour enregistrer le nom du joueur
    {
        var newPlayerName = _playerNameInputField.text; // je prend le texte qui est dans l'input Field

        if (string.IsNullOrEmpty(newPlayerName) ) // je fais une verif de securité pour obliger le joueur a mettre son nom
        {
            Debug.LogWarning("Player Name cannot be empty !");
            return;
        }

        // puis on vient sauvegarder le nom du joueur
        // je verifie si il existe deja une sauvegarde, afin de la conserver au cas ou 
        if(!SaveService.TryLoad(out SaveData save)) // je demande a SaveService si j'arrive a loader SaveData, le "!" de vant est un raccourcit pour dire si il est egale a false( si il n'a pas réussit a trouver de donnée suvegardées)
        {
            // si ca return false, c'est qu'il n'a pas trouvé
            save = new SaveData(); // je créer un nouvelle instance de SaveData(), du coup ca réinitialize par la meme mes données 

        }
        
        // il renvoit true, c 'est quil a trouvé
        save.PlayerName = newPlayerName;  // je créer un nouvelle ligne avec un nouveau nom de joueur

        // puis on vient sauvegarder en appelant notre methode save qui est dans notre "SaveService.cs"
        SaveService.Save(save);

        UpdatePlayerName();
    }

    public void UpdatePlayerName()
    {
        // je verifie si le fichier existe
        if (SaveService.TryLoad(out SaveData save)) // si je peux aller chercher dans SaveService.cs ma fonction TryLoad(out SaveData) c'est que le fichier.json existe (je nomme cet argument "save")
        {
            if (string.IsNullOrEmpty(save.PlayerName)) // je verifie si le nom de mon joueur est null ou vide
            {
                _playerNameText.text = "Player name not found"; // _playerNameText.text, est la variable que j'ai créer au dessus pour afficher le text dans mon text TMP
            }
            else
            {
                _playerNameText.text = save.PlayerName; // j'affiche le nom du joueur
            }

        }
        else // j'affiche a la place "fichier de sauvegarde non trouvé"
        {
            _playerNameText.text = "fichier de sauvegarde non trouvé"; // _playerNameText.text, est la variable que j'ai créer au dessus pour afficher le text dans mon text TMP
        }

    }
    public void UpdatePlayerRunCount()
    {
        // je verifie si le fichier existe
        if (SaveService.TryLoad(out SaveData save)) // si je peux aller chercher dans SaveService.cs ma fonction TryLoad(out SaveData) c'est que le fichier.json existe (je nomme cet argument "save")
        {
 
                _playerRunCountText.text = "Run count : "+ save.RunCount.ToString(); // RunCount est un int donc j'ajoute ToString(), pour l'afficher comme un string, j'affiche le nombre du run du joueur

        }
        else // j'affiche a la place "0"
        {
            _playerRunCountText.text = "Run count : 0"; // _playerNameText.text, est la variable que j'ai créer au dessus pour afficher le text dans mon text TMP
        }

    }
}
