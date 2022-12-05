using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;

    private Transform mainCamera;

    private Animator anim;

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
        anim = mainCamera.GetChild(0).GetComponent<Animator>();
        SpawnWeapon();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Attack();
    }

    

    private void SpawnWeapon()
    {
        if (currentWeapon == null)
        {
            return;
        }
        currentWeapon.SpawnNewWeapon(mainCamera.transform.GetChild(0).GetChild(0));
    }

    private void Attack()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            anim.SetTrigger("Attack");
            Debug.Log("fd");
        }

    }
}
