using UnityEngine;
using System.Collections;

public class BoostGravity : MonoBehaviour
{
   private bool scored = false;
   
   public float time = 5f;
   public float gravityScale = 2f;

   public string flavorText = "Fly like a rock!";

   public void OnTriggerEnter2D(Collider2D other)
   {
      if (other.tag != "Player" || scored)
         return;

      scored = true;
      PlayerController player = GameController.singleton.GetPlayer();
      player.BoostGravity(time, gravityScale);
      GameController.singleton.FlavorText(flavorText);
   }
}
