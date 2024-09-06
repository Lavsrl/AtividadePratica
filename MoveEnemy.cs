using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    public float speed = 15f;
    private bool isTurning = false;
    private Collider2D yellowCarCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    void Update()
    {
        if (!isTurning)
        {
            Vector2 direction = currentPoint.position - transform.position;
            rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed, 0);
            transform.rotation = Quaternion.Euler(0f, direction.x > 0 ? 0f : 180f, 0f);

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
            {
                currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YellowCar"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y) && !isTurning)
            {
                yellowCarCollider = collision.collider; 
                StartCoroutine(HandleCollision());
            }
        }
    }

    IEnumerator HandleCollision()
    {
        rb.velocity = Vector2.zero; 
        isTurning = true;

        yield return new WaitForSeconds(3.0f);

        if (yellowCarCollider != null)
        {
            Physics2D.IgnoreCollision(yellowCarCollider, GetComponent<Collider2D>(), false);
            yellowCarCollider = null; 
        }

        yield return new WaitForSeconds(1.0f); 
        isTurning = false; 
        rb.velocity = new Vector2(currentPoint == pointB.transform ? speed : -speed, 0);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YellowCar"))
        {
            if (yellowCarCollider != null)
            {
                Physics2D.IgnoreCollision(yellowCarCollider, GetComponent<Collider2D>(), false);
                yellowCarCollider = null; 
            }
        }
    }
}
