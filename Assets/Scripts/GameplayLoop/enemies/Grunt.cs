using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : MonoBehaviour
{

    GameHandler gameHandler;

    public float health = 120f;

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

    public int SpreadVariance = 10;

    public bool NextShotReady = false;

    public bool isReady = true;


    public float targetRadius;
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
    public int WeaponRange = 500;






    public int maxAmmo = 30;
    public int Ammo = 30;
    public int reloadSpeed = 3;

    public Transform debugLineOfSight;
    private float dot = 0;
    private float angleOfTarget = 0;
    private float targetDist = 0;
    private float NewTargetDist = 0;
    private float radius = 0;
    private bool FindLongerRoute;
    private Vector3 LastGivenPosition;
    private Vector3 LastGivenDirection;
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

    private IEnumerator despawn()
    {
        yield return new WaitForSeconds(cleanup);
        Destroy(sphereRB);
        Destroy(gameObject);
    }

    private IEnumerator shoot(TrailRenderer Trail, RaycastHit hit)
    {

        float time = 0;
        Vector3 startPos = Trail.transform.position;
        Vector3 spread = new Vector3(
                    Random.Range(-SpreadVariance, SpreadVariance),
                    targetPositionTransform.position.y,
                    Random.Range(-SpreadVariance, SpreadVariance)

                    );

        Vector3 hitWithSpread = hit.point + spread;

        while (time <= 1)
        {
            Trail.transform.position = Vector3.Lerp(Muzzle.position, hitWithSpread, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = hitWithSpread;

        Ammo -= 1;


        float DistanceFromImpact = Vector3.Distance(hitWithSpread, targetPositionTransform.position);
        if (DistanceFromImpact < enemyWeaponRad)
        {
            targetPositionTransform.GetComponent<PlayerHandler>().TakeDamage(DamgeToPlayer);
        }

        GameObject impact = Instantiate(explosionFX) as GameObject;

        impact.transform.position = hitWithSpread;
    }

    public IEnumerator ShootCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(fireRate);
        isReady = true;
    }
    //Assets/Scripts/ItemDropPivotPoint.prefab
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

    public IEnumerator Reload()
    {
        isReady = false;
        yield return new WaitForSeconds(reloadSpeed);
        Ammo = maxAmmo;
        isReady = true;
    }

    public IEnumerator GetNewTargetPos()
    {
        newTargetPos = new Vector3(targetPositionTransform.position.x + circle.x, 0, targetPositionTransform.position.z + circle.y);
        yield return null;

    }

    void Update()
    {

        if (newTargetPos == null)
        {

            newTargetPos = transform.position;
        }

        // calculates next location to where car must go to 
        if (Time.time > nextCoordsTime)
        {

            nextCoordsTime = Time.time + 1;
            circle = Random.insideUnitCircle.normalized * (targetRadius);
            StartCoroutine(GetNewTargetPos());
        }

        if (Vector3.Distance(targetPositionTransform.GetComponent<PlayerController>().explosionpoint, transform.position) < StaticGameVariables.WeaponRadius)
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




        /// ------- NEW AI ALGORYTHM
        /// 1. check if there is line of sight
        /// 2. if there is just drive forward, if not fire a circle of rays around the raycast hit

        // get rand pos around player
        Vector3 directionOfTarget = (targetPositionTransform.position - transform.position).normalized;
        

        //for turning
        

        //for driving forward/back

        float reachedTargetDist = 10f;
        targetDist = Vector3.Distance(transform.position, targetPositionTransform.position);

        RaycastHit LineOfSight;

        Physics.Raycast(transform.position, directionOfTarget, out LineOfSight);

        Debug.DrawRay(transform.position, directionOfTarget * targetDist);
        if(LineOfSight.collider.tag == "Player")
        {

            LastGivenPosition = targetPositionTransform.position;
            LastGivenDirection = directionOfTarget;

            FindLongerRoute = false;
            
        }
        else
        {
            // shoot rays in a cirlce to create a valid path
            int numOfRays = 50;
            


            for(int i = numOfRays; i >= 0 ; i--)
            {
                float angle = i * Mathf.PI * 2 / numOfRays;

                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
                direction = Quaternion.Euler(90, 0, 0) * direction;

                RaycastHit hitInfo;

                Physics.Raycast(LineOfSight.point, direction, out hitInfo, radius);
                if(hitInfo.collider == null)
                {

                    RaycastHit NewLineOfSight;
                    Ray ray = new Ray(LineOfSight.point, direction);

                    Physics.Raycast(ray, out NewLineOfSight, radius);
                    Vector3 Endpoint = ray.GetPoint(radius);

                    Vector3 directionOfTargetFromEndPoint = (targetPositionTransform.position - Endpoint).normalized;
                    float targetDistFromEndPoint = Vector3.Distance(Endpoint, targetPositionTransform.position);

                    RaycastHit FinalPos;
                    Physics.Raycast(Endpoint, directionOfTargetFromEndPoint, out FinalPos, targetDistFromEndPoint);
                    
                    if(FinalPos.collider.tag == "Player" && (targetDistFromEndPoint < targetDist || FindLongerRoute == true))
                    {
                        
                        Debug.DrawRay(LineOfSight.point, direction * radius, Color.green);
                        Debug.DrawRay(Endpoint, directionOfTargetFromEndPoint * targetDistFromEndPoint, Color.red);
                        

                        float distToEndpoint = Vector3.Distance(transform.position, Endpoint);
                        Vector3 DirectionToEndPoint = (Endpoint - transform.position).normalized;


                        LastGivenDirection = DirectionToEndPoint;
                        LastGivenPosition = Endpoint;
                        NewTargetDist = distToEndpoint;

                        radius = 0;
                        break;
                    }
                    else
                    {
                        FindLongerRoute = true;
                        radius += 1f;
                    }

                    

                    

                    


                }
                

            }

        }

        dot = Vector3.Dot(transform.forward, LastGivenDirection);
        angleOfTarget = Vector3.SignedAngle(transform.forward, LastGivenDirection, Vector3.up);
        NewTargetDist = targetDist;

        // follow player



        if (NewTargetDist > reachedTargetDist && dead == false)
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
            StartCoroutine(GetNewTargetPos());
        }

        // know if it can shoot
        // get aim position
        // fire at aim position
        //reload


        if (isReady == true && dead == false && targetPositionTransform.GetComponent<PlayerHandler>().playerDead == false)
        {
            if (Ammo > 0)
            {




                float maxRange = 300f;
                RaycastHit hit;

                // the ray's distance is the range check for the weapon
                if (Physics.Raycast(Muzzle.position, targetPositionTransform.position - Muzzle.position, out hit, WeaponRange))
                {


                    TrailRenderer bulletTrail = Instantiate(BulletTrail);
                    StartCoroutine(shoot(bulletTrail, hit));
                    StartCoroutine(ShootCooldown());

                }

            }
            else
            {
                StartCoroutine(Reload());
            }

        }
        Weapon.transform.LookAt(targetPositionTransform.position);

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
        sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);




    }
}
