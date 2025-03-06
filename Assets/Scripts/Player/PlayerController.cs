using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setup")]
    public LayerMask playerMask;
    public float AirDrag;
    public float groundDrag;

    private float moveInput;
    private float turnInput;

    public ParticleSystem ParticleEmit1;
    public ParticleSystem ParticleEmit2;

    public ParticleSystem DriftEmit1;
    public ParticleSystem DriftEmit2;

    private bool drifting = false;

    private Vector3 lookAtPoint;


    public GameObject sfx;
    private bool startedCountdown = false;

    public AudioSource tireScreech;

    public Transform CarBody;

    public AudioSource reloadSound;

    public Transform FLwheel;
    public Transform FRwheel;
    public Transform BLwheel;
    public Transform BRwheel;


    public Transform FrontWheelCentre;
    public Transform BackWheelCentre;

    public float wheelRadius;
    [Header("Weapon Setup")]

    private Vector3 explosionPos;
    public AudioSource ShotSound;

    public TrailRenderer BulletTrail;
    public TrailRenderer CurrentBullet;
    public RaycastHit CurrentShotPosition;
    public GameObject explosionFX;
    public Transform Muzzle;
    public Transform Weapon;

    public bool NextShotReady = false;
    public bool isReady = true;

    
    [Header("Weapon Stats")]

    public float fireRate = 0.01f;
    public float nextFire = 0f;
    public float damage = 10f;
    public float bulletSpeed = 1f;
    public float currentWeaponRadius = 10f;


    [Header("Game Setup")]

    public GameObject OutOfBoundsExplosionEffect;
    public GameObject OutOfBoundsMissile;
    private bool OutOFBoundsDamageGiven = false;
    public LayerMask groundLayer;
    public Rigidbody sphereRB;
    public Transform targetOfDeath;
    public Rigidbody CarCollider;
    public Camera mainCam;
    public Transform groundPlane; 
    public GameObject shatteredCube;
    public float AreaSize_Radius = 2000f;
    public Vector3 explosionpoint;
    private float GameTime = 0f;
    private bool takeZoneDamage = false;

    public LayerMask defaultLayers;
    private bool isReloading = false;

    public float gravity;
    public Transform shotOrigin;

    private int waveCubeHealth = 10;
    private float currentLeanAngle = 0f;
    [SerializeField]
    private Transform UI;

    private bool isAllowedToMove;

    public Transform centreOfMass;
    public Transform LeftNode;
    public Transform RightNode;
    public Transform UpNode;
    public float recoverySpeed;

    public LayerMask shatteredPiece;
    [Header("Player Stats")]

    public float dashStrength = 10f;
    public float dashTime = 1f;

    private float shiftAbiltyReadyTime = 0f;
    public float ShiftAbilityCooldown = 0.5f;

    public float driftSpeed = 10f;
    public float DriftTurnSpeed = 1.7f;
    public bool isDead = false;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    

    private float TurningMultiplier;
    private float lastHitDistance = 0f;

    [Header("Seperate Weapons")]

    public GameObject FlakGun;
    public GameObject Missiles;
    public GameObject Gatling;

    public GameObject FlakMuzzle;
    public GameObject MissileMuzzle;
    public GameObject GatlingMuzzle;

    public GameObject FlakWeapon;
    public GameObject MissilesWeapoon;
    public GameObject GatlingWeapon;

    public AudioSource FlakSound;
    public AudioSource MissileSound;
    public AudioSource GatleSound;

    public TrailRenderer FlakTrail;
    public TrailRenderer MissileTrail;
    public TrailRenderer GatleTrail;

    
    void Start()
    {
        currentWeaponRadius = StaticGameVariables.WeaponRadius;
        sphereRB.transform.parent = null;
        //CarCollider.transform.parent = null;
        
        if(StaticGameVariables.WeaponEquipped == 0)
        {
            Gatling.SetActive(true);
            Missiles.SetActive(false);
            FlakGun.SetActive(false);

            Muzzle = GatlingMuzzle.transform;
            Weapon = GatlingWeapon.transform;

            BulletTrail = GatleTrail;
            fireRate = 0.2f;
            damage = 3;

            StaticGameVariables.ClipsSize = 60;
            StaticGameVariables.BulletsInClip = 60;
            StaticGameVariables.reloadTime = 5;
            StaticGameVariables.WeaponRadius = 3f;
            StaticGameVariables.Accuracy = 5f;

            ShotSound = GatleSound;



        }
        if (StaticGameVariables.WeaponEquipped == 1)
        {
            Gatling.SetActive(false);
            Missiles.SetActive(true);
            FlakGun.SetActive(false);

            Muzzle = MissileMuzzle.transform;
            Weapon = MissilesWeapoon.transform;

            BulletTrail = MissileTrail;
            fireRate = 0.1f;
            damage = 75;
            

            StaticGameVariables.ClipsSize = 12;
            StaticGameVariables.BulletsInClip = 12;
            StaticGameVariables.reloadTime = 8;
            StaticGameVariables.WeaponRadius = 12f;
            StaticGameVariables.Accuracy = 10f;

            ShotSound = MissileSound;
        }
        if (StaticGameVariables.WeaponEquipped == 2)
        {
            Gatling.SetActive(false);
            Missiles.SetActive(false);
            FlakGun.SetActive(true);

            Muzzle = FlakMuzzle.transform;
            Weapon = FlakWeapon.transform;

            BulletTrail = FlakTrail;
            fireRate = 0.4f;
            damage = 6;
            StaticGameVariables.ClipsSize = 30;
            StaticGameVariables.BulletsInClip = 30;
            StaticGameVariables.reloadTime = 7;
            StaticGameVariables.WeaponRadius = 7f;
            StaticGameVariables.Accuracy = 2f;

            ShotSound = FlakSound;
        }

    }

    public IEnumerator ShootCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(fireRate);
        isReady = true;
    }
    private IEnumerator shiftAbility(float input)
    {


        
        float cooldownSpeed = ShiftAbilityCooldown;

        if(Time.time > shiftAbiltyReadyTime)
        {
            ParticleEmit1.Play();
            ParticleEmit2.Play();



            shiftAbiltyReadyTime = Time.time + cooldownSpeed;

            sphereRB.velocity = transform.forward * dashStrength;
            //sphereRB.AddForce((dashStrength * (sphereRB.velocity.normalized + (transform.forward*moveInput))), ForceMode.Force);

            UI.GetComponent<UI_Handler>().ShiftAbilityValue.value = 0;
            

            yield return new WaitForSeconds(dashTime);
            

        }



    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {

        shotOrigin.position = Muzzle.transform.position;
        float time = 0;
        Vector3 startPos = Trail.transform.position;

        while (time <= 1)
        {
            Trail.transform.position = Vector3.Lerp(shotOrigin.position, hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }

        

        explosionpoint = hit.point;
        Trail.transform.position = hit.point;

        
        GameObject impact = Instantiate(explosionFX) as GameObject;
        impact.transform.position = hit.point;
        if(hit.collider.tag == "WaveCube")
        {
            waveCubeHealth -= 1;
            if(waveCubeHealth <= 0)
            {
                Destroy(hit.collider.gameObject);
                var ShatteredWaveCube = Instantiate(shatteredCube);
                ShatteredWaveCube.transform.position = new Vector3(0, 7.76f, 0);
            }
        }
        yield return new WaitForSeconds(1);
        explosionpoint = new Vector3(0,1000,0);



    }

    private IEnumerator reload(float time)
    {

        reloadSound.pitch = Random.Range(0.9f, 1.1f);
        reloadSound.PlayOneShot(reloadSound.clip);
        yield return new WaitForSeconds(time);
        StaticGameVariables.BulletsInClip = StaticGameVariables.ClipsSize;
        isReloading = false;

        
    }

    private IEnumerator StartDrifting()
    {
        DriftEmit1.Play();
        DriftEmit2.Play();
        tireScreech.Play();
        yield return null;
    }

    private IEnumerator EndDrifting()
    {
        DriftEmit1.Stop();
        DriftEmit2.Stop();
        tireScreech.Stop();
        yield return null;
    }

    private IEnumerator OutOfBoundsDamageCooldown()
    {
        
        yield return new WaitForSeconds(1);
        OutOFBoundsDamageGiven = false;
        float randangle = Random.Range(-1f, 1f) * Mathf.PI * 2;



        float spawnCricleX = 50 * Mathf.Cos(randangle);
        float spawnCricleZ = 50 * Mathf.Sin(randangle);

        Vector3 missilePos = new Vector3(transform.position.x + spawnCricleX, transform.position.y + 75, transform.position.z + spawnCricleZ);
        GameObject missile = Instantiate(OutOfBoundsMissile);
        missile.transform.position = missilePos;

        

        float endCircleX = 1 * Mathf.Cos(randangle);
        float endCircleZ = 1 * Mathf.Sin(randangle);

        Vector3 missileEndPoint = new Vector3(transform.position.x + endCircleX, transform.position.y + 3, transform.position.z + endCircleZ);

        float time = 0;


        while (time <= 1)
        {
            missile.transform.position = Vector3.Lerp(missilePos, missileEndPoint, time);
            time += Time.deltaTime / missile.GetComponent<TrailRenderer>().time;
            yield return null;
        }

        GameObject explosionEffect = Instantiate(OutOfBoundsExplosionEffect);
        explosionEffect.transform.position = missileEndPoint;

        CameraShaker.Instance.ShakeOnce(12, 12, 0.1f, 1f);


    }
    // Update is called once per frame
    void Update()
    {

        transform.position = sphereRB.transform.position;
        transform.rotation = sphereRB.transform.rotation;
        isDead = transform.GetComponent<PlayerHandler>().playerDead;

        float Dist = Vector3.Distance(transform.position, new Vector3(0,-0.6f,0));

        if(Dist > AreaSize_Radius && OutOFBoundsDamageGiven == false)
        {
            OutOFBoundsDamageGiven= true;
            StartCoroutine(OutOfBoundsDamageCooldown());
            StaticGameVariables.playerHealth -= 10;

            

        }

        if (isDead == false)
        {
            moveInput = Input.GetAxisRaw("Vertical");

            turnInput = Input.GetAxis("Horizontal");
            if (Input.GetMouseButton(0))
            {

                if (isReady == true && StaticGameVariables.BulletsInClip > 0)
                {
                    float Range = 1000;
                    StartCoroutine(ShootCooldown());
                    Ray shotAT = mainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(shotAT, out RaycastHit ShotHit,~playerMask))
                    {
                        RaycastHit HitWithCollision;
                        Vector3 PointWithDeviation = new Vector3(
                            Random.Range(-StaticGameVariables.Accuracy, StaticGameVariables.Accuracy),
                            0,
                            Random.Range(-StaticGameVariables.Accuracy, StaticGameVariables.Accuracy)
                            );


                        if (Physics.Raycast(Muzzle.transform.position,((ShotHit.point + PointWithDeviation) - Muzzle.transform.position), out HitWithCollision, Range, defaultLayers)){

                            ShotSound.pitch = Random.Range(0.9f, 1.1f);

                            ShotSound.PlayOneShot(ShotSound.clip);

                            TrailRenderer bulletTrail = Instantiate(BulletTrail);
                            
                            StartCoroutine(SpawnTrail(bulletTrail, HitWithCollision));
                            StaticGameVariables.BulletsInClip -= 1;
                            CameraShaker.Instance.ShakeOnce(1.5f, 3, 0.1f, 1f);
                        }
                        
                    }
                    NextShotReady = true; // tells BulletHandler to handle bullet
                }
                else
                {
                    if(StaticGameVariables.BulletsInClip <= 0 && isReloading == false)
                    {
                        isReloading = true;
                        StartCoroutine(reload(StaticGameVariables.reloadTime));
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(shiftAbility(moveInput));
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


        /// game stuf
        /// 



    }



    private void FixedUpdate(){

        sphereRB.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        //sets car's pos to the sphere's pos
        //transform.position = sphereRB.transform.position;

        //sets rotation

        if(sphereRB.velocity.magnitude < 30){
            TurningMultiplier = sphereRB.velocity.magnitude*0.1f;
        }
        else{
            TurningMultiplier = 30f*0.1f;
        }


        
       

        //turning

        if (sphereRB.velocity.magnitude > 0f && StaticGameVariables.grounded == true)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime * TurningMultiplier;

            sphereRB.transform.Rotate(0, newRotation, 0, Space.World);

        }

        // recovery

        RaycastHit left;
        bool LeftRay;
        RaycastHit right;
        bool RightRay;
        RaycastHit up;
        bool UpRay;

        LeftRay = Physics.Raycast(LeftNode.position, -LeftNode.right, out left, 0.5f, ~playerMask);
        RightRay = Physics.Raycast(RightNode.position, RightNode.right, out right, 0.5f, ~playerMask);
        UpRay = Physics.Raycast(UpNode.position, UpNode.up, out up, 0.5f, ~playerMask);



        // Ground Check


       
        



       

       

        // recovery force acting
        if (left.collider != null && StaticGameVariables.grounded == false)
        {
            sphereRB.AddForceAtPosition(-recoverySpeed * RightNode.up, RightNode.position, ForceMode.Acceleration);

        }
        if (right.collider != null && StaticGameVariables.grounded == false)
        {
            sphereRB.AddForceAtPosition(-recoverySpeed * LeftNode.up, LeftNode.position, ForceMode.Acceleration);

        }
        if (up.collider != null && StaticGameVariables.grounded == false)
        {
            sphereRB.AddForceAtPosition(-(2f * recoverySpeed) * LeftNode.up, LeftNode.position, ForceMode.Acceleration);

        }



        // movement

        if (StaticGameVariables.grounded == false)
        {
            
            turnInput = 0f;
            moveInput = 0f;
            sphereRB.drag = AirDrag;


        }
        else
        {
            
            
            if(drifting != true)
            {

                sphereRB.drag = 2f;
                sphereRB.AddForce((sphereRB.transform.forward * moveInput), ForceMode.Acceleration);

            }
            else
            {
                sphereRB.drag = 2f;
                turnInput = turnInput / (1 / DriftTurnSpeed);
            }

            if ((StaticGameVariables.grounded ==  true) && ((sphereRB.velocity.magnitude > 20) && ((Input.GetKey(KeyCode.LeftControl) && ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))))))
            {

                if (DriftEmit1.isEmitting == false)
                {
                    StartCoroutine(StartDrifting());
                }
                drifting = true;

                sphereRB.AddForce(transform.forward * (driftSpeed * (sphereRB.velocity.magnitude * 0.1f)), ForceMode.Acceleration);
                turnInput = turnInput / (1 / DriftTurnSpeed);




            }
            else
            {
                if (DriftEmit1.isEmitting == true)
                {
                    StartCoroutine(EndDrifting());

                }

                drifting = false;

            }



        }

        


        //transform.rotation = CarCollider.rotation;

        //CarRb.MoveRotation(transform.rotation);
        // SHoot at ray

        if (Input.GetMouseButtonDown(0))
        {
            
        }

        // AIm at
        Ray AimAT = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(AimAT, out RaycastHit AimHit))
        {

            lookAtPoint = AimHit.point;
            Weapon.transform.LookAt(lookAtPoint);
        }

        Vector3 Direction = (AimHit.point - transform.position).normalized * 10f;


        
        






    }


    
}
