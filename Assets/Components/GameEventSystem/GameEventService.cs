using System;
using UnityEngine.SocialPlatforms.Impl;


public static class GameEventService // cet eventsystem ne va pas etre un script monobehavior, mais une classe static, une classe qui est accessible n'importe ou dans le jeu et existe tout le temps
{
    public static Action OnCollision; // pour declarer un evenement en c# utiliser les Action static  
    public static Action<int> OnPlayerLifeUpdated; // pour declarer un evenement en c# utiliser les Action static
                                                   // C’est un événement basé sur un delegate, plus précisément :
                                                   // Un event global (statique) utilisant Action<T>
                                                   // représente une fonction qui : ne retourne rien(void), prend un paramètre int


    public static Action<bool> OnCountdownState; // pour declarer un evenement en c# utiliser les Action static
    public static Action<bool> OnGameState; // pour declarer un evenement en c# utiliser les Action static
                                            // C’est un événement basé sur un delegate, plus précisément :
                                            // Un event global (statique) utilisant Action<T>
                                            // représente une fonction qui : ne retourne rien(void), prend un paramètre bool
                                            // comment ca marche concretement :
                                            // - 1) DONNER L'EVENEMENT (Invoke)
                                            // code :
                                            // GameEventService.OnGameState?.Invoke(true); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés
                                            // ATTENTION vu que la fonction dans laquelle il est va etre lu dans le Update, dans ce cas :
                                            // faire une compraison pour ne l'invoquer que quand il est différent de la valeur précèdante
                                            // ex :
                                            /*
                                            if (_pisteColorCurrent != pisteColorCurrentNew)
                                            {
                                                // Debug.Log("Invoke");
                                                GameEventService.OnColorPiste?.Invoke(pisteColorCurrentNew);
                                            }
                                            */
                                            // - 2) S'ABONNER à L'EVENEMENT (écouter l’événement) 
                                            // code :
                                            // GameEventService.OnGameState += HandleGameState;  // je m'abonne, j'entend (ecoute) que OnGameState est Invoké quelque part dans un script, alors je lis la fonction "HandleGameState()"
                                            // Cette ligne n’appelle pas Score tout de suite.
                                            // Elle enregistre la méthode Score comme abonnée à l’event OnColorPiste.
                                            // Score sera exécutée uniquement quand l’event sera invoqué, c’est-à-dire quand tu fais :
                                            // Tant que tu n’as pas fait l’Invoke, rien ne se passe.
                                            // DONC je peux mettre cet abonnement dans "OnEnable()" pas de soucis SURTOUT PAS dans le "Update()" (bouffe trop de CPU)
                                            // j'execute la méthode score en lui transmettant un parametre qui est dans mon GameEventService.cs
                                            // - 3) Se désabonner (OBLIGATOIRE)
                                            // code :
                                            // GameEventService.OnGameState -= HandleGameState; // je me désabonne (OBLIGATOIRE)
                                            // le mettre dans "OnDistroy()" ou "OnDisable()"
    public static Action<bool> OnGameOverState; // pour declarer un evenement en c# utiliser les Action static
    
    public static Action<float> OnCountdownTick; // pour declarer un evenement en c# utiliser les Action static
    
    public static Action<string> OnColorPiste; // stock le nom de ma couleur de piste

}