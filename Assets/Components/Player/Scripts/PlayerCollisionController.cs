using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [Header("Sphere Overlap parameters (collider)")]
    [SerializeField] private Vector3 _sphereStandCenter; // position de ma sphere, quand mon player est debout (deplacement et saut), parametre dont a besoin "Physics.OverlapSphere()" 
    [SerializeField] private float _sphereStandRadius; // Radius de ma sphere, quand mon player est debout (deplacement et saut), parametre dont a besoin "Physics.OverlapSphere()"

    [SerializeField] private Vector3 _sphereShrinkCenter; // position de ma sphere, quand mon player se baisse (slideDown), parametre dont a besoin "Physics.OverlapSphere()" 
    [SerializeField] private float _sphereShrinkRadius; // Radius de ma sphere, quand mon player se baisse (slideDown), parametre dont a besoin "Physics.OverlapSphere()"

    [Header("Debug")]
    [SerializeField] private bool _isHit; // je créer un bool pour verifier quand je suis en colision et quand je n'y suis plus
    [SerializeField] private Vector3 _sphereCenter; // 
    [SerializeField] private float _sphereRadius; // 

    private readonly Collider[] _hitResults = new Collider[1]; // Tu crées un tableau réutilisable pour Physics.OverlapSphereNonAlloc
                                                               // Il peut contenir au maximum 1 résultat
                                                               // car "OverlapSphereNonAlloc" demande un array préalloué dans lequel les colliders détectés seront écrits. Ici je limite volontairement à 1 résultat maximum.
                                                               // var de type "Collider", représente un composant physique attaché à un GameObject (utilisé par le moteur physique pour les collisions et détections.) c'est pour recupérer les collider de mes gameObject Obstacles qui eux en ont un
                                                               // "readonly" signifie que la référence du tableau ne peut plus être réassignée après l’initialisation, mais son contenu peut être modifié

    private void Start() // au demarrage du jeu mon personnage est debout, donc je lui attribut son detecteur de collision en possition debout
    {
        _sphereCenter = _sphereStandCenter;
        _sphereRadius = _sphereStandRadius;
    }

    private void Update()
    {
        // sorte de collider leger sans physique (je n'en ai pas besoin dans ce jeu), plus leger que les onTriggerEnter
        // Physics.OverlapSphere(_sphereCenter, _sphereRadius); 
        // il renvoit un array, contenant une liste de collider (gameObject de ma scene) qui sont en train de collider avec lui
        // mais dans notre cas on veut juste savoir si il y a juste une collision, donc une version encore plus simple (et donc legere) d'"OverlapSphere" existe "OverlapSphereNonAlloc"
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position + _sphereCenter, _sphereRadius, _hitResults); // il renvoit juste un int, (le nombre de collision detectée toute les frame) donc si collision ou pas si ce int est supérieur a 0
                                                                                                                      // j'ajoute transform.position + a "_sphereCenter" pour que "OverlapSphereNonAlloc" suive la position de mon Player
                                                                                                                      // "OverlapSphereNonAlloc" exige une parametre suplementaire, un array du nombre de collider que je veux lire dans mon ca 1 seul suffit
                                                                                                                      // "hitCount", recupére ce nombre

        if (hitCount > 0 && _isHit == false) // si _isHit est faux (pas de colision) et que hitCount > 0 (tout a coup collision)
        {

            // Debug.Log("Player take damage");

            GameEventService.OnCollision?.Invoke(); // alors le transmets a mon GameEventService que j'ai touché un GameObject


            _isHit = true; // devient true

            // SAVOIR SI LE COLLIDER EST UN COLLECTIBLE SI OUI LEQUEL :
            // sachant que mon player n'a pas de collider mais un OverlapSphereNonAlloc
            // sachant qu'un OverlapSphereNonAlloc : Ne détecte PAS "une collision", Vérifie juste quels Collider (GameObject) sont DANS sa zone
            // sachant qu'il stock ces Collider dans un tableau _hitResults (Retourne combien il en a trouvé (hitCount))
            // Comment connaitre les Collider de ce tableau ? 
            // faire une boucle, pour parcourir ce tableau
            for (int i = 0; i < hitCount; i++)
            {
                CollectibleCM collectible = _hitResults[i].GetComponent<CollectibleCM>(); // je créer une variable nommée "collectible" de type "CollectibleCM" variable créer par moi meme (custom class)
                                                                                          // pour voir si je peux récupérer le script "CollectibleCM" sur le gameObjet rentré dans avec le "colider" de mon player Physics.OverlapSphereNonAlloc  

                if (collectible != null) // Si le gameObject à le script CollectibleCM (!= null), c'est un collectible
                {
                    collectible.Collected();
                    GameEventService.OnCollisionCollectible?.Invoke(); // alors le transmets a mon GameEventService que j'ai touché un collectible

                    if (_hitResults[i].GetComponent<SucreDOrgeCollectible>())
                    {
                        GameEventService.OnCollisionCollectibleSucreDOrge?.Invoke(); // alors le transmets a mon GameEventService que j'ai touché un collectible SucreDOrge
                    }
                    else if (_hitResults[i].GetComponent<CadeauCollectible>())
                    {
                        GameEventService.OnCollisionCollectibleCadeau?.Invoke(); // alors le transmets a mon GameEventService que j'ai touché un collectible Cadeau
                    }
                    else if (_hitResults[i].GetComponent<BouleCollectible>())
                    {
                        GameEventService.OnCollisionCollectibleBoule?.Invoke(); // alors le transmets a mon GameEventService que j'ai touché un collectible Boule
                    }
                    else
                    {
                        Debug.Log("le collectible n'a pas de script BouleCollectible, CadeauCollectible ou SucreDOrgeCollectible");
                    }
                }

                else
                {
                    Debug.Log("ce n'est pas un collectible car il n'as pas de script CollectibleCM");
                }
            }
        }
        else if (hitCount == 0) // sinon si pas de collision detectée
        {
            _isHit = false; // redevient faux
        }


    }

    // permet de visualiser dans la vue scene notre Physics.OverlapSphere pour pouvoir le regler car de base il n'est pas visuel
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _sphereCenter, _sphereRadius);
    }

    public void ShrinkCollider(bool shrink) // ce coup ci je créer une methode public, car je devrait la recupérer dans "PlayerMouvementController"
    {
        if (shrink) // si shrink == true, je suis en mode detecteur compressé
        {
            _sphereCenter = _sphereShrinkCenter;
            _sphereRadius = _sphereShrinkRadius;

        } // sinon je ne le suis pas
        else
        {
            _sphereCenter = _sphereStandCenter;
            _sphereRadius = _sphereStandRadius;

        }
    }

 
   


}
