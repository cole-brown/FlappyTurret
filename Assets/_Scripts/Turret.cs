using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
   public GameObject projectilePrefab;
   public GameObject projectileSpawnPoint;

   private Collider2D myCollider;

   public float range = 2f;
   private float rangeSqr;

   public float fireDelay = 1f;
   private float nextFire;

   private void Start()
   {
      rangeSqr = range * range;

      myCollider = GetComponent<Collider2D>();
   }
   
   private void FixedUpdate()
   {
      Vector3 playerPos = GameController.singleton.GetPlayerPos();
      if ((projectileSpawnPoint.transform.position - playerPos).sqrMagnitude > rangeSqr ||
          Time.time < nextFire)
         return;

      nextFire = Time.time + fireDelay;

      // fire and forget
      GameObject projectile = Instantiate(projectilePrefab,
                                          projectileSpawnPoint.transform.position,
                                          LookAt2D(projectileSpawnPoint.transform.position, playerPos)) as GameObject;
      DestroyByContact destructible = projectile.GetComponent<DestroyByContact>();
      destructible.ignored = myCollider;
   }

   // You'd think Unity would have this...
   private Quaternion LookAt2D(Vector2 looker, Vector2 target)
   {
      Vector3 dir = target - looker;
      float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      return Quaternion.AngleAxis(angle, Vector3.forward);
   }
}
