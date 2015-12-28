using UnityEngine;
using System.Collections;

public class ScoreByPickup : MonoBehaviour
{
   public int scoreValue = 2;
   private bool scored = false;

   public string flavorText = "Points! +{0} Points!";

   public void OnTriggerEnter2D(Collider2D other)
   {
      if (other.tag != "Player" || scored)
         return;

      scored = true;
      GameController.singleton.AddScore(scoreValue);
      GameController.singleton.FlavorText(string.Format(flavorText, scoreValue));
   }
}
