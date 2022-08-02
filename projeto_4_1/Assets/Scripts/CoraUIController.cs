using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoraUIController : MonoBehaviour
{
    // referemcia para o objeto de texto 
    [SerializeField] private TMP_Text CoraText;

    private void OnEnable()
    {
        // se inscreve no canal de Coração
        PlayerObserveManager.OnCorasChanged += UpdateCoraText;
    }

    private void OnDisable()
    {
        //retira a inscrição no canal de Coração
        PlayerObserveManager.OnCorasChanged -= UpdateCoraText;
    }
    
    // função usada para trocar a notificão do canal
    // de Coração
    private void UpdateCoraText(int newCorasvalue)
    {
        CoraText.text = newCorasvalue.ToString();
    }
}
