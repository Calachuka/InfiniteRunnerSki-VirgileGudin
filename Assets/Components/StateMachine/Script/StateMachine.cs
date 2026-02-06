using Components.Data;
using Test;
using UnityEngine;

// --- BUT ---
// cours : Accompagnement M3 S1
// - gere logique du jeu, enchainement et initialisation des différents states
// Connaître l’état courant
// Gérer les transitions
// Appeler Enter / Update / Exit au bon moment
// 1) création d'une class public "StateMachine", qui permet de changer l'etat de mon jeu, grace a sa fonction "ChangeState",
// qui dit comment vont s'enchainer les methodes (elles crées dans astract, plus bas) pour que ca fasse un changements
/*
CurrentState?.Exit();
CurrentState = newState;
CurrentState.Enter();
*/
// 2) je cérer une class abstract (abstraite) State : (une classe qu’on ne peut PAS instancier, c'est un modèle / contrat pour ses classes enfants)
// pour declarer les 3 methodes (Enter(); Update(); Exit();) qui vont gouverner mes différents state
// qui contient mon constructeur de class de base, qui sera ensuite appeler dans mes différents states plus bas
// -----------

namespace Components.StateMachine
{
    public class StateMachine // sachant q'un sript en MonoBehaviour est directement instancier dans la scene car elle existe sur un GameObj, si je ne met pas "MonoBehaviour", je dois alors créer une instance de ma StateMachine
                              // instance que je créer dans le script "StateMachineController.cs"
    {

        public State CurrentState { get; private set; } // un Get (Getter) c'est comme une variable mais seule la classe "StateMachine" peut le lire

        public void ChangeState(State newState) // je créer une methode ChangeState qui prend comme argument un type "State" que je nomme "newState"
        {
            CurrentState?.Exit(); // ?. : raccourcit d'un if pour dire : si CurrentState est différent de Null, donc verifie l'existance de CurrentState, alors j'appelle Exit(); (methode definit plus bas)
            CurrentState = newState;
            CurrentState.Enter(); // CurrentState = Enter(); (methode definit plus bas)
        }

        public void Update() => CurrentState?.Update(); // je créer un raccoucit en mode Update, ?. qui veut dire "est différent de Null" donc verifie l'existance de CurrentState, alors je l'Update()

    }


    // =============================
    // 2) je cérer une class abstract (abstraite), pour declarer les 3 methodes (Enter(); Update(); Exit();) qui vont gouverner mes différents state
    // =============================
    public abstract class State // je cérer une class abstract (abstraite), pour declarer les 3 methodes (Enter(); Update(); Exit();) qui vont gouverner mes différents state
    {
        protected readonly StateMachine StateMachine; // me créer un acces a ma class "StateMachine" car je vais en avoir besoin plus bas, pour lui dire change de state
                                                      // readonly, pour dire qu'on peux l'initialiser une fois a l'instialtion de la class
                                                      // donc je suis obligé de créer un constructeur de class juste en dessous

        protected readonly SOLevelParameters LevelParameters; // me créer un acces aux parametres "LevelParameters" de mon scriptableObject "SOLevelParameters"

        // je créer mon constructeur de class de base, qui sera ensuite appeler dans mes différents states plus bas "CountdownState", "GameState", ...
        protected State(StateMachine stateMachine, SOLevelParameters levelParameters) // ajouter le parametre "SOLevelParameters levelParameters" a cette fonction qui existait deja
        {
            StateMachine = stateMachine; // fait ref à StateMachine, la class construite juste au dessus, et qui contient la méthode "ChangeState"
            LevelParameters = levelParameters; // et enfin je dis que "LevelParameters" declaré au dessus, est == a "levelParameters"
        }

        public abstract void Enter(); // quand je rentre dans mon etat
        public abstract void Update(); // quand mon etat se fait Update
        public abstract void Exit(); // quand je sort de mon etat
    }




    // =============================
    // 3) je créer mes différents etats "states"
    // =============================


    // je créer un etat "CountdownState", compte a rebour pour la lancer le joueur directement dans la partie au chargement
    // ==================================
    public class CountdownState : State // je precise que cette class est un State
    {
        private float _countdownTimer; // je créer une variable _countdownTimer

        // je créer un constructeur de class, qui créer une instance de CountdownState et prend comme argument "stateMachine"
        // base(stateMachine) signifie qu'il apple le constcteur de base (creer dans "public abstract class State" creer au dessus) pour mettre stateMachine dans stateMachine
        public CountdownState(StateMachine stateMachine, SOLevelParameters levelParameters) : base(stateMachine, levelParameters) 
        {
            // meme si je n'ai rien a mettre dedans le declarer car je suis obligé d'appler le constructeur ed class de base créer plus haut
        }

        // j'implement ma methode Enter(); (quand je rentre dans mon etat) crée plus haut (obligatoire)
        public override void Enter()
        {
            GameEventService.OnCountdownState?.Invoke(true); // je previens le "GameEventService.cs" que je viens de lancer le CountdownState
            _countdownTimer = 3f; // je l'intialise a 3 seconde
        }

