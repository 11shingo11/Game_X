using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isWalking;
    private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;


    


    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f , inputVector.y);
        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = Time.deltaTime * moveSpeed;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up*playerHeight, playerRadius, moveDir, moveDistance );
        if (canMove)
        {
            transform.position += moveDir *moveDistance;
        }
        //transform.position += moveDir * Time.deltaTime * moveSpeed;
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);//rotate player
    }

    public bool IsWalking()
    {
        return isWalking;//this bool is for animation of walking
    }
}
