using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class Player : MonoBehaviour
{
    public AudioSource[] allMyAudioSources;
    public GameObject gameManager;
    public GameObject landingParticles;
    public GameObject walkingParticles;
    public GameObject ghost;
    public GameObject spawnPlayer;
    public Animator anim;
    public SkinnedMeshRenderer[] ren;
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
    private float startVelocity;
    private bool isDucking;
    private bool jumping;
    private bool dashing;
    private bool grounded;

    private bool facingRight = true;
    public bool FacingRight
    {
        get
        {
            return facingRight;
        }

        set
        {
            facingRight = value;
        }
    }

    private void Start()
    {
        startVelocity = moveVelocity;
        allMyAudioSources = GetComponents<AudioSource>();
        ren = this.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (!gameManager.GetComponent<GameManager>().paused && !gameManager.GetComponent<GameManager>().noInput)
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

            if (Input.GetButton("DontMove_" + this.playerIndex) && grounded)
            {
                moveVelocity = 0;
            }
            else if (Input.GetButtonUp("DontMove_" + this.playerIndex))
            {
                moveVelocity = startVelocity;
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
        //Movement();
    }

    private void OnCollisionEnter(Collision col)
    {
        //Grounded check
        if (col.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isGrounded", true);
            this.grounded = true;
            Instantiate(landingParticles, this.transform.position, this.transform.rotation);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            this.grounded = false;
            anim.SetBool("isGrounded", false);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("deadPlayer") && Input.GetButtonDown("Jump_" + this.playerIndex) && !grounded)
        {
            spawnPlayer.SetActive(true);
            spawnPlayer.transform.position = this.transform.position;
            spawnPlayer.GetComponent<Player>().hp = 1;
            spawnPlayer.GetComponent<Player>().StartCoroutine("Flash");
            spawnPlayer.GetComponent<Player>().StartCoroutine("InvincibleFrames");
            spawnPlayer.GetComponent<Rigidbody>().AddForce(0, 250, 0);
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Parry") && Input.GetButtonDown("Jump_" + this.playerIndex) && !grounded)
        {
            Destroy(col.gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("bulletEnemy") || col.gameObject.CompareTag("PeanutMissile"))
        {
            allMyAudioSources[1].Play();
            //StartCoroutine("GamePadVibration");
            StartCoroutine("Flash");
            StartCoroutine("InvincibleFrames");
        }
    }

    private void Death()
    {
        if (hp <= 0)
        {
            GamePad.SetVibration(0, 0, 0);
            Instantiate(ghost, this.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }

    private void RotateCharacter()
    {
        //rotate character
        if (facingRight)
        {
            this.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(0, 270, 0);
        }
    }

    private void Duck()
    {
        float moveY;
        moveY = Input.GetAxis("Vertical_" + this.playerIndex);

        if (moveY >= 0.75f && grounded)
        {
            anim.SetBool("isCrouching", true);
            isDucking = true;
        }
        else if (moveY <= 0.75f)
        {
            isDucking = false;
            anim.SetBool("isCrouching", false);
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
            this.rb.velocity = new Vector3(0, this.rb.velocity.y, 0);
        }

        if (moveX > 0.1f)
        {
            facingRight = true;
            anim.SetBool("isWalking", true);
        }

        if (moveX < -0.1f)
        {
            facingRight = false;
            anim.SetBool("isWalking", true);
        }
        
        if(moveX == 0)
        {
            anim.SetBool("isWalking", false);
        }

        if (jumping)
        {
            this.rb.AddForce(0, jumpForce, 0);
            jumping = false;
        }
    }   //jump higher while holding button- fix

    private void Dash()
    {
        if (dashing)
        {
            transform.position = Vector3.Lerp(dashStart, dashDestination, completion);
            completion += Time.deltaTime * dashVelocity;
            if (completion >= 1)
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
            foreach (SkinnedMeshRenderer item in ren)
            {
                item.enabled = false;
            }
            yield return new WaitForSeconds(.1f);

            foreach (SkinnedMeshRenderer item in ren)
            {
                item.enabled = true;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator GamePadVibration()
    {
        GamePad.SetVibration(PlayerIndex.One, 1, 1);
        yield return new WaitForSeconds(0.5f);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }
}
