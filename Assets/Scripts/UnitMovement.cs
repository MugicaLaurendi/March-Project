
using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed = 2;
    public Rigidbody2D rb ;

    public Transform unitTransform ;
    public Camera mainCamera ;
      
    // public Animator animator;

    public SpriteRenderer spriteRenderer;
    private Vector3 velocity = Vector3.zero ;

    public Vector3 targetPosition ;

   
    void Awake()
    {
        targetPosition = unitTransform.position ;
    }


    // Update is called once per frame
    void Update()
    {

        MoveTo(targetPosition);

        Flip(rb.velocity.x);

        //float characterVelocity = Mathf.Abs(rb.velocity.x);
        //animator.SetFloat("Speed",characterVelocity);
    }

    //methods

    void MoveTo(Vector3 _targetPosition)
    {
        Vector3 currentPosition = unitTransform.position ;
        Vector2 movementValue =  _targetPosition - currentPosition;

        if(  -0.1 < movementValue.x && movementValue.x < 0.1 && -0.1 < movementValue.y && movementValue.y < 0.1 )
        {
            rb.velocity = new Vector3(0,0,0) ;
        }
        else
        {
           rb.velocity = movementValue.normalized * moveSpeed;
        }
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