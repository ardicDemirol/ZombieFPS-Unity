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

    private bool isAttacking = false;

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
        anim = mainCamera.GetChild(0).GetComponent<Animator>();

        if (currentWeapon != null)
        {
            SpawnWeapon();
        }
   
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
        currentWeapon.SpawnNewWeapon(mainCamera.transform.GetChild(0).GetChild(0),anim);
    }

    public void EquipWeapon(Weapon weaponType)
    {
        if(currentWeapon != null)
        {
            currentWeapon.Drop();
        }

        currentWeapon = weaponType;
        SpawnWeapon();
    }

    private void Attack()
    {
        if (Mouse.current.leftButton.isPressed && !isAttacking)
        {
            StartCoroutine(AttackRoutine());    
        }

    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentWeapon.GetAttackRate);
        isAttacking = false;
    }

    public int GetDamage()
    {
        if(currentWeapon != null)
        {
            return currentWeapon.GetDamage;
        }
        return 0;
    }
}
