using Components.Data;
using TMPro;
using UnityEngine;

public class UILifeController : MonoBehaviour
{
    [SerializeField] private SOLevelParameters _levelParameters; // je créer un champ pour lui glisser le paramettre que je souhaite, de mon ScriptableOject
    [SerializeField] private TMP_Text _lifeText;

    private void Start()
    {
        SetLife(_levelParameters.PlayerLife); // je transmet _levelParameters.PlayerLife à ma fonction "SetLife". SetLife methode definit plus bas, qui me sert a afficher le nombre de vie
        // a mon start, je m'abonne a OnCollision (dans mon eventsystem qui est classe static)
        GameEventService.OnPlayerLifeUpdated += SetLife; // += c'est un delegat, pour executer une methode (fonction) HandleCollision que j'ai ecrite plus bas
    }

    // ATTENTION toujours se désabonner à la fin du jeu
    private void OnDestroy() // quand ce script est detruit (c 'est la fin du jeu), en dessous je me desabonne
    {
        GameEventService.OnPlayerLifeUpdated -= SetLife; // -= c'est un delegat, je me désabonne
    }

    // je créer une methode qui ecrit le text du text mesh pro de mon canvas
    private void SetLife(int life) 
    {
       _lifeText.text = "life :"+ life;
    }
}
