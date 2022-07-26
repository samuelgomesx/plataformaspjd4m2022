using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUIController : MonoBehaviour
{
    // referemcia para o objeto de texto 
   [SerializeField] private TMP_Text coinText;

   private void OnEnable()
   {
       // se inscreve no canal de coins
       PlayerObserveManager.OnCoinsChanged += UpdateCoinText;
   }

   private void OnDisable()
   {
       //retira a inscrição no canal de coins
       PlayerObserveManager.OnCoinsChanged -= UpdateCoinText;
   }
    
   // função usada para trocar a notificão do canal
   // de coins
   private void UpdateCoinText(int newCoinsvalue)
   {
       coinText.text = newCoinsvalue.ToString();
   }
}
