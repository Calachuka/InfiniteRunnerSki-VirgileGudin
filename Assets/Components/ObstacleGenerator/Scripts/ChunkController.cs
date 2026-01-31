using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private Transform _endAnchor; //je créer un variable pour mon anchor ceci me créer une case dans mon inspector, y glisser mon anchor
                                                   // [SerializeField] permet de créer un champ dans l'inspector meme si ma variable est privée
    public Transform EndAnchor => _endAnchor; // ceci est un "Getter", ceci me permet d'autoriser les autres scipts de lire _endAnchor (créer juste au dessus) alors qu'il est privé, 
												// mais ils ne peuvent pas le modifier (ceci s'appelle un "getter" )

    public bool IsBehind => _endAnchor.position.z <= 0; // je créer un nouveau getter qui me renvoi IsBehind qui est un bool, me renvoyant true si la position de mon endAnchor en Z est <= 0, si elle est passée derriere mon perso
}
