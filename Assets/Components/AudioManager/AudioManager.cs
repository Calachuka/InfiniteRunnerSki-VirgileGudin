using UnityEngine;

/// <summary>
/// permet de gérer tous les sons de mon jeu :
/// dans ma hierachy
/// 1- créer un Obj Empty le nommer "AudioManager"
/// 2- lui assigner ce script du meme nom
/// 3- A ce Obj Empty nommer "AudioManager", lui ajouter un componant audio source
/// 4- dans l'inspector de mon Obj Empty nommer "AudioManager", lui glisser dans sa case "AudioSource" du script, le composant audio source que je viens de créer
/// 5- je n'ai plus qu'a glisser dans les cases de l'inspector de mon Obj Empty nommer "AudioManager" 
///     les clip audios qui se trouve dans mes assets
/// </summary>

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // sert à créer ce qu’on appelle un Singleton simple (car Instance est static) vu que Grâce à ça, n’importe quel script peut faire : AudioManager.Instance.PlayCollectible();

    [Header("Audio Source")]
    [SerializeField] private AudioSource _audioSource; // je glisse mon component audio source dedans

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _obstacleClip; // je glisse mon clip audio pour les obstacles
    [SerializeField] private AudioClip _collectibleClip; // je glisse mon clip audio pour les collectibles
    

    private void Awake()
    {
        // sert à créer ce qu’on appelle un Singleton simple (car Instance est static) vu que Grâce à ça, n’importe quel script peut faire : AudioManager.Instance.PlayCollectible();
        Instance = this; // Le script AudioManager est initialisé à l'awake
    }

    private void OnEnable() // "OnEnable()" est lu avant le "Update()"
    {
        GameEventService.OnCollisionObstacle += PlayObstacle;  // je m'abonne à mon GameEventService.cs / OnCollisionObstacle et j'exécute la fonction "PlayObstacle" à laquelle est transmisse la valeur contenu dans OnCollisionObstacle
        GameEventService.OnCollisionCollectible += PlayCollectible; // je m'abonne à mon GameEventService.cs / OnCollisionCollectible et j'exécute la fonction "PlayCollectible" à laquelle est transmisse la valeur contenu dans OnCollisionCollectible
    }

    public void PlayObstacle()
    {
        _audioSource.PlayOneShot(_obstacleClip);
    }

    public void PlayCollectible()
    {
        _audioSource.PlayOneShot(_collectibleClip);
    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnCollisionObstacle -= PlayObstacle; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
        GameEventService.OnCollisionCollectible -= PlayCollectible;// je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }
}