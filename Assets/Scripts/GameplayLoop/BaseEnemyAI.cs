using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour

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
    public float RandomTurnDeviation;

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



    private void Awake()
    {
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

    public IEnumerator ShootCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(fireRate);
        isReady = true;
    }


    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {


        float time = 0;
        Vector3 startPos = Trail.transform.position;

        while (time <= 1)
        {
            Trail.transform.position = Vector3.Lerp(Muzzle.position, hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = hit.point;

        float DistanceFromImpact = Vector3.Distance(hit.point, targetPositionTransform.position);
        if(DistanceFromImpact < enemyWeaponRad)
        {
            targetPositionTransform.GetComponent<PlayerHandler>().TakeDamage(DamgeToPlayer);
        }

        GameObject impact = Instantiate(explosionFX) as GameObject;
        
        impact.transform.position = hit.point;
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


    void Update()
    {

        if (Time.time > nextCoordsTime)
        {
            nextCoordsTime = Time.time + Random.Range(3, 10);
            circle = Random.insideUnitCircle.normalized * (targetRadius*Random.Range(0.5f, 3f));

        }

        if (Vector3.Distance(targetPositionTransform.GetComponent<PlayerController>().explosionpoint, transform.position) < targetPositionTransform.GetComponent<PlayerController>().currentWeaponRadius)
        {

            targetPositionTransform.GetComponent<PlayerController>().explosionpoint = new Vector3(0, 1000, 0);
            TakeDamage(targetPositionTransform.GetComponent<PlayerController>().damage);

        }

        if(health < 1 && dead == false)
        {
            dead = true;


            StartDropItem();
            gameHandler.remainingEnemies.Remove(gameObject);
            GameObject death = Instantiate(deathParticle);
            death.transform.position = transform.position;
            death.GetComponent<ParticleSystem>().Play();

            StartCoroutine(despawn());
            


        }

        Vector3 t = new Vector3(targetPositionTransform.position.x + circle.x, 0, targetPositionTransform.position.z + circle.y);
        newTargetPos = t;

        // get rand pos around player
        Vector3 directionOfTarget = (newTargetPos - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionOfTarget);

        float angleOfTarget = Vector3.SignedAngle(transform.forward, directionOfTarget, Vector3.up);
        float reachedTargetDist = 10f;
        float targetDist = Vector3.Distance(transform.position, newTargetPos);

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

            if (angleOfTarget > 0)
            {
                turnInput = 1f + RandomTurnDeviation;
            }
            else
            {
                turnInput = -1f - RandomTurnDeviation;
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

        }
        else
        {
            moveInput = 0f;
            turnInput = 0f;
        }

        // avoid eachother


        // shoot









        if (isReady == true && dead == false && targetPositionTransform.GetComponent<PlayerHandler>().playerDead == false)
        {
            StartCoroutine(ShootCooldown());


            float maxRange = 100f;
            RaycastHit hit;

            Vector3 spread = new Vector3(
                Random.Range(-SpreadVariance, SpreadVariance),
                targetPositionTransform.position.y,
                Random.Range(-SpreadVariance, SpreadVariance)

                );
            if(Physics.Raycast(Muzzle.position, ((targetPositionTransform.position - Muzzle.position) + spread)/2, out hit, maxRange))
            {
                TrailRenderer bulletTrail = Instantiate(BulletTrail);
                StartCoroutine(SpawnTrail(bulletTrail, hit));
            }
        }







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


        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, groundLayer);

        if (isCarGrounded)
        {
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);

        }
        else
        {
            sphereRB.AddForce(transform.up * -9.81f);
        }




        //weapon stuff

        Weapon.transform.LookAt(targetPositionTransform.position);
    }
}
