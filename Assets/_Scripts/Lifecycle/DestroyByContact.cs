using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
   public Collider2D ignored;
   public string ignoredTag;

   public void OnTriggerEnter2D(Collider2D other)
   {
      DestroyMyself(other);
   }

   public void OnCollisionEnter2D(Collision2D other)
   {
      DestroyMyself(other.collider);
   }

   private void DestroyMyself(Collider2D other)
   {
      if (ignored != null && other == ignored)
         return;

      if (!string.IsNullOrEmpty(ignoredTag) && other.tag == ignoredTag)
         return;

      if (other.tag == "Boundary")
         return; // ignore boundary - it will kill us

      // We hit something so now we're done with life.
      Destroy(gameObject);
   }
}
