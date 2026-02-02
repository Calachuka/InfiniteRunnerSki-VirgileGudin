using Components.Data;
using UnityEngine;

// --- BUT ---
// il Créer UNE nouvelle instance de StateMachine, "_stateMachine" class StateMachine qui elle, se trouve dans StateMachine.cs
// au demarrage du jeu donc dans private void Awake(), j'initialise mon jeu au state "CountdownState" 
// que je donne ma nouvelle instance de "StateMachine", donc _stateMachine
// j'appelle aussi le Update() nouvelle instance de "StateMachine", donc l'Update de mon "CurrentState",
// -----------

namespace Components.StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        private StateMachine _stateMachine; // je créer un variable private (accessible uniquement dans ce script) que je nomme _stateMachine et qui va me servir plus bas
                                            // Tu réserves un emplacement pour stocker StateMachine. À ce moment-là, la variable existe mais elle est vide(null)
        
        [SerializeField] private SOLevelParameters _levelParameters; // je créer un champ pour lui glisser le paramettre que je souhaite, de mon ScriptableOject

        private void Awake() // Awake() s’exécute, Juste après le chargement de l’objet (le script), avant Start()
                             // Awake() généralement ? Initialiser des variables, Créer des objets, Préparer des références
                             // --- ordre d'execution des methodes speciale Unity : ---
                             // Awake() : appelé une seule fois,
                             // OnEnable() : appelé juste après Awake,
                             // Start() : appelé une seule fois,
                             // Update() : appelé chaque frame.
        {
            _stateMachine = new StateMachine(); // j'attribue à _stateMachine une nouvelle instance de ma class StateMachine qui se trouve dans StateMachine.cs

            // au demarrage du jeu j'initialise mon jeu au state "CountdownState" au quel je donne une instance de "StateMachine", donc _stateMachine
            var initialState = new CountdownState(_stateMachine, _levelParameters); // donc ce créer une variable "initialState" a la quelle je donne une nouvelle instance de "CountdownState" de mon instance _stateMachine
            _stateMachine.ChangeState(initialState); // j'execute alors la fonction "ChangeState" (avec commme argument "initialState"), qui se trouve dans "StateMachine.cs" ceci en executant ma StateMachine grace son instance nommée "_stateMachine" 
        }

        public void Update() => _stateMachine.Update(); // j'appelle aussi le Update() de ma StateMachine, donc l'Update de mon "CurrentState", ceci grace a son instance _stateMachine, ceci en fesant ce raccoucit
                                                        // meme chose que :
                                                        /* 
                                                        public void Update()
                                                        {
                                                            _stateMachine.Update();
                                                        }
                                                        */

    }

}