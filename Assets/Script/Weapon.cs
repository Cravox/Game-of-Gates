using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shotSpawnPosition;
    public GameObject bullet;

    public int bulletForce = 250;

    private int playerIndex;
    private bool facingRight;

    private float shootFrequency = 0.14f;
    private float shootTimer = 0;

    private bool defaultFire = true;

    // Use this for initialization
    void Start()
    {
        playerIndex = gameObject.GetComponentInParent<Player>().playerIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchShot_" + this.playerIndex))
        {
            defaultFire = !defaultFire;
        }

        if (Input.GetButton("Fire_" + this.playerIndex))
        {
            Shoot();
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
