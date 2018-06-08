using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioSource[] allMyAudioSources;
    public GameObject gameManager;
    public GameObject landingParticles;
    public GameObject walkingParticles;
    public GameObject playerHpUI;
    public GameObject ghost;
    public GameObject spawnPlayer;
    public Animator anim;
    public MeshRenderer ren;
    public Rigidbody rb;
    public float jumpForce = 200f;
    public float dashVelocity = 5;
    public float moveVelocity = 5;
    public int dashRange = 1;
    public int playerIndex = 0;
    public int hp = 3;

    private AudioSource audioSource;
    private Vector3 dashStart;
    private Vector3 dashDestination;
    private float dashLimit;
    private float completion = 0f;
    private float dashTimer;
    private bool isDucking;
    private bool jumping;
    private bool dashing;
    private bool grounded;

    private bool facingRight;
    public bool FacingRight {
        get {
            return facingRight;
        }

        set {
            facingRight = value;
        }
    }

    private void Start()
    {
        allMyAudioSources = GetComponents<AudioSource>();
    }

    private void Update()
    {
        playerHpUI = GameObject.Find("HP_" + this.playerIndex);
        playerHpUI.GetComponent<Text>().text = "HP:" + this.hp;
        InputManager();
    }

    private void InputManager()
    {
        if(!gameManager.GetComponent<GameManager>().paused)
        {
            if (Input.GetButtonDown("Dash_" + this.playerIndex))
        {
            allMyAudioSources[0].Play();

            int inv = facingRight ? dashRange : -dashRange;

            completion = 0f;
            dashing = true;
            dashStart = this.transform.position;
            dashDestination = new Vector3(transform.position.x + inv, transform.position.y, transform.position.z);
        }

            if (Input.GetButtonDown("Jump_" + this.playerIndex) && grounded == true)
        {
            jumping = true;
        }

            RotateCharacter();

            Movement();

            Death();

            Duck();

            Dash();
        }
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision col)
    {
        //Grounded check
        if (col.gameObject.CompareTag("Ground"))
        {
            this.grounded = true;
            Instantiate(landingParticles, this.transform.position, this.transform.rotation);
        }
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
                GameObject spawnedPlayer = Object.Instantiate(spawnPlayer, col.gameObject.transform.position, this.gameObject.transform.rotation);
                spawnedPlayer.GetComponent<Rigidbody>().AddForce(0, 250, 0);
            }
            else
            {
                GameObject spawnedPlayer = Object.Instantiate(spawnPlayer, col.gameObject.transform.position, this.gameObject.transform.rotation);
                spawnedPlayer.GetComponent<Rigidbody>().AddForce(0, 250, 0);
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
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("bulletEnemy") || col.gameObject.CompareTag("PeanutMissile"))
        {
            allMyAudioSources[1].Play();
            StartCoroutine("Flash");
            StartCoroutine("InvincibleFrames");
        }
    }

    private void Death()
    {
        if (hp <= 0)
        {
            Instantiate(ghost, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void RotateCharacter()
    {
        //rotate character
        if (facingRight) {
            this.transform.localEulerAngles = new Vector3(0, 90, 0);
        } else {
            this.transform.localEulerAngles = new Vector3(0, 270, 0);
        }
    }

    private void Duck()
    {
        float moveY;
        moveY = Input.GetAxis("Vertical_" + this.playerIndex);

        if (moveY >= 0.75f && grounded)
        {
            anim.SetBool("Ducking", true);
            isDucking = true;
        }
        else if(moveY <= 0.75f)
        {
            isDucking = false;
            anim.SetBool("Ducking", false);
        }

    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal_" + this.playerIndex); //use horizontal-axis for player-movement
         

        if (!isDucking)
        {
            this.rb.velocity = new Vector3(this.moveVelocity * moveX, this.rb.velocity.y, 0);
        }
        else
        {
            this.rb.velocity = new Vector3(0 , this.rb.velocity.y, 0);
        }

        if (moveX > 0.1f) facingRight = true;
        if (moveX < -0.1f) facingRight = false;


        if (jumping)
        {
            this.rb.AddForce(0, jumpForce, 0);
            jumping = false;
        }
    }   //jump higher while holding button- fix

    private void Dash()
    {
        if(dashing)
        {
            transform.position = Vector3.Lerp(dashStart, dashDestination, completion);
            completion += Time.deltaTime * dashVelocity;
            if(completion >= 1)
            {
                dashing = false;
            }
        }
    } //BUUUUUGG

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
            GetComponentInChildren<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
            ren.enabled = true;
            GetComponentInChildren<MeshRenderer>().enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }
}
