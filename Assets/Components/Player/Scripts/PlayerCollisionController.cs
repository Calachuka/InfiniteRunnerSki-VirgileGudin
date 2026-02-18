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

    private readonly Collider[] _hitResults = new Collider[1]; // "OverlapSphereNonAlloc" exige une parametre suplementaire, un array du nombre de collider que je veux lire dans mon ca s1 seul suffit
                                                               // var de type Unit "Collider", représente un composant physique attaché à un GameObject c 'est pour recupérer les collider de mes gameObject Obstacles qui eux en ont un
                                                               // "readonly" Tu comprends immédiatement Cette valeur est une constante d’instance, Si une variable ne devrait jamais changer après l’initialisation, mets readonly

    private void Start() // au demarrage du jeu mon personnage est debout, donc je lui attribut son detecteur de collision en possition debout
    {
        _sphereCenter = _sphereStandCenter;
        _sphereRadius = _sphereStandRadius;
    }

    private void Update()
    {
        // sorte de collider leger sans physique (je n'en ai pas besoin dans ce jeu), plus leger que les onTriggerEnter
        // Physics.OverlapSphere(_sphereCenter, _sphereRadius); 
        // il renvoit un array, contenant une liste de collider (gameObject de ma scene) qui sont en train de colider avec lui
        // mais dans notre cas on veut juste savoir si il y a juste une collision, donc une version encore plus simple (et donc legere) d'"OverlapSphere" existe "OverlapSphereNonAlloc"
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position + _sphereCenter, _sphereRadius, _hitResults); // il renvoit juste un int, (le nombre de collision detectée toute les frame) donc si collision ou pas si ce int est supérieur a 0
                                                                                                                      // j'ajoute transform.position + a "_sphereCenter" pour que "OverlapSphereNonAlloc" suive la position de mon Player
                                                                                                                      // "OverlapSphereNonAlloc" exige une parametre suplementaire, un array du nombre de collider que je veux lire dans mon ca 1 seul suffit
                                                                                                                      // "hitCount", recupére ce nombre

        if (hitCount > 0 && _isHit == false) // si _isHit est faux (pas de colision) et que hitCount > 0 (tout a coup collision)
        {

            Debug.Log("Player take damage");

            // j'appelle "OnCollision" (qui me dit si collision ou pas) qui sur mon eventSystem (de type classe static)
            // je n'ai pas besoin d'appler cette classe plus haut car elle existe partout donc elle est directement accessible
            GameEventService.OnCollision?.Invoke(); // j'applle "OnCollision" qui est sur mon script "GameEventSystem.cs" et sa valeur "OnCollision", "?" est neccessaire pour faire une erreur si personne n'est abonné à "OnCollision" | "Invoke" = appeler, déclencher
            
            _isHit = true; // devient true
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
