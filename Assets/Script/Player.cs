using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private RigidbodyConstraints rbC;

    private Animator anim;
    private Rigidbody rb;

    private MeshRenderer ren;

    public Transform shotSpawnPosition;

    private GameObject playerHP;
    private GameObject bullet;
    private GameObject spreadBullet;
    private GameObject ghost;

    public float dashForce;
    public float jumpForce = 200f;
    public float dashLength;
    public float dashTimer;
    private float velocity = 5;
    private float shootFrequency;
    private float shootTimer = 0;
    
    public bool canShoot;
    private bool defaultFire = true;
    private bool jumping;
    private bool dashing;
    private bool grounded;
    private bool facingRight;

    public int hp;
    public int playerIndex = 0;

    private void Awake()
    {
        hp = 3;
        facingRight = true;

        playerHP = GameObject.Find("HP_" + this.playerIndex); //Find HP-UI-Element

        ren = this.gameObject.GetComponent<MeshRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bullet = (GameObject)Resources.Load("Player/Shot_p" + this.playerIndex);
        spreadBullet = (GameObject)Resources.Load("Player/Spreadshot_p" + this.playerIndex);
        ghost = (GameObject)Resources.Load("Player/dead_player");
    }

    private void Update()
    {
        RotateCharacter();

        Shoot();

        Death();

        Duck();

        Dash();

        playerHP.GetComponent<Text>().text = "HP:" + this.hp;

        if (Input.GetButtonDown("Jump_" + this.playerIndex) && grounded == true)
        {
            jumping = true;
        }

        if (Input.GetButtonDown("Dash_" + this.playerIndex)) dashing = true;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision col)
    {
        //Grounded check
        if (col.gameObject.CompareTag("Ground")) this.grounded = true;
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground")) this.grounded = false;
    }

    private void OnTriggerStay(Collider col)
    {

        if (col.gameObject.CompareTag("deadPlayer") && Input.GetButtonDown("Jump_" + this.playerIndex) && !grounded)
        {

            if (playerIndex == 0)
            {
                GameObject newPlayer = (GameObject)Resources.Load("Player/Player_1");
                GameObject spawnedPlayer = Object.Instantiate(newPlayer, col.gameObject.transform.position, this.gameObject.transform.rotation);
                //GameObject prefab = Instantiate(Resources.Load("Player_1", typeof(GameObject)) as GameObject);
                spawnedPlayer.GetComponent<Rigidbody>().AddForce(0, 500, 0);
            }
            else
            {
                GameObject newPlayer = (GameObject)Resources.Load("Player/Player_0");
                GameObject spawnedPlayer = Object.Instantiate(newPlayer, col.gameObject.transform.position, this.gameObject.transform.rotation);
                //GameObject prefab = Instantiate(Resources.Load("Player_0", typeof(GameObject)) as GameObject);
                spawnedPlayer.GetComponent<Rigidbody>().AddForce(0, 500, 0);
            }

            Destroy(col.gameObject);
        }

        if(col.gameObject.CompareTag("Parry") && Input.GetButtonDown("Jump_"+this.playerIndex) && !grounded)
        {
            Destroy(col.gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("bulletEnemy"))
        {
            this.hp -= 1;
            StartCoroutine("Flash");
            StartCoroutine("InvincibleFrames");
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("SwitchShot_" + this.playerIndex)) defaultFire = !defaultFire;

        shootFrequency = 0.14f;

        shootTimer += Time.deltaTime;
        if (shootTimer > shootFrequency)
        {

            if (Input.GetButton("Fire_" + this.playerIndex))
            {
                if (defaultFire)
                {
                    Vector3 spawn = shotSpawnPosition.transform.position;

                    //GameObject bulletResource = (GameObject)Resources.Load("Prefabs/shot_p" + this.playerIndex);
                    GameObject bulletInstance = Object.Instantiate(bullet, spawn, new Quaternion(0, 0, 270, 0));

                    if (facingRight) bulletInstance.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * 250);
                    else bulletInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * 250);
                }
                else
                {
                    Vector3 spawn = shotSpawnPosition.transform.position;

                    GameObject spreadBulletInstance1 = Object.Instantiate(spreadBullet, spawn, new Quaternion(0, 0, 270, 0));
                    GameObject spreadBulletInstance2 = Object.Instantiate(spreadBullet, spawn, new Quaternion(0, 0, 270, 0));
                    GameObject spreadBulletInstance3 = Object.Instantiate(spreadBullet, spawn, new Quaternion(0, 0, 270, 0));

                    if(facingRight)
                    {
                        spreadBulletInstance1.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0.3f, 0) * 250);
                        spreadBulletInstance2.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * 250);
                        spreadBulletInstance3.GetComponent<Rigidbody>().AddForce(new Vector3(1, -0.3f, 0) * 250);
                    }else
                    {
                        spreadBulletInstance1.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0.3f, 0) * 250);
                        spreadBulletInstance2.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * 250);
                        spreadBulletInstance3.GetComponent<Rigidbody>().AddForce(new Vector3(-1, -0.3f, 0) * 250);
                    }
                }
                shootTimer = 0;
            }
        }
    }

    private void Death()
    {
        //GameObject newPlayer = (GameObject)Resources.Load("Player_"+this.playerInde   x);
        //GameObject spawnPlayer = Object.Instantiate(newPlayer, respawn, this.gameObject.transform.rotation);

        //SceneManager.LoadScene("levelSelect");
        if (hp <= 0)
        {
            Object.Instantiate(ghost, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    private void RotateCharacter()
    {
        //rotate character
        if (this.facingRight)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }

    private void Duck()
    {
        float moveY;
        moveY = Input.GetAxis("Vertical_" + this.playerIndex);

        if (moveY >= 0.75f && grounded)
        {
            anim.SetBool("Ducking", true);
            this.velocity = 0;
        }
        else
        {
            this.velocity = 5;
            anim.SetBool("Ducking", false);
        }

    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal_" + this.playerIndex); //use horizontal-axis for player-movement

        if (hp > 0) this.rb.velocity = new Vector3(this.velocity * moveX, this.rb.velocity.y, 0);

        if (moveX > 0.1f) facingRight = true;
        if (moveX < -0.1f) facingRight = false;


        if (jumping)
        {
            this.rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            jumping = false;
        }


    }   //jump higher while holding button- fix

    private void Dash()
    {
        if(Input.GetButtonDown("Dash_"+this.playerIndex))
        {
            dashTimer += Time.deltaTime;
            this.rb.useGravity = false;

            transform.Translate(Vector3.right * dashForce);

            if(dashTimer >= dashLength) this.rb.useGravity = true;
        }
        
    }

    private IEnumerator InvincibleFrames()
    {
        gameObject.layer = 15; //switch to Invincible-Layer
        yield return new WaitForSeconds(2f);
        gameObject.layer = 8; //switch back to Player-Layer
    }

    private IEnumerator Flash()
    {
        for (int i = 0; i < 10; i++)
        {
            ren.enabled = false;
            yield return new WaitForSeconds(.1f);
            ren.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }
}
