using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
   public float xMin, xMax, zMin, zMax;   
}

public class PlayerController : MonoBehaviour 
{
   private Rigidbody2D rb;

   public float force = 300f;
   private Vector2 flapForce;

   public float flapRate = 0.1f;
   private float nextFlap = 0.0f;

   public float yMin, yMax;

   public float tilt = 9f;

   // death vars
   private bool dead = false;
   private float initialRot;
   private float timeOfDeath;
   private float endOfDeathLerp;
   public float deathRotTime = 0.2f;
   public float deadRot = -90f;

   // buffs
   private float resetGravity = float.MaxValue;
   private float resetForce = float.MaxValue;

   public void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      flapForce = new Vector2(0f, force);

      GameController.singleton.RegisterPlayer(this);
   }

   public void Update()
   {
      // will have to check touches (or add touches to "Jump"?) if mobile
      // but editor isn't mobile, so space/left-click are jump for now
      if (!dead && Input.GetButtonDown("Jump") && Time.time > nextFlap)
      {
         if (rb.isKinematic)
            StartGame();

         nextFlap = Time.time + flapRate;
         rb.velocity = Vector2.zero; // gets the right feel, otherwise not bouncy enough.
         rb.AddForce(flapForce);
      }

      if (dead)
      {
         // normalize
         float normTime = Mathf.Min(1f,
                                    (Time.time - timeOfDeath) / (endOfDeathLerp - timeOfDeath));

         // lerp to dead rot
         rb.rotation = Mathf.Lerp(initialRot, deadRot, normTime);
      }
      else
      {
         rb.rotation = Mathf.Clamp(rb.velocity.y * tilt, -90f, 45f);
      }
   }

   public void FixedUpdate()
   {
      // keep player from going too far up
      rb.position = new Vector2(0f, Mathf.Clamp(rb.position.y, yMin, yMax));

      // wear off gravity buff
      if (Time.time > resetGravity)
      {
         rb.gravityScale = 1f;
         resetGravity = float.MaxValue;
      }
      if (Time.time > resetForce)
      {
         flapForce = new Vector2(0f, force);
         resetForce = float.MaxValue;
      }
   }

   public void StartGame()
   {
      // start using gravity
      rb.isKinematic = false;

      // tell controller it's go time!
      GameController.singleton.StartGame();
   }

   public void Die()
   {
      if (dead)
         return;
      
      // set dead flag
      dead = true;

      // set up lerp to make death rot look right
      timeOfDeath = Time.time;
      endOfDeathLerp = timeOfDeath + deathRotTime;
      initialRot = rb.rotation;
      rb.velocity = Vector2.zero;
   }

   public void BoostGravity(float duration, float gravityScale)
   {
      rb.gravityScale = gravityScale;
      resetGravity = Time.time + duration;
   }

   public void BoostForce(float duration, float forceMult)
   {
      flapForce = new Vector2(0f, force * forceMult);
      resetForce = Time.time + duration;
   }
}
