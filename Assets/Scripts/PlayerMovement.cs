
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed ;
    public Rigidbody2D rb ;
      
    // public Animator animator;

    public SpriteRenderer spriteRenderer;
    private Vector3 velocity = Vector3.zero ;

   



    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {


        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime ;
        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime ;
        

        MovePlayer(horizontalMovement,verticalMovement);

        Flip(rb.velocity.x);

        //float characterVelocity = Mathf.Abs(rb.velocity.x);
        //animator.SetFloat("Speed",characterVelocity);

    }


    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, _verticalMovement) ;
        rb.velocity = Vector3.SmoothDamp(rb.velocity,targetVelocity,ref velocity, .05f) ;

    }

    void Flip(float _velocity)
    {
        if(_velocity>0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if(_velocity<-0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}

