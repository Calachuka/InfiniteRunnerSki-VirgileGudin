using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// son but générer en random des portions a l'infini et les faire translater
/// </summary>



public class ObstacleGenerator : MonoBehaviour
{
    [Header("Prefabs chunk portionObstacle ")]
    // ----- explication de cette ligne
    // [SerializeField] private GameObject[] _ChunkPrefabs; // variable et champ pour indiquer PortionObstaclePrefab, il va y en avoir plusieur donc [] pour en faire un array
    // ensuite vu que c'est un prefab je devrait faire  _potionPrefab.GetComponent pour recupérer son script
    // il existe une methode qui permet de le recupérer directement, a la place de "GameObject", mettre directement le nom du script que je veux récupérer ChunkController
    // et glisser dans la case apparut de dans l'inspector, le prefab contenant le script ChunkController.cs, prefab qui n 'est pourtant pas encore instancier dans ma scene 
    [SerializeField] private ChunkController[] _chunkPrefabs; // array de mes chunks, avantage m'evite de faire par la suite _potionPrefab.GetComponent pour recupérer le script et plus besoin de mettre de script ChunkController.cs a mes autres prefabs

    [Header("Parameters")]
    [SerializeField] private float _translationSpeed = 1f; // vitesse de translation
                                                            // [SerializeField] permet de créer un champ dans l'inspector meme si ma variable est privée
    [SerializeField] private int _activeChunksCount = 5; // créer une varible int pour le nombre de chunk que je veux devant moi
    [SerializeField] private int _behindChunksCount = 2; // créer une varible int pour le nombre de chunk que je veux derriere moi (behind = derrière)
    [SerializeField] private bool _preventSameChunkGeneration = true; // créer une varible bool pour activer ou pas mon amelioration de random

    // Declaration de la variable _activeChunks...
    // Liste qui contiendra tous les chunks actuellement actifs dans la scène
    // List = taille dynamique (contrairement à un tableau)
    // Ici, on dit au programme :
    // "Il existera une variable appelee _activeChunks
    // capable de contenir une liste de ChunkController"
    //
    // A ce stade :
    // - Aucune liste n est creee en memoire
    // - _activeChunks vaut null
    // - Il n y a rien a l interieur
    private List<ChunkController> _activeChunks;

    // je veux recupérer le dernier element de la liste _activeChunks, me permettera ensuite de recupérer son anchor et y coller le chunk prochain dessus
    //
    // Explication detaillee :
    // - _activeChunks.Count retourne le nombre total d elements dans la liste
    // - Count - 1 correspond a l index du dernier element (les index commencent a 0)
    // - _activeChunks[Count - 1] permet donc d acceder au dernier chunk ajouté
    // Attention :
    // - Si _activeChunks est vide, cette ligne provoquera une erreur
    // => est une sorte de raccourcis (pour remplacer un getter) qui revois directement la valeur du calcul (ce calcule est une methode), de plus celle-ci est partageble entre mes sript comme celui de dans mon script "ChunkController.cs"
    // ca aurait pu secrire comme cela en version longue
    /*
	private ChunkController LastChunk
    {
        get
         {
            return _activeChunks[_activeChunks.Count - 1];
         }
    }
    */
    // Quand une propriete ou une methode :
    // ne fait qu une seule instruction
    // retourne une valeur directement
    // Alors => est parfait, ca simplifie la lecture du code
    private ChunkController LastChunk => _activeChunks[_activeChunks.Count - 1];

    private int _lastChunkIndex = 0; // je créer une variable pour recuperer le dernier nun de chunk instantiate // pour que dans le random il n' y ai pas 2 fois de suite le meme chunk

    private bool _enabled; // je declare une variable de type bool nommée _enabled, pas besoin de l'initialiser a false, car par default un bool est initialisé a false

