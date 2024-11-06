using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum MoveState
    {
        up = 1,
        down = 2,
        left = 3,
        right = 4
    }

    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    Animator animator;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        Move();
        UpdateMoveState();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    public void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void UpdateMoveState()
    {
        if (animator.GetBool("Run"))
        {
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                animator.SetBool("Run", false);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("AnimationState", (int)MoveState.left);
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("AnimationState", (int)MoveState.right);
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("AnimationState", (int)MoveState.up);
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("AnimationState", (int)MoveState.down);
            animator.SetBool("Run", true);
        }
    }
}
