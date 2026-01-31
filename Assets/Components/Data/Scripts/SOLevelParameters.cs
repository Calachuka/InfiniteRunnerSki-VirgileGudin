using UnityEngine;

namespace Components.Data
{
    [CreateAssetMenu(menuName = "Data/LevelParameters")] // sachant q'un ScriptableObject est un asset, il faut alors créer l'instance de cet asset et le nommer
    public class SOLevelParameters : ScriptableObject // ma class n'est pas un MonoBehavior mais un ScriptableObject
    {
        [SerializeField] private int _playerLife = 3; // je declare une variable _playerLife, que j'instancie a 3
        public int PlayerLife => _playerLife; // et créer un Getter pour rendre public en lecture _playerLife, permet de la laisser privé en ecriture mais de le rendre public en lecture (sécu)
    }
}
