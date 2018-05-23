using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Image ultiMeter;
    public Transform shotSpawnPosition;
    public GameObject bullet;
    public GameObject ultimateBullet;
    public Transform spawnSpreadUltimate;
    public int playerIndex;
    public int spreadUltimateNumber = 8;
    public int bulletForce = 250;

    private AudioSource audio;
    private float ultimateProfit = 0.02f;
    private float shootFrequency = 0.14f;
    private float shootTimer = 0;
    private bool defaultFire = true;
    private bool facingRight;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("SwitchShot_" + this.playerIndex))
        {
            defaultFire = !defaultFire;
        }

        if (Input.GetButton("Fire_" + this.playerIndex))
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }

            Shoot();
        }

        if (Input.GetButtonDown("Ultimate_"+this.playerIndex))
        {
            UltimateShot();
        }
    }

    public void UltimateShot()
    {
        facingRight = gameObject.GetComponentInParent<Player>().FacingRight;

        int inv = facingRight ? 1 : -1;

        if(ultiMeter.fillAmount == 1 && defaultFire)
        {
            Vector3 spawn = shotSpawnPosition.transform.position;

            ultiMeter.fillAmount = 0;
            GameObject ultimateBulletInstance = Instantiate(ultimateBullet, spawn, Quaternion.identity);
            ultimateBulletInstance.GetComponent<Rigidbody>().AddForce(new Vector3(inv, 0, 0) * bulletForce);
        }

        if(ultiMeter.fillAmount == 1 && !defaultFire)
        {
            GameObject[] spreadUltimateBullets = new GameObject[spreadUltimateNumber];

            float bulletAngleInc = Mathf.Deg2Rad * (360 / spreadUltimateNumber);

            float bulletAngle = 0f;

            ultiMeter.fillAmount = 0;

            for(int i = 0; i < spreadUltimateBullets.Length; i++)
            {
                spreadUltimateBullets[i] = Instantiate(ultimateBullet, spawnSpreadUltimate.position, Quaternion.identity);
                spreadUltimateBullets[i].GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle), 0) * bulletForce);
                spreadUltimateBullets[i].GetComponent<UltimateBullet>().Initialize(UltimateBullet.Type.SPREAD);
                bulletAngle += bulletAngleInc;
            }
        }
        
    }

    private void Shoot()
    {
        facingRight = gameObject.GetComponentInParent<Player>().FacingRight;

        shootTimer += Time.deltaTime;

        if (shootTimer > shootFrequency)
        {

            int inv = facingRight ? 1 : -1;

            Vector3 spawn = shotSpawnPosition.transform.position;
            if (defaultFire)
            {
                GameObject bulletInstance = Instantiate(bullet, spawn, Quaternion.identity);
                bulletInstance.GetComponent<Rigidbody>().AddForce(new Vector3(inv, 0, 0) * bulletForce);
                bulletInstance.GetComponent<Bullet>().Initialize(Bullet.Type.NORMAL);
            }
            else
            {
                GameObject[] spreadBullets = new GameObject[3];

                float bulletAngle = -0.3f;

                for (int i = 0; i < spreadBullets.Length; i++)
                {
                    spreadBullets[i] = Instantiate(bullet, spawn, Quaternion.identity);
                    spreadBullets[i].GetComponent<Rigidbody>().AddForce(new Vector3(inv, bulletAngle, 0) * bulletForce);
                    spreadBullets[i].GetComponent<Bullet>().Initialize(Bullet.Type.SPREAD);
                    bulletAngle += 0.3f;
                }
            }
            shootTimer -= shootFrequency;
        }

    }
}
