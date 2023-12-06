using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public AudioClip deadClip;
    public float jumpForce = 700f;
    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() { 
        if (isDead) {
            return;
        }
        
        if (Input.GetMouseButtonDown(0) && jumpCount < 2) {
            jumpCount++;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpForce));
            audioSource.Play();
        }
        else if (Input.GetMouseButtonUp(0) && rb.velocity.y > 0) {
            rb.velocity = rb.velocity * 0.5f;
        }

        animator.SetBool("Grounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Dead" &&  !isDead) {
            Die();       
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.y > 0.7f) {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
    private void Die() {
        animator.SetTrigger("Die");
        audioSource.clip = deadClip;
        audioSource.Play();

        rb.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }
}
