using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
   public GameObject turretSpawnPoint;
   public GameObject upTurretPrefab;
   public GameObject downTurretPrefab;
   public Vector2 upMinMax;
   public Vector2 downMinMax;

   public GameObject[] pickupPrefabs;
   public Vector2 pickupMinMax;

   private PlayerController player;

   private int score;
   private int bestScore;
   private bool gameOver;
   private bool gameStarted;

   // Turrets
   public float initialSpawnDelay = 3.5f;
   public float nextSpawnDelay = 2f;
   private float nextSpawnTime;

   // Pickups
   public float initialPickupSpawnDelay = 6.5f;
   public float nextPickupSpawnDelay = 2f;
   private float nextPickupSpawnTime;

   public Text scoreText;
   public Text pickupText;
   public Button restartButton;

   public static GameController singleton;
   private void Awake()
   {
      singleton = this;
   }
   private void OnDestroy()
   {
      if (singleton == this)
         singleton = null;
   }

	private void Start()
   {
      score = 0;
      gameOver = false;
      gameStarted = false;
      restartButton.gameObject.SetActive(false);
      pickupText.text = string.Empty;

      UpdateScore();
   }

   public void RegisterPlayer(PlayerController player_)
   {
      player = player_;
   }

   private void Update()
   {
      // do nothing until game in started
      if (!gameStarted)
         return;

      // don't spawn turrets after game is over
      if (!gameOver)
      {
         SpawnTurret();
         SpawnPickup();
      }
   }

   private void SpawnTurret()
   {
      if (Time.time < nextSpawnTime)
         return;

      nextSpawnTime = Time.time + nextSpawnDelay;

      Vector2 minMax = upMinMax;
      GameObject prefab = upTurretPrefab;
      if (Random.value > 0.5f)
      {
         minMax = downMinMax;
         prefab = downTurretPrefab;
      }

      Vector3 spawnPos = new Vector3(turretSpawnPoint.transform.position.x,
                                     Random.Range(minMax.x, minMax.y),
                                     0f);
      GameObject turret = Instantiate(prefab, spawnPos, prefab.transform.rotation) as GameObject;
      turret.transform.parent = turretSpawnPoint.transform;
   }

   private void SpawnPickup()
   {
      if (Time.time < nextPickupSpawnTime)
         return;

      nextPickupSpawnTime = Time.time + nextPickupSpawnDelay;

      if (Random.value > 0.25f)
      {
         Debug.Log("No pickup.");
         return;
      }

      if (pickupPrefabs == null || pickupPrefabs.Length < 1)
      {
         Debug.Log("No pickup prefabs. " + pickupPrefabs);
         return;
      }

      GameObject prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length - 1)];
      if (prefab == null)
      {
         Debug.Log("null prefab " + prefab);
      }

      Vector3 spawnPos = new Vector3(turretSpawnPoint.transform.position.x,
                                     Random.Range(pickupMinMax.x, pickupMinMax.y),
                                     0f);
      GameObject pickup = Instantiate(prefab, spawnPos, prefab.transform.rotation) as GameObject;
      pickup.transform.parent = turretSpawnPoint.transform;
   }

   public void StartGame()
   {
      gameStarted = true;
      nextSpawnTime = Time.time + initialSpawnDelay;
      nextPickupSpawnTime = Time.time + initialPickupSpawnDelay;
   }

   public void AddScore(int value)
   {
      score += value;
      UpdateScore();
      Debug.Log("score: " + score);
   }

   private void UpdateScore()
   {
      string text =  "Score: " + score;
      if (gameOver)
         text += "\nBest: " + bestScore;
      
      scoreText.text = text;
   }

   public void FlavorText(string text)
   {
      pickupText.text = text;
      // pickupText.color = Color.white; // does not fix alpha
      pickupText.CrossFadeAlpha(1f, 0f, false); // Cuz who needs a sane solution to get alpha back?.. Yay.
      pickupText.CrossFadeAlpha(0f, 1f, false); // Fade out.
   }

   public void OnRestartClick()
   {
      Debug.Log("Reload level for restart.");
      Application.LoadLevel(Application.loadedLevel);
   }

   public Vector3 GetPlayerPos()
   {
      return player.transform.position;
   }

   public PlayerController GetPlayer()
   {
      return player;
   }

   public void PlayerKilled()
   {
      if (gameOver)
         return;

      Debug.Log("PlayerKilled! Game Over!");

      gameOver = true;

      // tell PlayerController it's dead
      player.Die();

      // tell turrets to stop moving
      foreach (Mover turret in turretSpawnPoint.GetComponentsInChildren<Mover>())
         turret.StopMoving();

      // bullets can die on their own

      // get/set high score
      bestScore = PlayerPrefs.GetInt("High Score");
      if (score > bestScore)
      {
         bestScore = score;
         PlayerPrefs.SetInt("High Score", bestScore);
      }

      UpdateScore();
      
      restartButton.gameObject.SetActive(true);
   }
   
}
