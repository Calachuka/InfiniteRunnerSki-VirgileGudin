using UnityEngine;
using System.Collections;

/// <summary>
/// Player movement by listening to inputs
/// </summary>




public class PlayerMovementController : MonoBehaviour
{

    [Header("Jump Parameters")]
    [SerializeField] private float _JumpDuration = 1f; // durée du saut (le "_" c'est la convention de codage de Yona pour reconnaitre que cette variable est privée)
                                                       // [SerializeField] permet de créer un champ dans l'inspector meme si ma variable est privée
    [SerializeField] private float _JumpHeight = 2f; // hauteur du saut (le "_" c'est la convention de codage de Yona pour reconnaitre que cette variable est privée)
    [SerializeField] private AnimationCurve _JumpCurve; // je créer une "animation curve", pour smoother l'anim de mon saut en montée. dans l'inspector, je peux maintenant modifier ma courbe (double clic pour ajouter un point)
    [SerializeField] private AnimationCurve _FallCurve; // je créer une "animation curve", pour smoother l'anim de mon saut en descente. , dans l'inspector, je peux maintenant modifier ma courbe (double clic pour ajouter un point)

    [Header("Slide Parameters")] // gere les parametres de mes deplacement horirontaux
    [SerializeField] private float _slideDuration = 0.5f; //gére la durée de deplacement d'un lane a un autre
    [SerializeField] private Transform[] _slideTargets; // va créer un tableau dans le quel je vais pouvoir mettre les 3 lanes

    [Header("SlideDown Parameters")]
    [SerializeField] private float _slideDownDuration = 0.5f; // durée du slideDown

    [Header("Components")] // gere ici les components relier a script
    [SerializeField] private Animator _animator; // je créer une variable donc un champ pour indiquer mon animator au code
    [SerializeField] private PlayerCollisionController _collisionController; // je créer un variable qui va appeler le componant, le script "PlayerCollisionController" que je nomme "_collisionController"
                                                                            // ATTENTION je dois gliise rmon Player dans cette case dans mon isnpector
    [Header("debug")] // pour vois les debug
    [SerializeField] private bool _isJumping; // ce bool va me permettre de verifier si je suis pas deja dans un saut, me créer un case dans l'inspector, qui se coche quand mon booleen est en saut
    [SerializeField] private bool _isSliding; // ce bool va me permettre de verifier si je suis pas deja dans un deplacement, me créer un case dans l'inspector, qui se coche quand mon booleen est en deplacement
    [SerializeField] private bool _isSlidingDown; // ce bool va me permettre de verifier si je suis pas deja dans un deplacement, me créer un case dans l'inspector, qui se coche quand mon booleen est en isSlidingDown
    [SerializeField] private int _CurrentLaneIndex = 1; // me variable qui m'initialise mon player a la lane 1 soit la lane du milieu

    private const string JUMP_PARAMETER = "IsJumping";
    private const string SLIDE_DOWN_PARAMETER = "IsSlidingDown";
    private const string GROUNDED_PARAMETER = "Grounded";

