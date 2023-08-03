using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObject_SO kitchenObject_SO;

    private IKitchenObjectParents kitchenObjectParent;

    public KitchenObject_SO GetKitchenObject()
    {
        return kitchenObject_SO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParents kitchenObjectParent
        )
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
            Debug.Log("Has obj");
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParents GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    
}
