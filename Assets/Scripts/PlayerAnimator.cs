using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKIKING = "IsWalking";


    [SerializeField] private Player player;


    private Animator animator;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        animator.SetBool(IS_WALKIKING, player.IsWalking());//setting bool triger for animation
    }
}
