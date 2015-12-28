using UnityEngine;
using System.Collections;

public class KillOnContact : MonoBehaviour
{
   public void OnTriggerEnter2D(Collider2D other)
   {
      Kill(other);
   }
   
   public void OnCollisionEnter2D(Collision2D other)
   {
      Kill(other.collider);
   }

   public void Kill(Collider2D other)
   {
      if (other.tag != "Player")
         return;

      GameController.singleton.PlayerKilled();
   }
}