    private void Update() // j'utilise Update car je veux chequer une permanence cet aspect
    {
        // INPUT "SAUT"
        if (Input.GetKeyDown(KeyCode.UpArrow)) // ancien systeme d'input, ici : quand j'appuie sur "UpArrow" je saute en deplacant mon player vers le haut (Y)
        {
            HandleJump(); // J'appelle la fonction : HandleJump() que je vais ecrire plus bas
        }

        // INPUT "deplacement Gauche"
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // si j'apuye sur LeftArrow
        {
            if (_isSliding) //si "isSliding" = true, ce bool va me permettre de verifier si je suis pas deja dans un deplacement, pour eviter que ca lance plusieur deplacement si je mattraque la touche, ca terminera bien la coroutine avnt
            { 
                return; // alors je n'ai rien a faire dans cette boucle, Les lignes suivantes (décrémenter _CurrentLaneIndex et lancer la coroutine) ne sont pas exécutées.
                        // quitte la fonction immédiatement, Tout ce qui suit le return dans la fonction est ignoré
                        // La prochaine lecture repart depuis le début de la fonction lors de la prochaine frame (si c’est Update())
            }
            if (_CurrentLaneIndex == 0) // si _CurrentLaneIndex est = 0
            { 
                return; // alors je n'ai rien a faire dans cette boucle, Les lignes suivantes (décrémenter _CurrentLaneIndex et lancer la coroutine) ne sont pas exécutées.
            }

            _CurrentLaneIndex--; // me permet de descendre dans mes lanes
            StartCoroutine(SlideCoroutine(_slideTargets[_CurrentLaneIndex]));  // J'appelle la fonction : SlideCoroutine() qui contient le parametre Transform, donc le nom de la target actuelle sur la quelle mon Plyer est
        }

        // INPUT "deplacement Droite"
        if (Input.GetKeyDown(KeyCode.RightArrow)) // si j'apuye sur RightArrow
        {
            if (_isSliding) //si "isSliding" = true, ce bool va me permettre de verifier si je suis pas deja dans un deplacement, pour eviter que ca lance plusieur deplacement si je mattraque la touche, ca terminera bien la coroutine avnt
            {
                return; // alors je n'ai rien a faire dans cette boucle, Les lignes suivantes (décrémenter _CurrentLaneIndex et lancer la coroutine) ne sont pas exécutées.
            }

            if (_CurrentLaneIndex == _slideTargets.Length - 1) // si _CurrentLaneIndex est = a la longueur de ma liste _slideTargets -1 car on compte a partir de 0 (donc si égale 2) 
            {
                return; // alors je n'ai rien a faire dans cette boucle, Les lignes suivantes (décrémenter _CurrentLaneIndex et lancer la coroutine) ne sont pas exécutées.
            }

            _CurrentLaneIndex++; // me permet de monter dans mes lanes
            StartCoroutine(SlideCoroutine(_slideTargets[_CurrentLaneIndex]));  // J'appelle la fonction : SlideCoroutine() qui contient le parametre Transform, donc le nom de la target actuelle sur la quelle mon Plyer est
        }

        // INPUT "je me baisse"
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_isSlidingDown || _isJumping) // si "isSlidingDown" = true OU "_isJumping " = true evites de lancer un slideDown si je suis deja en train d 'en faire un OU si je suis en train de sauter
            {
                return;// alors je n'ai rien a faire dans cette boucle, je ne lance pas ma coroutine, car je suis deja en train de isSlidingDown ( de faire SlideDoWn)     
            }
            StartCoroutine(SlideDownCoroutine());  // J'appelle la fonction : SlideDownCoroutine()
        }

    }


    // ma fonction qui gère les sauts
    // ------------------------
    private void HandleJump()
    {
        if (_isJumping)  // si "isJumping" = true, tant que _isJumping n'est pas egale a false, je ne veux pas demarrer de coroutine de saut, donc ne vais pas plus loin avec avec le "return" qui tourne en boucle et ne vas pas lire plus bas
        {
            return; // C’est une barrière de sécurité, On appelle ça un guard clause, tourne en boucle (chaque frame) tant que _isJumping est "true"
        }

        StartCoroutine(JumpCoroutine());  // J'appelle la fonction : JumpCoroutine() que je vais ecrire plus bas
    }



    // ma coroutine de saut qui permet de gérer le temps de mon saut
    // ------------------------
    private IEnumerator JumpCoroutine()
    {
        _animator.SetBool(JUMP_PARAMETER, true); // donc quand j'appuie sur espace, je dis à mon animator de jouer "IsJumping" en true
        // sachant que mon saut c'est sauter/retomber, je créer une variable qui divise en 2 temps _JumpDuration ("Half" signifie moitier)
        var halfJumpDuration = _JumpDuration / 2f;
        // je créer un var "JumpTimer" que j'initialise a 0
        var JumpTimer = 0f;


        // -------- montée de mon saut -------
        // je fais une boucle while, qui execute tant que sa condition n 'est pas remplie
        while (JumpTimer < halfJumpDuration)
        {
            _isJumping = true ; // veux dire que je suis en train de sauter, ceci pour verifier que je suis bien dans une boucle de saut

            // a chaque Update j'incremente "JumpTimer" de soit 0.16 secondes
            JumpTimer += Time.deltaTime;

            // je normalise mon temps entre 0 (point de départ(en bas)) et 1 (point de d'arrivée(en haut))
            float normalizedTime = Mathf.Clamp01(JumpTimer / halfJumpDuration); // (par ex : 0.32 / 0.5 = 0,64) // et pour etre sur que ce soit compris entre 0 et 1, je clamp avec "Mathf.Clamp01"		

            // je remplace mon "Mathf.Lerp" (Lerp : abreviation de Linear Interpolation) qui était linéaire par l'animation curve _JumpCurve.Evaluate, nous permet de recupérer des valeur de mon animation curve _JumpCurve, ceci en fonction d'un temps normaliser entre 0 et 1
            float targetHeight = _JumpCurve.Evaluate(normalizedTime) * _JumpHeight ;

            // je modifie ma position en "y" de mon player
            var targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            transform.position = targetPosition;

            // je suis obligé de le mettre pour qu'il recommence a la fin de chaque itérations
            yield return null;
        }

        _animator.SetBool(JUMP_PARAMETER, false); // donc quand on a fini de sauter, je dis à mon animator de mettre "IsJumping" en false et donc de passer a l'anim fall

        // -------- descente de mon saut -------
        // je réinitalise mon JumpTimer, car il etait incrementé dans le while de montée du saut
        JumpTimer = 0f;

        // je fais une boucle while, qui execute tant que sa condition n 'est pas remplie
        while (JumpTimer < halfJumpDuration)
        {
            // a chaque Update j'incremente "JumpTimer" de soit 0.16 secondes
            JumpTimer += Time.deltaTime;

            // je normalise mon temps entre 0 (point de départ(en bas)) et 1 (point de d'arrivée(en haut))
            float normalizedTime = Mathf.Clamp01(JumpTimer / halfJumpDuration); // (par ex : 0.32 / 0.5 = 0,64) // et pour etre sur que ce soit compris entre 0 et 1, je clamp avec "Mathf.Clamp01"	

            // je remplace mon "Mathf.Lerp" (Lerp : abreviation de Linear Interpolation) qui était linéaire par l'animation curve _FallCurve.Evaluate, nous permet de recupérer des valeur de mon animation curve _JumpCurve, ceci en fonction d'un temps normaliser entre 0 et 1
            float targetHeight = _FallCurve.Evaluate(normalizedTime) * _JumpHeight;

            // je modifie ma position en "y" de mon player
            var targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            transform.position = targetPosition;

            // je suis obligé de le mettre pour qu'il recommence a la fin de chaque frame
            yield return null;
        }

        _animator.SetTrigger(GROUNDED_PARAMETER); // donc quand on a fini de sauter et que je touche le sol, je dis à mon animator sortir et de retourner a jump

        _isJumping = false; // veux dire que je NE suis PLUS en train de sauter, ceci pour verifier que je NE suis PLUS dans une boucle de saut

    }


    // ma coroutine de Slide (deplacement gauche ou droit) qui permet de gérer (deplacement gauche ou droit)
    // ------------------------
    private IEnumerator SlideCoroutine(Transform target) // je lui donne comme argument le "Transform" que j'ai créer plus haut et qui contient mes 3 lanes
    {
        _isSliding = true; // veux dire que je suis en train de me deplacer, ceci pour verifier que je suis bien dans une boucle de déplacement

        // je créer un var "SlideTimer" que j'initialise a 0
        var SlideTimer = 0f;

        while (SlideTimer < _slideDuration)
        {
            // a chaque Update j'incremente "SlideTimer" de soit 0.16 secondes
            SlideTimer += Time.deltaTime;

            // je normalise mon temps entre 0 (point de départ(en bas)) et 1 (point de d'arrivée(en haut))
            float normalizedTime = Mathf.Clamp01(SlideTimer / _slideDuration); // (par ex : 0.32 / 0.5 = 0,64) // et pour etre sur que ce soit compris entre 0 et 1, je clamp avec "Mathf.Clamp01"		

            // je veux deplacer mon perso sur la position, donc je créer une nouvelle variable de type vector3 qui prendra la position x de target de notre coroutine plus haut
            // et pour y et z , je recupere la position acteilles du player (avec transform.position.y, transform.position.z) car il est peut etre en train de sauter 
            var targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);

            // je veux dire que notre tranform.position (position de mon player) est maintenant egale a notre targetPosition (position de nos lanes) le deplacement 
            // j'utilise Vector3.Lerp (Lerp : abreviation de Linear Interpolation) qui va faire lerp (transition) entre le 2 vector3 sur un temps donné normalisé "normalizedTime"
            transform.position = Vector3.Lerp(transform.position, targetPosition, normalizedTime);

            // je suis obligé de le mettre pour qu'il recommence a la fin de chaque frame
            yield return null; // signifie que le programme s’est terminé correctement. renvoie 0 au système / Par convention, tout s’est bien passé
        }

        _isSliding = false; // veux dire que je suis sorti de ma boucle de déplacement
    }



    // ma coroutine de SlideDown (je me baisse) qui permet de gérer (je me baisse)
    // ------------------------
    private IEnumerator SlideDownCoroutine() // pas besoin d'argument entre ses parentheses
    {
        var SlideTimer = 0f; // je créer une variable SlideTimer que initialise a 0

        _isSlidingDown = true; // me permet de vérifier je suis deja en train de faire le mouvement, pour que le joueur ne puisse en lancer directement un autre
        _animator.SetBool(SLIDE_DOWN_PARAMETER, true); // au debut de mon animator "SLIDE_DOWN_PARAMETER" est true
        _collisionController.ShrinkCollider(true); // mets la methode public "ShrinkCollider" a true, elle se situe dans mon script "PlayerCollisionController

        // tant que mon SlideTimer est inférieur a la slideDownDuration
        while (SlideTimer < _slideDownDuration)
        {
            // a chaque Update j'incremente "SlideTimer" de soit 0.16 secondes
            SlideTimer += Time.deltaTime;

            // je suis obligé de le mettre pour qu'il recommence a la fin de chaque frame
            yield return null; // signifie que le programme s’est terminé correctement. renvoie 0 au système / Par convention, tout s’est bien passé
        }

        _collisionController.ShrinkCollider(false); // mets la methode public "ShrinkCollider" a false, elle se situe dans mon script "PlayerCollisionController
        _animator.SetBool(SLIDE_DOWN_PARAMETER, false); // a la fin de mon animator "SLIDE_DOWN_PARAMETER" est false
        _isSlidingDown = false; // me permet de vérifier je suis deja en train de faire le mouvement, pour que le joueur ne puisse en lancer directement un autre
    }

}
