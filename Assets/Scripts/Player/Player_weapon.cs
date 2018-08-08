using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_weapon : MonoBehaviour
{
    public AudioSource[] allAudioSources;
    public Image ultiMeter;
    public Transform shotSpawnPosition;
    public GameObject bullet;
    public GameObject ultimateBullet;
    public GameManager gM;
    public Animator anim;
    public Transform spawnSpreadUltimate;
    public int playerIndex;
    public int spreadUltimateNumber = 8;
    public int bulletForce = 250;
    public int ultimateBulletForce = 1500;
    public bool animReady;

    private float ultimateProfit = 0.02f;
    private float shootFrequency = 0.14f;
    private float shootTimer = 0;
    private bool defaultFire = true;
    private bool facingRight;
    private float moveX;
    private float moveY;

    void Start()
    {
        allAudioSources = this.GetComponents<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("SwitchShot_" + this.playerIndex))
        {
            defaultFire = !defaultFire;
        }

        if (Input.GetButton("Fire_" + this.playerIndex))
        {
            //if (!allAudioSources[0].isPlaying && defaultFire)
            //{
            //    allAudioSources[0].Play();
            //}else if(!allAudioSources[1].isPlaying && !defaultFire)
            //{
            //    allAudioSources[1].Play();
            //}
            anim.SetBool("isShooting", true);
            Shoot();
        }
        if (Input.GetButtonUp("Fire_" + this.playerIndex))
        {
            anim.SetBool("isShooting", false);
        }

        if (Input.GetButtonDown("Ultimate_" + this.playerIndex) && !gM.paused && !gM.noInput && ultiMeter.fillAmount == 1)
        {
            anim.SetTrigger("Ultimate");
        }

        RotateWeapon();
    }

    private void Shoot()
    {
        facingRight = gameObject.GetComponentInParent<Player>().FacingRight;

        shootTimer += Time.deltaTime;
        if (!gM.paused && !gM.noInput)
        {
            if (shootTimer > shootFrequency)
            {
                int inv = facingRight ? 1 : -1;

                Vector3 spawn = shotSpawnPosition.transform.position;
                if (defaultFire)
                {
                    GameObject bulletInstance = Instantiate(bullet, spawn, Quaternion.identity);
                    bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
                    bulletInstance.GetComponent<Player_bullet>().Initialize(Player_bullet.Type.NORMAL);
                }
                else
                {
                    GameObject[] spreadBullets = new GameObject[3];

                    float moveY = Input.GetAxisRaw("Vertical_" + this.playerIndex);
                    float moveX = Input.GetAxisRaw("Horizontal_" + this.playerIndex);

                    float bulletAngle = -0.2f;

                    for (int i = 0; i < spreadBullets.Length; i++)
                    {
                        spreadBullets[i] = Instantiate(bullet, spawn, Quaternion.identity);
                        if (moveY <= -0.75f)
                        {
                            spreadBullets[i].GetComponent<Rigidbody>().AddForce((transform.forward + new Vector3(bulletAngle, 0, 0)) * bulletForce);
                        }
                        else if (moveY <= -0.5f && moveX > 0.1f && !facingRight || moveY <= -0.5f && moveX < -0.1f && !facingRight)
                        {
                            spreadBullets[i].GetComponent<Rigidbody>().AddForce((transform.forward + new Vector3(-bulletAngle / 2, -bulletAngle / 2, 0)) * bulletForce);
                        }
                        else if (moveY <= -0.5f && moveX > 0.1f && facingRight || moveY <= -0.5f && moveX < -0.1f && facingRight)
                        {
                            spreadBullets[i].GetComponent<Rigidbody>().AddForce((transform.forward + new Vector3(bulletAngle / 2, -bulletAngle / 2, 0)) * bulletForce);
                        }
                        else
                        {
                            spreadBullets[i].GetComponent<Rigidbody>().AddForce((transform.forward + new Vector3(0, bulletAngle, 0)) * bulletForce);
                        }
                        spreadBullets[i].GetComponent<Player_bullet>().Initialize(Player_bullet.Type.SPREAD);
                        bulletAngle += 0.2f;
                    }
                }
                shootTimer -= shootFrequency;
            }
        }
    }

    public void UltimateShot()
    {
        facingRight = gameObject.GetComponentInParent<Player>().FacingRight;

        int inv = facingRight ? 1 : -1;
        Vector3 spawn = shotSpawnPosition.transform.position;

        ultiMeter.fillAmount = 0;
        GameObject ultimateBulletInstance = Instantiate(ultimateBullet, spawn, Quaternion.identity);
        ultimateBulletInstance.GetComponent<Rigidbody>().AddForce(new Vector3(inv, 0, 0) * ultimateBulletForce);
    }

    void RotateWeapon()
    {
        float moveY = Input.GetAxisRaw("Vertical_" + this.playerIndex);
        float moveX = Input.GetAxisRaw("Horizontal_" + this.playerIndex);

        if (moveY <= -0.75f)
        {
            anim.SetBool("aim90°", true);
            anim.SetBool("aim45°", false);
            this.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }
        else if (moveY <= -0.5f && moveX > 0.1f || moveY <= -0.5f && moveX < -0.1f)
        {
            anim.SetBool("aim90°", false);
            anim.SetBool("aim45°", true);
            this.transform.localEulerAngles = new Vector3(-30, 0, 0);
        }
        else
        {
            anim.SetBool("aim45°", false);
            anim.SetBool("aim90°", false);
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

}
