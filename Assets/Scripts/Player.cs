using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParents
{
    public static Player Instance { get; private set; }
    


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs: EventArgs
    {
        public BaseCounter selectedCounter;
    }


    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private bool isWalking;
    private float moveSpeed = 5f;
    private KitchenObject kitchenObject;


    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractActions;
    }

    private void GameInput_OnInteractActions(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;//this bool is for animation of walking
    }


    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if(moveDir!= Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit hit, interactDistance, counterLayerMask))
        {
            if(hit.transform.TryGetComponent(out BaseCounter baseCounter) )
            {
                //this obj has clear counter
                if(baseCounter!= selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
                  
            }
            else
            {
                SetSelectedCounter(null);
            }

        }
        else
        {
            SetSelectedCounter(null);
        }
    }


    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = Time.deltaTime * moveSpeed;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {//can move only in x axis
                moveDir = moveDirX;
            }
            else
            {//can move only in z axix
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {//can move only in x axis
                    moveDir = moveDirZ;
                }
                else
                { //cannnot move in any direction

                }
            }

        }
        
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        //transform.position += moveDir * Time.deltaTime * moveSpeed;
       
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);//rotate player
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() { kitchenObject = null; }

    public bool HasKitchenObject() { return kitchenObject != null; }
}
