using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideTaxi : MonoBehaviour
{

    GameHandler gameHandler;

    public float health = 40f;

    [SerializeField]
    private GameObject game;

    [SerializeField] private Transform targetPositionTransform;
    private Vector3 targetPosition;

    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;

    public float MaxDeviationValue;
    private int RandomTurnDeviation;

    private float TurningMultiplier;
    public LayerMask groundLayer;
    public Rigidbody sphereRB;


    public Transform Weapon;


    public float fireRate = 0.1f;
    public float nextFire = 0f;

    public TrailRenderer BulletTrail;

    public float bulletSpeed = 1f;
    public TrailRenderer CurrentBullet;
    public RaycastHit CurrentShotPosition;

    public GameObject explosionFX;
    public Transform Muzzle;

    public float SpreadVariance = 0.0f;

    public bool NextShotReady = false;

    public bool isReady = true;


    public float targetRadius = 40f;
    private float nextCoordsTime = 0f;
    private Vector2 circle;

    private Vector3 newTargetPos;
    public GameObject deathParticle;
    public bool dead = false;
    private float cleanup = 3f;

    public float DamgeToPlayer = 1;

    public float enemyWeaponRad = 10f;

    public float partDropChance = 1f;
    public float attachmentDropChance = 1f;
    public GameObject ItemDropPivot;

    private bool ReadyForNextSwurve = true;
    private int SwurveSide;
    public int swurveLength = 20;

    private float detonationTime = 0.15f;
    public ParticleSystem detonation;


    public float BlastRadius = 50f;


    private void Awake()
    {
        SwurveSide = Random.Range(0, 1);
        gameHandler = GameObject.Find("Game").GetComponent<GameHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {

        sphereRB.transform.parent = null;

        circle = Random.insideUnitCircle.normalized * targetRadius;

        targetPositionTransform = GameObject.FindWithTag("Player").transform;

    }

    public void TakeDamage(float Amount)
    {
        health -= Amount;
    }

    private IEnumerator despawn()
    {
        yield return new WaitForSeconds(cleanup);
        Destroy(sphereRB);
        Destroy(gameObject);
    }

    // Update is called once per frame

    public void StartDropItem()
    {
        // part chance
        if (Random.Range(0f, 1f) <= partDropChance)
        {
            var pivot = Instantiate(ItemDropPivot);
            pivot.transform.position = transform.position;
        }

    }
    private IEnumerator TurnDeviatation()
    {
        ReadyForNextSwurve = false;


        if(SwurveSide == 1)
        {
            
            RandomTurnDeviation = 40;

        }
        else
        {
            RandomTurnDeviation = -40;
        }
        

        yield return new WaitForSeconds(1f);

        // // this ensure that the next swurve is on the other side 
        if (SwurveSide == 1)
        {
            SwurveSide = 0;
        }
        else
        {
            SwurveSide = 1;
        }

        ReadyForNextSwurve = true;
    }

    
    private IEnumerator detonate()
    {
        yield return new WaitForSeconds(detonationTime);
        var explosion = Instantiate(detonation);
        explosion.transform.position = transform.position + transform.up *4;
        float targetDist = Vector3.Distance(transform.position, targetPosition);
        if (targetDist <= BlastRadius)
        {
            StaticGameVariables.playerHealth -= 200;
        }

        Destroy(gameObject);

    }



    void Update()
    {

        if (Time.time > nextCoordsTime)
        {
            nextCoordsTime = Time.time + Random.Range(3, 10);
            circle = Random.insideUnitCircle.normalized * (targetRadius * Random.Range(0.5f, 3f));

        }

        if (Vector3.Distance(targetPositionTransform.GetComponent<PlayerController>().explosionpoint, transform.position) < targetPositionTransform.GetComponent<PlayerController>().currentWeaponRadius)
        {

            targetPositionTransform.GetComponent<PlayerController>().explosionpoint = new Vector3(0, 1000, 0);
            TakeDamage(targetPositionTransform.GetComponent<PlayerController>().damage);

        }

        if (health < 1 && dead == false)
        {
            dead = true;


            StartDropItem();
            gameHandler.remainingEnemies.Remove(gameObject);
            GameObject death = Instantiate(deathParticle);
            death.transform.position = transform.position;
            death.GetComponent<ParticleSystem>().Play();

            StartCoroutine(despawn());



        }

        if(ReadyForNextSwurve == true)
        {
           StartCoroutine(TurnDeviatation());
        }

        targetPosition = targetPositionTransform.position;
        // get rand pos around player
        Vector3 directionOfTarget = (targetPosition - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionOfTarget);

        float reachedTargetDist = 10f;
        float CloseEnough = 100f;

        float targetDist = Vector3.Distance(transform.position, targetPosition);

        //required for turning
        float angleOfTarget = Vector3.SignedAngle(transform.forward, directionOfTarget, Vector3.up) + RandomTurnDeviation;

        // follow player
        


        if (targetDist > reachedTargetDist && dead == false)
        {
            if (dot > 0)
            {
                moveInput = 1f;
            }
            else
            {
                moveInput = -1f;
            }

            if (moveInput > 0)
            {
                // moving forward

                moveInput *= fwdSpeed;
            }
            else
            {
                // moving backwards
                moveInput *= revSpeed;
            }

            if (angleOfTarget > 0)
            {
                turnInput = 1f;
            }
            else
            {
                turnInput = -1f;
            }

        }
        else
        {
            moveInput = 0f;
            turnInput = 0f;
        }

        // avoid eachother


        // shoot









        







    }


    // enable it to move
    private void FixedUpdate()
    {
        //sets car's pos to the sphere's pos
        transform.position = sphereRB.transform.position;

        if (sphereRB.velocity.magnitude < 30)
        {
            TurningMultiplier = sphereRB.velocity.magnitude * 0.1f;
        }
        else
        {
            TurningMultiplier = 30f * 0.1f;
        }

        if (sphereRB.velocity.magnitude > 0f)
        {
            
            float newRotation = turnInput * turnSpeed * Time.deltaTime * TurningMultiplier;
            transform.Rotate(0, newRotation, 0, Space.World);


        }

        Vector3 directionOfTarget = (targetPosition - transform.position);
        float CloseEnough = 100f;

        float targetDist = Vector3.Distance(transform.position, targetPosition);

        if (targetDist < CloseEnough)
        {

            if(targetDist < 40)
            {
                StartCoroutine(detonate());
                sphereRB.AddForce(transform.forward * moveInput * 5, ForceMode.Acceleration);
                
            }
            turnSpeed = 200;
            RandomTurnDeviation = 0;
            ReadyForNextSwurve = false;
            sphereRB.AddForce(transform.forward * moveInput * 3, ForceMode.Acceleration);
        }
        else
        {
            
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }

        
        

    }
}
