using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Shooting : MonoBehaviour
{
    public Transform firePoint; 
    public GameObject[] bulletTypes;
    public float bulletDelay; 
    [SerializeField] private float[] shootTimer; 
    private Vector3 mousePosition;
    [SerializeField] private bool canShoot = true, canCurrentlyFire = true;

    [Header("[SwappingGuns]")]
    public Weapon[] myWeapons; 
    [SerializeField] private int currentGun = 1;
    [SerializeField] private int maxWeapons = 2;
    public Sprite[] weaponSprites;
    public Image[] weaponUI;
    public TMP_Text ammoText;
    [SerializeField] private float[] ammo;

    /*
    [SerializeField] private float[] gunBulletDelays;
    [SerializeField] private float[] bulletSpreadRange;

    [SerializeField] private float[] maxAmmo;
    [SerializeField] private int[] weaponProjectileNumber;

    public int[] currentMinDamage;
    public int[] currentMaxDamage;
    */ 
    private const float radius = 1f;

    private AudioManager myAudioManager;
    [SerializeField] private Sounds[] mySounds;
    [SerializeField] private Sounds[] ammoSounds;
    [SerializeField] private Sounds[] gainAmmoSounds; 
    public float shakeIntensity, shakeTime; 

    private void Start()
    {
        myWeapons[0].maxAmmo = Mathf.Infinity;

        for (int i = 0; i < myWeapons.Length; i++)
        {
            PlayerObjectPool.instance.bulletPrefab[i] = myWeapons[i].bulletType;

            PlayerObjectPool.instance.SetBulletIDs(i, myWeapons[i].bulletID); 
        }

        PlayerObjectPool.instance.BeginPooling(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (myAudioManager == null)
        {
            myAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        UpdateAmmo(); 

        MouseRotation(); 
        
        if (!canShoot)
        {
            //Begin adding real time
            shootTimer[currentGun - 1] += Time.deltaTime; 

            if (shootTimer[currentGun - 1] > bulletDelay)
            {
                canShoot = true;
                shootTimer[currentGun - 1] = 0; 
            }
        }
        
        if (Input.GetButton("Fire1") && canShoot)
        {
            if (canCurrentlyFire)
            {
                canShoot = false;
                Shoot();
            }

        }

        SwapWeaponUI(); 
        
    }


    void UpdateAmmo()
    {
        if (currentGun == 1)
        {
            ammoText.text = "9999"; 
        }
        else
        {
            if (ammo[currentGun - 1] < 10)
            {
                ammoText.text = "000" + ammo[currentGun - 1]; 
            }
            else if (ammo[currentGun - 1] < 100)
            {
                ammoText.text = "00" + ammo[currentGun - 1]; 
            }
            else if (ammo[currentGun - 1] < 1000)
            {
                ammoText.text = "0" + ammo[currentGun - 1]; 
            }
            else
            {
                ammoText.text = "" + ammo[currentGun - 1]; 
            }
        }
    }

    void MouseRotation()
    {
        //Get the Screen position of the mouse
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePosition - transform.position;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    void Shoot()
    {
        bulletDelay = myWeapons[currentGun-1].gunBulletDelay;

        if (currentGun == 1)
        {
            myAudioManager.Play(mySounds[0]);

            //GameObject temp = Instantiate(bulletTypes[0], firePoint.position, Quaternion.identity);
            GameObject temp = PlayerObjectPool.instance.GetPooledObject(0); 
            if (temp != null)
            {
                temp.transform.position = firePoint.position;
                temp.transform.rotation = Quaternion.identity;
                temp.SetActive(true); 
            }

            temp.GetComponent<Bullet>().myAudioManager = myAudioManager; 
            temp.GetComponent<Bullet>().bulletDamage = Random.Range(myWeapons[currentGun - 1].currentMinDamage, myWeapons[currentGun - 1 ].currentMaxDamage);
            temp.GetComponent<Bullet>().InstantiateDirection(); 

            CineMachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime); 
        }
        else if (ammo[currentGun - 1] > 0)
        {
            myAudioManager.Play(mySounds[currentGun - 1]);
            SpawnProjectiles(myWeapons[currentGun - 1].weaponProjectileNumber);
            CineMachineShake.Instance.ShakeCamera(shakeIntensity, shakeTime);
            ammo[currentGun - 1] -= 1; 
        }
        else
        {
            Debug.Log("No ammo!"); 
        }
    }

    void SwapWeaponUI()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (maxWeapons == 1)
            {
                currentGun = 1; 
            }
            else
            {
                //swap out weapons
                //save the first one so we can put it as the last one
                Sprite firstWeaponSprite = weaponSprites[0]; 

                for (int i = 0; i < maxWeapons; i++)
                {
                    //if last weapon
                    if (i == (maxWeapons - 1))
                    {
                        weaponSprites[i] = firstWeaponSprite;
                        weaponUI[i].sprite = firstWeaponSprite; 
                    }
                    else
                    {
                        weaponSprites[i] = weaponSprites[i + 1];
                        weaponUI[i].sprite = weaponSprites[i]; 
                    }
                }

                if (currentGun == maxWeapons)
                {
                    currentGun = 1; 
                }
                else
                {
                    currentGun++;
                }

            }
        }
    }

    //Spawns x number of projectiles
    void SpawnProjectiles(int _numberProjectiles)
    { 

        //if numProj is even
        if ((_numberProjectiles % 2) == 0)
        {
            float multAngle = myWeapons[currentGun - 1].bulletSpreadRange / (_numberProjectiles / 2);
            float currentAngle = myWeapons[currentGun - 1].bulletSpreadRange; 

            for (int i = 0; i < (_numberProjectiles / 2); i++)
            {
                Quaternion spread = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);
                Quaternion spread2 = firePoint.rotation * Quaternion.Euler(0, 0, -currentAngle);

                //Debug.Log(currentAngle);

                //GameObject tmpObject = Instantiate(bulletTypes[currentGun - 1], firePoint.position, spread);

                GameObject tmpObject = PlayerObjectPool.instance.GetPooledObject(currentGun - 1);
                if (tmpObject != null)
                {
                    tmpObject.transform.position = firePoint.position;
                    tmpObject.transform.rotation = spread; 
                    tmpObject.SetActive(true);
                }

                //GameObject tmpObject2 = Instantiate(bulletTypes[currentGun - 1], firePoint.position, spread2);
                GameObject tmpObject2 = PlayerObjectPool.instance.GetPooledObject(currentGun - 1);
                if (tmpObject2 != null)
                {
                    tmpObject2.transform.position = firePoint.position;
                    tmpObject2.transform.rotation = spread2;
                    tmpObject2.SetActive(true);
                }


                Rigidbody2D rb = tmpObject.GetComponent<Rigidbody2D>();
                Rigidbody2D rb2 = tmpObject2.GetComponent<Rigidbody2D>();

                tmpObject.GetComponent<Bullet>().bulletDamage = Random.Range(myWeapons[currentGun - 1].currentMinDamage, myWeapons[currentGun - 1].currentMaxDamage);
                tmpObject.GetComponent<Bullet>().myAudioManager = myAudioManager;
                tmpObject2.GetComponent<Bullet>().bulletDamage = Random.Range(myWeapons[currentGun - 1].currentMinDamage, myWeapons[currentGun - 1].currentMaxDamage);
                tmpObject2.GetComponent<Bullet>().myAudioManager = myAudioManager;

                rb.velocity = rb.transform.right * tmpObject.GetComponent<Bullet>().bulletForce;
                rb2.velocity = rb2.transform.right * tmpObject.GetComponent<Bullet>().bulletForce;

                currentAngle -= multAngle;
            }
        }
        else
        {
            //odd # of bullets

            int loopNum = (_numberProjectiles / 2) + 1;
            float multAngle = myWeapons[currentGun - 1].bulletSpreadRange / (_numberProjectiles / 2);
            float currentAngle = myWeapons[currentGun - 1].bulletSpreadRange;

            for (int i = 0; i < loopNum; i++)
            {
                if (i == 0)
                {
                    Quaternion spread = firePoint.rotation * Quaternion.Euler(0, 0, 0);

                    //GameObject temp = Instantiate(bulletTypes[currentGun - 1], firePoint.position, spread);

                    GameObject temp = PlayerObjectPool.instance.GetPooledObject(0);
                    if (temp != null)
                    {
                        temp.transform.position = firePoint.position;
                        temp.transform.rotation = Quaternion.identity;
                        temp.SetActive(true);
                    }

                    Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
                    temp.GetComponent<Bullet>().bulletDamage = Random.Range(myWeapons[currentGun - 1].currentMinDamage, myWeapons[currentGun - 1].currentMaxDamage);
                    rb.velocity = rb.transform.right * temp.GetComponent<Bullet>().bulletForce;
                }
                else
                {
                    Quaternion spread = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);
                    Quaternion spread2 = firePoint.rotation * Quaternion.Euler(0, 0, -currentAngle);

                    //Debug.Log(currentAngle);

                    //GameObject tmpObject = Instantiate(bulletTypes[currentGun - 1], firePoint.position, spread);

                    GameObject tmpObject = PlayerObjectPool.instance.GetPooledObject(currentGun - 1);
                    if (tmpObject != null)
                    {
                        tmpObject.transform.position = firePoint.position;
                        tmpObject.transform.rotation = spread;
                        tmpObject.SetActive(true);
                    }

                    //GameObject tmpObject2 = Instantiate(bulletTypes[currentGun - 1], firePoint.position, spread2);
                    GameObject tmpObject2 = PlayerObjectPool.instance.GetPooledObject(currentGun - 1);
                    if (tmpObject2 != null)
                    {
                        tmpObject2.transform.position = firePoint.position;
                        tmpObject2.transform.rotation = spread2;
                        tmpObject2.SetActive(true);
                    }

                    Rigidbody2D rb = tmpObject.GetComponent<Rigidbody2D>();
                    Rigidbody2D rb2 = tmpObject2.GetComponent<Rigidbody2D>();

                    tmpObject.GetComponent<Bullet>().bulletDamage = Random.Range(myWeapons[currentGun - 1].currentMinDamage, myWeapons[currentGun - 1].currentMaxDamage);
                    tmpObject.GetComponent<Bullet>().myAudioManager = myAudioManager;
                    tmpObject2.GetComponent<Bullet>().bulletDamage = Random.Range(myWeapons[currentGun - 1].currentMinDamage, myWeapons[currentGun - 1].currentMaxDamage);
                    tmpObject2.GetComponent<Bullet>().myAudioManager = myAudioManager;

                    rb.velocity = rb.transform.right * tmpObject.GetComponent<Bullet>().bulletForce;
                    rb2.velocity = rb2.transform.right * tmpObject.GetComponent<Bullet>().bulletForce;

                    currentAngle -= multAngle;
                }
            }
        }
    }

    public void AddAmmo(int gunNumber)
    {
        float checkAmmo = ammo[gunNumber] + 1;

        if (checkAmmo <= myWeapons[gunNumber].maxAmmo)
        {
            ammo[gunNumber]++;
            myAudioManager.Play(ammoSounds[gunNumber - 1]);
        }

    }

    public void GainAmmo(int gunNumber)
    {
        float checkAmmo = ammo[gunNumber] + 1;

        if (checkAmmo <= myWeapons[gunNumber].maxAmmo)
        {
            ammo[gunNumber]++;
            myAudioManager.Play(gainAmmoSounds[gunNumber - 1]);
        }
    }

    public bool IsAmmoAtMax(int gunNumber)
    {
        if (ammo[gunNumber] >= myWeapons[gunNumber].maxAmmo)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }

}
