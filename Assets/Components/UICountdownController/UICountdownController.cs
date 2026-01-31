using TMPro;
using UnityEngine;

public class UICountdownController : MonoBehaviour
{
    [SerializeField] private GameObject _countdownPanel; // objet de notre canvas qui contiendra notre countdown et qui s'activera ou pas pour afficher ou pas notre countdown
    [SerializeField] private TMP_Text _countdownText;

    // comment se pluger a notre Countdown ?
    private void Start() // je m'abonne
    {
        GameEventService.OnCountdownState += HandleCountdownState;
        GameEventService.OnCountdownTick += SetCountdown; 
    }

    private void OnDestroy() // je me desabonne
    {
        GameEventService.OnCountdownState -= HandleCountdownState;
        GameEventService.OnCountdownTick -= SetCountdown;
    }

    private void HandleCountdownState(bool enterState) // est ce que je rentre dans le state ou pas
    {
        _countdownPanel.SetActive(enterState); // si je rentre dans le state alors c 'est true donc il active le panel conteant mon countdown dans mon canvas
    }


    public void SetCountdown (float countdown)
    {
        _countdownText.text = countdown.ToString("0"); // "0" : pour ne pas avoir que le premier chiffre, et non pas le chiffre qui suivent
        
        // afficher GO ! a la place de 0
        if (countdown < 1)
        {
            _countdownText.text = "GO !";
        }


    }
}
