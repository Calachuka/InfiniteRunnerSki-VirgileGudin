using UnityEngine;

public class AvalancheController : MonoBehaviour
{
    // ********** BUT **********
    // mon avalanche est a 100m de mon joueur
    // elle progresse vers mon joueur de 2 manieres :
    // 1) se remplit automatiquement de 1% à chaque intervalle de temps, déterminé selon la couleur de la piste :
    // Verte: 8s, Bleue: 6s, Rouge: 4s, Noire: 2s
    // 2) à chaque collision de mon Player, elle progresse vers mon Player :
    // Verte: 5%, Bleue : 10%, Rouge : 20%, Noire : 30%
    // quand elle est sur le Player c'est GAMEOVER
    // ************************

    [SerializeField] private float _avalancheDistance = 100f;
    [SerializeField] private float _colissionDamage = 10f;

    [SerializeField] private ScoreController _scoreController; // pour récupérer la couleur de ma piste
                                                               // je creer une instance (ou référence) au script ScoreController que je nomme _scoreController 
                                                               // script ScoreController, qui contient la "PisteColor" le couleur de ma piste gérer pas sa méthode "TrackColor"
                                                               // ATTENTTION ceci créer une case, je dois y glisser le Game Obj de ma scene contenant mon script "ScoreController.cs"

    [Header("% de progression automatique de l'avalanche")]
    [SerializeField] private float _pourcentProgressAvalAuto = 1f; // pourcentage de progression de mon avalanche tous les n secondes

    [Header("Progression automatique toutes les n seconde")]
    [SerializeField] private float _timeAutoProgressAvalVerte = 8f;
    [SerializeField] private float _timeAutoProgressAvalBleu = 6f;
    [SerializeField] private float _timeAutoProgressAvalRouge = 4f;
    [SerializeField] private float _timeAutoProgressAvalNoire = 2f;

    [Header("Progression de l'avalanche a la collision en %")]
    [SerializeField] private float _progressAvalOnCollisionVerte = 5f;
    [SerializeField] private float _progressAvalOnCollisionBleu = 10f;
    [SerializeField] private float _progressAvalOnCollisionRouge = 20f;
    [SerializeField] private float _progressAvalOnCollisionNoire = 30f;


    // Update is called once per frame
    void Update()
    {
        GameEventService.OnColorPiste += VariablePerPisteColor;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "VariablePerPisteColor" à laquelle est transmisse la valeur contenu dans OnColorPiste

        // VitesseProgressAvalanche(_pourcentProgressAvalAuto, _timeAutoProgressAvalCurrent); // je recupere la variable PisteColor qui est dans mon script "ScoreController.cs"
    }


    // je créer une fonction qui me donne les bonnes variable suivant la couleur de la piste
    private void VariablePerPisteColor(string pisteColor)
    {
        float _timeAutoProgressAvalCurrent = 0;
        float _progressAvalOnCollisionCurrent = 0;

        if (pisteColor == "Verte")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalVerte;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionVerte;
        }
        else if (pisteColor == "Bleu")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalBleu;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionBleu;

        }
        else if (pisteColor == "Rouge")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalRouge;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionRouge;
        }
        else // (pisteColor == "Noire")
        {
            _timeAutoProgressAvalCurrent = _timeAutoProgressAvalNoire;
            _progressAvalOnCollisionCurrent = _progressAvalOnCollisionNoire;
        }

        Debug.Log("Piste : " + pisteColor);
    }

        void VitesseProgressAvalanche(float pourcentProgress, float timeProgress)
    {
        // je dois calculer la vitesse de mon avanche sachant que la formule mathématique est :
        // vitesse = distance / durée
        // dans notre cas : vitesse = pourcentage / durée
        // je vais obtenir une vitesse exprimée en % par seconde
        // vitesse que je devrais * Time.deltaTime
        float vitesseAvalanche = 0;
        vitesseAvalanche = pourcentProgress / timeProgress * Time.deltaTime;
        Debug.Log("VitesseAval" + vitesseAvalanche);
    }

    // penser a se desabonner OnDestroy() c 'est bien car quittera l'écoute a la fin de la partie (sinon elle peut rester en memoire plusieur parties)
    private void OnDestroy()
    {
        GameEventService.OnColorPiste -= VariablePerPisteColor; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }

}

