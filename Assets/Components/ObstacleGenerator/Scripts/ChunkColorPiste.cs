using TMPro;
using UnityEngine;

public class ChunkColorPiste : MonoBehaviour
{
    Renderer _renderer; // déclare cette variable que je pourrait initialiser, dans start et qui recupere le "Renderer" de mon gameObject (ceci de mon inspector, car c'est lui qui contient "material")

    [Header("Material per color")]
    [SerializeField] private Material _materialVerte; // je créer un champ pour lui glisser le material de mon sol Bleu
    [SerializeField] private Material _materialBleu;
    [SerializeField] private Material _materialRouge;
    [SerializeField] private Material _materialNoire;

    private Material _materialCurrent; // je crer un variable qui va me servir plus bas

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameEventService.OnColorPiste += MaterialPerColorPiste;  // je m'abonne à mon GameEventService.cs et j'exécute la fonction "ChangeColorPiste" à laquelle est transmis la valeur contenu dans OnColorPiste
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>(); // je peux au start initialiser _renderer, qui recupere son "Renderer" de mon gameObject (ceci de mon inspector, car c'est lui qui contient "material")
        _materialCurrent = _materialVerte; // au lancement j'intialise cette _materialCurrent à _materialVerte
        _renderer.material = _materialCurrent; // applique le material au démarrage
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // méthode qui attribut un material différent suivant la couleur de la piste
    // --------------------------------------
    private void MaterialPerColorPiste(string pisteColor)
    {

        if (pisteColor == "Verte")
        {
            _materialCurrent = _materialVerte;
        }
        else if (pisteColor == "Bleu")
        {
            _materialCurrent = _materialBleu;
        }
        else if (pisteColor == "Rouge")
        {
            _materialCurrent = _materialRouge;
        }
        else // (pisteColor == "Noire")
        {
            _materialCurrent = _materialNoire;
        }

        _renderer.material = _materialCurrent; // C’EST ÇA QUI MANQUAIT

        Debug.Log("PisteColor : " + pisteColor);
        Debug.Log("MaterialColor : " + _renderer.material);

    }
    

    void OnDestroy()
    {
        GameEventService.OnColorPiste -= MaterialPerColorPiste; // je me désabonne de mon GameEventService.cs (OBLIGATOIRE)
    }
    
}
