using UnityEngine;
using System.Collections;

public class ScoreByPosition : MonoBehaviour
{
   // don't care about Y, it's all about the X
   public float minXPos = 0.0f;

   public int scoreValue = 1;
   private bool scored = false;

   private void FixedUpdate ()
   {
      if (scored || transform.position.x > minXPos)
         return;

      scored = true;
      GameController.singleton.AddScore(scoreValue);
   }
}
