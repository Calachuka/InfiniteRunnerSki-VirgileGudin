using Components.Data;
using UnityEngine;

namespace Components.StateMachine
{
    public class StateMachine // sachant q'un sript en MonoBehaviour est directement instancier dans la scene car elle existe sur un GameObj, si je ne met pas "MonoBehaviour", je dois alors créer une instance de ma StateMachine
                              // instance que je créer dans le script "StateMachineController.cs"
    {

        public State CurrentState { get; private set; } // un Get (Getter) c 'est comme une variablemais seule la classe "StateMachine" peut le lire

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
            StateMachine = stateMachine;
            LevelParameters = levelParameters; // et enfin je dis que "LevelParameters" declaré au dessus, est == a "levelParameters"
        }

        public abstract void Enter(); // quand je rentre dans mon etat
        public abstract void Update(); // quand mon etat se fait Update
        public abstract void Exit(); // quand je sort de mon etat
    }




    // =============================
    // 3) je créer mes différents etats "states"
    // =============================
    #region class CountdownState

    // je créer un etat "CountdownState", compte a rebour pour la lancer le joueur directement dans la partie au chargement
    // ----------------------------------
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
            GameEventService.OnCountdownState?.Invoke(true); // je vais chercher mon Action OnGameState dans "GameEventService.cs" et si quelqu'un m'ecoute "?" je la mets a true
            _countdownTimer = 3;
        }

        // j'implement ma methode Update(); (quand mon etat se fait Update) crée plus haut (obligatoire)
        public override void Update()
        {
            _countdownTimer -= Time.deltaTime; // dans le Update (chaque frame) je decrement _countdownTimer
            
            if (_countdownTimer > 0) // je verifie si _countdownTimer et supérieur a 0 alors je return continuer a decrémenter
            {
                GameEventService.OnCountdownTick?.Invoke(_countdownTimer);
                return;
            }

            // si _countdownTimer n 'est plus > 0, alors j'ai fini ce state je passe au state de la partie (GameState)
            StateMachine.ChangeState(new GameState(StateMachine, LevelParameters)); // j'appler ma fonction "ChangeState" (qui change les etats, voir plus haut) à laquelle je donne une instance de l'état "GameState()" contenu dans ma StateMachine
        }


        // j'implement ma methode Exit(); (quand je sort de mon etat) crée plus haut (obligatoire)
        public override void Exit()
        {
            GameEventService.OnCountdownState?.Invoke(false); // je vais chercher mon Action OnGameState dans "GameEventService.cs" et si quelqu'un m'ecoute "?" je la mets a false
        }
    }
    #endregion

    #region class GameState
    // je créer mon etat "GameState" qui est mon etat de jeu qui arrive apres le l'etat "CountdownState"
    // ------------------------------
    public class GameState : State // je precise que cette class est un State
    {

        private int _currentLife;

        // je créer un constructeur de class, qui créer une instance de CountdownState et prend comme argument "stateMachine"
        // base(stateMachine) signifie qu'il apple le constcteur de base (creer dans "public abstract class State" creer au dessus) pour mettre stateMachine dans stateMachine
        public GameState(StateMachine stateMachine, SOLevelParameters levelParameters) : base(stateMachine, levelParameters)
        {
            // meme si je n'ai rien a mettre dedans le declarer car je suis obligé d'appler le constructeur ed class de base créer plus haut
        }

        // j'implement ma methode Enter(); (quand je rentre dans mon etat) crée plus haut (obligatoire)
        public override void Enter()
        {
            GameEventService.OnGameState?.Invoke(true); // j'invoke OnGameState dans "GameEventService.cs" je la mets a true
            // je m'abonne a OnCollision (dans mon eventsystem qui est classe static)
            GameEventService.OnCollision += HandleCollision; // += c'est un delegat, pour executer une methode (fonction) HandleCollision que j'ai ecrite plus bas
            _currentLife = LevelParameters.PlayerLife; // ma vie est égale a mon parametre PlayerLife de LevelParameters créer dans mon scriptableObject "SOLevelParameters.cs"
 
        }

        // j'implement ma methode Update(); (quand mon etat se fait Update) crée plus haut (obligatoire)
        public override void Update()
        {

        }

        // j'implement ma methode Exit(); (quand je sort de mon etat) crée plus haut (obligatoire)
        public override void Exit()
        {
            GameEventService.OnGameState?.Invoke(false); // je vais chercher mon Action OnGameState dans "GameEventService.cs" et si quelqu'un m'ecoute "?" je la mets a false
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

    #endregion

    #region class GameOverState

    // je créer mon etat "GameOverState" qui est mon etat de jeu quand je perd
    // ---------------------------------
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

    #endregion
}