    private void Start()
    {
        // Initialisation de la liste _activeChunks
        // Ici, on cree reellement une nouvelle liste vide en memoire
        // et on assigne cette liste a la variable _activeChunks
        //
        // A partir de maintenant :
        // - _activeChunks n est plus null
        // - La liste existe vraiment
        // - On peut ajouter, retirer et parcourir des ChunkController
        _activeChunks = new List<ChunkController>();// Initialisation de la liste
                                                    // tu creer une nouvelle liste vide pour stocker tous les chunks actifs dans ta scene.
                                                    // _activeChunks est de type List<ChunkController> : c'est une structure qui peut grandir ou retrecir dynamiquement.
                                                    // Si tu ne l'initialises pas, _activeChunks serait null et ton code planterait des que tu ferais foreach.

        AddBaseChunks(); // méthode (ou fonction) qui gère la création des chunks au lancement (au start)

        // je créer une class pour m'abonner a la methode "HandleGameState" créee en bas
        GameEventService.OnGameState += HandleGameState;
    }

    // je me suis abonner juste au dessus donc je DOIS me desabonner
    private void OnDestroy()
    {
        GameEventService.OnGameState -= HandleGameState;
    }

    // je créer qui dit que c'est enabled si je rentre dans le State Enter()
    private void HandleGameState(bool enterState) // si _enabled = false, je sors du state
    {
        _enabled = enterState; 
    }

    // gere la position d'instantiation de mes chhunk
    private void AddBaseChunks()
    {
        // faire en sorte que mes chunk s'instantie a la position de l'anchor du précèdent
        for (int i = 0; i < _activeChunksCount; i++)
        {
            if (i == 0) // sauf, si c'est mon premier chunk, donc 0, lui seulement je le met a l'origine du monde donc en Vector3.zero
            {
                AddChunk(Vector3.zero);
                continue; // permet de lire 
            }

            AddChunk(LastChunk.EndAnchor.position); // lastChunk, c’est le dernier chunk déjà instancié dans la liste _activeChunks.
                                                    // lastChunk.EndAnchor.position, c’est la position de l’anchor de fin de ce dernier chunk.
                                                    // EndAnchor : est déclaré dans notre script "ChunkController.cs" je le recupere ici
                                                    // AddChunk(...),  instancie un nouveau chunk à cette position.
                                                    // "AddChunk" execute la fonction du meme nom ecrite juste en dessous qui permet d'ajouter un chunk

        }

    }

	//------------------------
    // ajouter un chunk
	//------------------------
    private void AddChunk(Vector3 position)
    {

        // j'ameiore mon system de random pour pas qu'il y ai pas 2 fois de suite le meme chunk
        var newChunkIndex = Random.Range(0, _chunkPrefabs.Length); // en random de entre 0 et la longueur de ma liste "_chunkPrefabs.Length"

        if (_preventSameChunkGeneration) // ce if verifie si j'ai bien coché cette amerioration dans mon inspector
        {
            for (int i = 0; i < 10; i++) // je lui donne 10 chance de random pour tenter d'avoir un nombre différent du precedant
            {
                if (newChunkIndex == _lastChunkIndex)
                {
                    newChunkIndex = Random.Range(0, _chunkPrefabs.Length);
                }
            
            }

            _lastChunkIndex = newChunkIndex; // je recupere maintenant la derniere index qui vient d 'entre generé
        }






        // ChunkController chunk = Instantiate(_chunkPrefabs[0], transform); // version si je veux juste en instantion 'une seule sorte 
        ChunkController chunk = Instantiate(_chunkPrefabs[newChunkIndex], transform);  // je les instantie mes chunk de ma liste "_chunkPrefabs" ceci grace au random de "newChunkIndex" créer plus haut
                                                                                       // on stock le résultat de l'instantialtion dans une variable "ChunkController" devient ceci
                                                                                       // Crée une nouvelle instance du prefab de chunk (le premier du tableau _chunkPrefabs)
                                                                                       // "transform" seul permet qu'il devient enfant de l'obj courant ChunkController
                                                                                       // puis stocke la référence dans la variable "chunk"

        chunk.transform.position = position; // Positionne le chunk instancie a l endroit specifie par la variable "position"
                                             // "transform" fait reference a la position, rotation et echelle de l objet dans la scene
                                             // En assignant ".position", on deplace l objet a la position precise souhaitée

        _activeChunks.Add(chunk);// Ici, tu ajoutes le chunk que tu viens d'instancier dans ta liste _activeChunks.
                                 // c'est essentiel car dans Update(), tu as un foreach :   
                                 //Si tu ne l'ajoutes pas a la liste, _activeChunks reste vide et la boucle ne fera rien, donc ton prefab ne bougera jamais.

    }