        // j'implement ma methode Update(); (quand mon etat se fait Update) crée plus haut (obligatoire)
        public override void Update()
        {
            _countdownTimer -= Time.deltaTime; // dans le Update (chaque frame) je decrement _countdownTimer

            // si _countdownTimer est plus supérieur à 0 (> 0), je reste dans la boucle
            if (_countdownTimer > 0)
            {
                GameEventService.OnCountdownTick?.Invoke(_countdownTimer); // j'envois à mon "GameEventService.cs" les etapes (chiffres) de _countdownTimer

                return; // Quitte la fonction Update() immédiatement (le reste du code ne sera pas exécuté), donc, puis il repasse directement dans Update()
            }

            // Je passe au state de la partie (GameState)
            StateMachine.ChangeState(new GameState(StateMachine, LevelParameters)); // fait ref à StateMachine, la class construite juste au dessus, et qui contient la méthode "ChangeState"
                                                                                    // ChangeState : méthode qui permet de changer les états
                                                                                    // à laquelle je donne une instance de l'état "GameState()" qui lance le level

        }

        // j'implement ma methode Exit(); (quand je sort de mon etat) crée plus haut (obligatoire)
        public override void Exit()
        {
            GameEventService.OnCountdownState?.Invoke(false); // je previens le "GameEventService.cs" que je sors du CountdownState
        }
    }





    // je créer mon etat "GameState" qui est mon etat de jeu qui arrive apres le l'etat "CountdownState"
    // =============================
    public class GameState : State // je precise que cette class est un State
    {

        private int _currentLife;


        // je créer un constructeur de class, qui créer une instance de GameState et prend comme argument "stateMachine"
        // base(stateMachine) signifie qu'il apple le constcteur de base (creer dans "public abstract class State" creer au dessus) pour mettre stateMachine dans stateMachine
        public GameState(StateMachine stateMachine, SOLevelParameters levelParameters) : base(stateMachine, levelParameters)
        {
            // meme si je n'ai rien a mettre dedans le declarer car je suis obligé d'appler le constructeur ed class de base créer plus haut
        }

        // j'implement ma methode Enter(); (quand je rentre dans mon etat) crée plus haut (obligatoire)
        public override void Enter()
        {
            GameEventService.OnGameState?.Invoke(true); // je previens le "GameEventService.cs" que je viens de lancer le GameState
            // je m'abonne a OnCollision (dans mon eventsystem qui est classe static)
            GameEventService.OnCollision += HandleCollision; // += c'est un delegat, j'ecoute le "GameEventService.cs" pour lancer HandleCollision que j'ai ecrite plus bas à chaque collision
            _currentLife = LevelParameters.PlayerLife; // ma vie est égale a mon parametre PlayerLife de LevelParameters créer dans mon scriptableObject "SOLevelParameters.cs"
 
        }

        // j'implement ma methode Update(); (quand mon etat se fait Update) crée plus haut (obligatoire)
        public override void Update()
        {

        }

        // j'implement ma methode Exit(); (quand je sort de mon etat) crée plus haut (obligatoire)
        public override void Exit()
        {
            GameEventService.OnGameState?.Invoke(false); // je previens le "GameEventService.cs" que je viens de quitter le GameState
            GameEventService.OnCollision -= HandleCollision; // je me desabonne
            
        }


        // methode qui gere mes collisions
        /// <summary>
        /// 
        /// </summary>
        private void HandleCollision()
        {
            _currentLife--; // je perd un point de vie
            GameEventService.OnPlayerLifeUpdated?.Invoke(_currentLife);// si tu m'ecoute je te dis que la vie du joueur vaut newLife

            // je gere mon GAMEOVER
            if (_currentLife <= 0) // si newLife <= 0, plus de vie
            {
                StateMachine.ChangeState(new GameOverState(StateMachine, LevelParameters)); // je charge mon etat de GameOverState
            }

        }

    }





    // je créer mon etat "GameOverState" qui est mon etat de jeu quand je perd
    // ================================
    public class GameOverState : State // je precise que cette class est un State
    {

        // je créer un constructeur de class, qui créer une instance de GameOverState et prend comme argument "stateMachine"
        // base(stateMachine) signifie qu'il apple le constcteur de base (creer dans "public abstract class State" creer au dessus) pour mettre stateMachine dans stateMachine
        public GameOverState(StateMachine stateMachine, SOLevelParameters levelParameters) : base(stateMachine, levelParameters)
        {
            // meme si je n'ai rien a mettre dedans le declarer car je suis obligé d'appler le constructeur ed class de base créer plus haut
        }

        // j'implement ma methode Enter(); (quand je rentre dans mon etat) crée plus haut (obligatoire)
        public override void Enter()
        {
            GameEventService.OnGameOverState?.Invoke(true); // je vais chercher mon Action OnGameOverState dans "GameEventService.cs" et si quelqu'un m'ecoute "?" je la mets a true
        }

        // j'implement ma methode Update(); (quand mon etat se fait Update) crée plus haut (obligatoire)
        public override void Update()
        {

        }


        // j'implement ma methode Exit(); (quand je sort de mon etat) crée plus haut (obligatoire)
        public override void Exit()
        {
            GameEventService.OnGameOverState?.Invoke(false); // je vais chercher mon Action OnGameOverState dans "GameEventService.cs" et si quelqu'un m'ecoute "?" je la mets a false

        }
    }

}
