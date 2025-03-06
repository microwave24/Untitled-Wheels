using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameHandler : MonoBehaviour
{
    // start wave
    // get set amount of enemies
    // wait until player dies or kills all enemies
    // end wave
    // repreat
    // 
    // also as time increases --> dif gets hard


    [SerializeField]
    private GameObject UIhandler;


    public GameObject player;
    [SerializeField]
    private GameObject enemySpawn;
    public float ArenaRadius = 140f;

    public TrailRenderer SpawnTrail;
    public ParticleSystem SpawningRing;

    // 0 = easy, 1 = normal, 2 = hard
    public int Difficulty = 0;
    public float enemyDifficultyRating = 0.0f;
    public bool waveActive = false;

    public List<GameObject> remainingEnemies;

    public int wavesBetweenGarage = 0;


    // Enemy List

    public GameObject ID1;
    public GameObject ID2;
    public GameObject ID3;
    public GameObject ID4;
    public GameObject ID5;


    void Start()
    {
    }
    private void Awake()
    {
    }

    private IEnumerator spawnEnemy(GameObject type, Vector3 position, float spawnDelay)
    {

       
        float SpawnTrailStartHeight = 70f;
        Vector3 startPos = new Vector3(position.x, SpawnTrailStartHeight, position.z);
        var trail = Instantiate(SpawnTrail);
        trail.transform.position = startPos;
        float time = 0f;
       

        while(time < .1f)
        {
            trail.transform.position = Vector3.Lerp(startPos, position, time / 0.1f);
            time += Time.deltaTime;
            yield return null;
        }
        var spawningRing = Instantiate(SpawningRing);
        spawningRing.transform.position = position;
        
        trail.transform.position = position;

        GameObject enemyPrefab;
        enemyPrefab = Instantiate(type, position, Quaternion.identity);

        remainingEnemies.Add(enemyPrefab);
    }

    private IEnumerator sendToGarage()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
    public void CalculateWave()
    {
        // get total enemy count
        int BaseEnemyCount = 4 * (StaticGameVariables.currentWave) +6;
        // split it between batches
        int batches = 0;

        if(StaticGameVariables.currentWave <= 5)
        {
            batches = Random.Range(3,5);
        }
        if (StaticGameVariables.currentWave > 5 && StaticGameVariables.currentWave <= 10)
        {
            batches = Random.Range(5, 7);
        }
        if (StaticGameVariables.currentWave > 10 && StaticGameVariables.currentWave <= 15)
        {
            batches = Random.Range(7, 9);
        }
        if (StaticGameVariables.currentWave > 15 && StaticGameVariables.currentWave <= 20)
        {
            batches = Random.Range(9, 11);
        }
        if (StaticGameVariables.currentWave > 20 && StaticGameVariables.currentWave <= 25)
        {
            batches = Random.Range(11, 13);
        }
        if (StaticGameVariables.currentWave > 25 && StaticGameVariables.currentWave <= 30)
        {
            batches = Random.Range(13, 15);
        }
        if (StaticGameVariables.currentWave > 30 && StaticGameVariables.currentWave <= 35)
        {
            batches = Random.Range(15, 17);
        }
        if (StaticGameVariables.currentWave > 35 && StaticGameVariables.currentWave <= 40)
        {
            batches = Random.Range(17, 19);
        }
        if (StaticGameVariables.currentWave > 40 && StaticGameVariables.currentWave <= 45)
        {
            batches = Random.Range(19, 21);
        }
        if (StaticGameVariables.currentWave > 45 && StaticGameVariables.currentWave <= 50)
        {
            batches = Random.Range(21, 23);
        }

        if (StaticGameVariables.currentWave > 50)
        {
            batches = Random.Range(23, 33);
        }


        int UnitsInBatch = (int)Mathf.Round(BaseEnemyCount / batches);
        int totalUnits = batches * (int)UnitsInBatch;

        StartCoroutine(spawnBatch(batches, totalUnits, UnitsInBatch));


    }
    private IEnumerator spawnBatch(int batches, int totalunits, int inBatch)
    {

        // spawn enemies

        

        int WaveEnemyycount = 0;

        waveActive = true;
        while (WaveEnemyycount < totalunits)
        {
            if(WaveEnemyycount > totalunits)
            {
                break;
                
            }
            
            float randangle = Random.Range(0f, 1f) * Mathf.PI * 2;
            float DistFromEdge = 30f;
            

            // gets a random gerenral spawning area for the enemies
            //float spawnCricleAreaX = (ArenaRadius - DistFromEdge) * Mathf.Cos(randangle);
            //float spawnCricleAreaZ = (ArenaRadius - DistFromEdge) * Mathf.Sin(randangle);
            Vector3 SpawnCricleArea = new Vector3(0, 1.5f, 0);



            for (int enemy = 0; enemy <= inBatch - 1; enemy++)
            {
                int selectedEnemy = Random.Range(0, StaticGameVariables.enemies.Length);
                yield return new WaitForSeconds(0.5f);


                // get enemy spawn position(s)

                randangle = Random.Range(0f, 1f) * Mathf.PI * 2;
                float randRadius = Random.Range(1f, 30f);

                float CurrentEnemySpawnX = randRadius * Mathf.Cos(randangle);
                float CurrentEnemySpawnz = randRadius * Mathf.Sin(randangle);

                Vector3 CurrentEnemySpawnPos = new Vector3(CurrentEnemySpawnX + 0, 1.5f, CurrentEnemySpawnz + 0);

                StartCoroutine(spawnEnemy(StaticGameVariables.enemies[selectedEnemy], CurrentEnemySpawnPos, 1f));
                WaveEnemyycount += 1;
            }
            yield return new WaitForSeconds(5f);
            


            
        }
        waveActive = false;


        
    }



    

    void Update()
    {
        
        StaticGameVariables.gameTime += Time.deltaTime;
        StaticGameVariables.difficultyTimeMultiplier = StaticGameVariables.gameTime * 0.0004f;


        if (remainingEnemies.Count <= 0 && waveActive == false)
        {
            waveActive = false;



            print("Wave ended");
            StaticGameVariables.currentWave += 1;

            UIhandler.GetComponent<UI_Handler>().WaveCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "wave: " + StaticGameVariables.currentWave.ToString();

            CalculateWave();
        }

    }
}