    private void Update()
    {
        // controle si je lance le defilement de mes chunk
        // si ma variable _enabled qui est declarée en entete, est différent de false donc true, on return, c.a.d je ne lis pas la suite du code
        if (!_enabled) 
        { 
            return; 
        }

        foreach (ChunkController chunk in _activeChunks)
        {
            // Déplace l'objet "chunk" vers l'arrière (axe Z négatif)
            // à une vitesse définie par _translationSpeed,
            // en tenant compte du temps écoulé entre deux frames (Time.deltaTime), rend le mouvement indépendant du framerate
            chunk.transform.Translate((Vector3.back * _translationSpeed * Time.deltaTime));

        }

        UpdateChunks(); // <-- appel ici

    }

    // méthode qui va me permettre de générer automatiquement les nouveau chunks
    // sachant que je peux toujours 5 chunks actif et que un chunk considérer comme behinds est un chunk qui est passéee derriere le player
    // donc si sa position en Z de son endAnchor est inférieur a 0 est passée dernier le perso
    // elle va etre appelée dans Update car je dois verifier a chaque frame si je dois ajouter un nouveau chunk
    // mais je dois la créer en dehors de Update, car dans Update, je ne peux pas directement créer une methode (fonction eu ensemnle de fonction)
    private void UpdateChunks() // je la nomme Update... car elle va etre applée dans l'update, c'est visuel ca permet de s'y reperer plus vite
    {
        // 1) Créer une liste temporaire pour stocker les chunks qui sont derrière le joueur
        List<ChunkController> behindChunks = new();

        // 2) Parcourir tous les chunks actifs
        foreach (var chunk in _activeChunks)
        {
            // Vérifie si le chunk est derrière le joueur
            // IsBehind est une propriété de ChunkController qui retourne true si le chunk est passé derrière le joueur
            if (chunk.IsBehind)
            {
                // Si le chunk est derrière, on l'ajoute à la liste temporaire behindChunks
                behindChunks.Add(chunk);
            }
        }

        // 3) Calculer combien de chunks derrière doivent être détruits
        int chunkToDeleteCount = behindChunks.Count - _behindChunksCount;

        // On ne détruit des chunks que si on a plus de chunks derrière que le nombre autorisé
        if (chunkToDeleteCount > 0)
        {
            for (int i = 0; i < chunkToDeleteCount; i++)
            {
                // Récupère le chunk à supprimer depuis la liste behindChunks
                var chunkToDelete = behindChunks[i];

                // Retire le chunk de la liste des chunks actifs
                _activeChunks.Remove(chunkToDelete);

                // Détruit le GameObject du chunk pour le supprimer de la scène
                Destroy(chunkToDelete.gameObject);
            }
        }

        // 4) Vérifier combien de chunks doivent être ajoutés pour atteindre le nombre total souhaité
        // _activeChunksCount est le nombre total de chunks que l'on veut avoir devant le joueur
        int missingChunkCount = _activeChunksCount - _activeChunks.Count;

        // 5) Ajouter les chunks manquants
        for (int i = 0; i < missingChunkCount; i++)
        {
            // AddChunk instancie un nouveau chunk à la position de l'ancre de fin du dernier chunk actif
            AddChunk(LastChunk.EndAnchor.position);
        } 
    }

}
