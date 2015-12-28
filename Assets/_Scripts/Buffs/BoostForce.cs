using UnityEngine;
using System.Collections;

public class BoostForce : MonoBehaviour
{
   private bool scored = false;
   
   public float time = 5f;
   public float forceMult = 1.25f;

   public string flavorText = "Big Flaps!";

   public void OnTriggerEnter2D(Collider2D other)
   {
      if (other.tag != "Player" || scored)
         return;

      scored = true;
      PlayerController player = GameController.singleton.GetPlayer();
      player.BoostForce(time, forceMult);
      GameController.singleton.FlavorText(flavorText);
   }
}
