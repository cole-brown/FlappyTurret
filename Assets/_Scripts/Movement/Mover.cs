using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
   private Rigidbody2D rb;

   public bool useForwardDirection = false;
   public Vector2 direction;
   public float speed;

   public void Start()
   {
      rb = GetComponent<Rigidbody2D>();

      if (useForwardDirection)
         rb.velocity = transform.right * speed; // right is the forward of 2D... yes.
      else
         rb.velocity = direction * speed;
   }

   public void StopMoving()
   {
      rb.velocity = Vector2.zero;
   }
}
