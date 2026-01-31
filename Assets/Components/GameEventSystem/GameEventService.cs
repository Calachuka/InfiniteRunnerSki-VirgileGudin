using System;


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
                                            // - S’abonner (écouter l’événement)
                                            // GameEventService.OnGameState += HandleGameState;  // si j'entend (ecoute) que OnGameState est Invoké quelque part dans un script, alors je lis la fonction "HandleGameState()"
                                            // - Déclencher l’événement
                                            // GameEventService.OnGameState?.Invoke(true); // donne l'info a notre GameEventService.cs, il l'Invoke, envoies l’information aux abonnés
                                            // en general je mets ceci dans le Update() qui script qui donne l'info
                                            // - Se désabonner (OBLIGATOIRE)
                                            // GameEventService.OnGameState -= HandleGameState;
    public static Action<bool> OnGameOverState; // pour declarer un evenement en c# utiliser les Action static

    public static Action<float> OnCountdownTick; // pour declarer un evenement en c# utiliser les Action static

}