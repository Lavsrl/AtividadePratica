using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Vector2 direcao;
    public float velocidade = 10f;
    public Animator animator;
    public Rigidbody2D rb;
    private Collider2D redCarCollider;

    private bool isCollidingWithEnemy;

    void Start()
    {
        isCollidingWithEnemy = false;
    }

    void Update()
    {
        if (!isCollidingWithEnemy)
        {
            float axisX = Input.GetAxisRaw("Horizontal");
            float axisY = Input.GetAxisRaw("Vertical");

            bool isParado = animator.GetCurrentAnimatorStateInfo(0).IsName("Parado");

            if (isParado && axisY < 0)
            {
                axisY = 0;
            }

            direcao = new Vector2(axisX, axisY);

            if (direcao != Vector2.zero)
            {
                animator.SetFloat("XInput", axisX);
                animator.SetFloat("YInput", axisY);
                animator.SetBool("Movendo", true);
            }
            else
            {
                animator.SetBool("Movendo", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isCollidingWithEnemy)
        {
            rb.MovePosition(rb.position + direcao * velocidade * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedCar"))
        {   
            Vector2 collisionNormal = collision.contacts[0].normal;
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
            {
                redCarCollider = collision.collider;
                StartCoroutine(HandleCollision());
            }

            isCollidingWithEnemy = true;
            animator.SetBool("Movendo", false);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | 
                             RigidbodyConstraints2D.FreezePositionY | 
                             RigidbodyConstraints2D.FreezeRotation;
        }
    }

    IEnumerator HandleCollision()
    {
        Physics2D.IgnoreCollision(redCarCollider, GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(1.0f); 
        Physics2D.IgnoreCollision(redCarCollider, GetComponent<Collider2D>(), false);
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedCar"))
        {
            isCollidingWithEnemy = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
