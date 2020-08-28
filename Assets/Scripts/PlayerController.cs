using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRb;

    private PhotonView photonView;

    private float h, v;
    
    public Transform groundCheck;
    public Collider2D standing, crouching;

    public float speed;
    public float jumpForce;

    public bool attacking;
    public bool grounded;
    public bool lookLeft;
    public int idAnimation;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();

        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);

        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        //if (photonView.IsMine)
        if (true)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            if (h > 0 && lookLeft && !attacking)
            {
                Flip();
            } else if(h < 0 && !lookLeft && !attacking)
            {
                Flip();
            }

            if (v < 0)
            {
                idAnimation = 2;//Abaixado
                if(grounded)
                {
                    h = 0;
                }
            } else if (h != 0)
            {
                idAnimation = 1;//Andando
            } else
            {
                idAnimation = 0;//Parado
            }

            if(Input.GetButtonDown("Fire1") && v >= 0 && !attacking)
            {
                playerAnimator.SetTrigger("attack");
            }

            if (Input.GetButtonDown("Jump") && grounded && !attacking)
            {
                playerRb.AddForce(new Vector2(0, jumpForce));
            }

            if (attacking && grounded)
            {
                h = 0;
            }

            if (v < 0 && grounded)
            {
                SetPlayerCollider(false);//Abaixado
            } else if(v >= 0 && grounded)
            {
                SetPlayerCollider(true);//Em pé/pulando
            }else if (v != 0 && !grounded)
            {
                SetPlayerCollider(true);//Em pé/pulando
            }

            playerAnimator.SetBool("grounded", grounded);
            playerAnimator.SetInteger("idAnimation", idAnimation);
            playerAnimator.SetFloat("speedY", playerRb.velocity.y);
        }
    }

    void Flip()
    {
        lookLeft = !lookLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public void Attack(int atk)
    {
        switch (atk)
        {
            case 0:
                attacking = false;
                break;
            case 1:
                attacking = true;
                break;
        }
    }

    private void SetPlayerCollider(bool stand)
    {
        standing.enabled = stand;
        crouching.enabled = !stand;
    }
}
