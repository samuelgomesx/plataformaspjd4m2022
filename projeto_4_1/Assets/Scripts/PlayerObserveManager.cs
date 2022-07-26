using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//youtube
//modificador static diz que pode ser ucessado
// de qualquer lugar do código
public static class PlayerObserveManager 

{
   // canal da variavel coins PlayerController
   // 1 - parte da inscrição
   public static Action<int> OnCoinsChanged;
   // 2 - parte do sininho (notificação)
   public static void CoinsChanged(int value)
   {
      // Existe alguém inscrito em OnCXoinsChanged?
      // caso tenha, mande o value para todos
      OnCoinsChanged?.Invoke(value);
   }

}
