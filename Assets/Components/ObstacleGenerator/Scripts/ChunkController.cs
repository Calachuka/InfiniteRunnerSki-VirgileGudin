using UnityEngine;

/// <summary>
/// ne concerne que le chunk en cours nouvellement instancier car ce script est mis directement sur mes préfabs chunk et est donc réinitialisé a chaque nouveaux chunk instancier
/// </summary>
/// 

public class ChunkController : MonoBehaviour
{
    [SerializeField] private Transform _endAnchor; //je créer une variable pour recupérer mon anchor ceci me créer une case dans mon inspector, y glisser mon anchor
                                                   // [SerializeField] permet de créer un champ dans l'inspector meme si ma variable est privée
    public Transform EndAnchor => _endAnchor; // ceci est un "Getter", ceci me permet d'autoriser les autres scipts de lire _endAnchor (créer juste au dessus) alors qu'il est privé, 
                                              // mais ils ne peuvent pas le modifier (ceci s'appelle un "getter" )
                                              // équivaut à :
                                                 /*
                                                    public Transform EndAnchor
                                                    {
                                                        get { return _endAnchor; }
                                                    }
                                                */

    public bool IsBehind => _endAnchor.position.z <= 0; // je créer un nouveau getter qui me renvoi IsBehind qui est un bool, me renvoyant true si la position de mon endAnchor en Z est <= 0, si elle est passée derriere mon perso
}
