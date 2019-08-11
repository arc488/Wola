using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Gun defaultGun = null;
    [SerializeField] Gun equippedGun = null;
    [SerializeField] Transform gunFlashPoint = null;

    GameObject gunInstance;
    GameObject player;
    Vector3 rayOrigin;
    Animator animator;
    AudioSource audioSource;
    float timeSinceLastShot = 0f;
    bool isReloading = false;

    private void Awake()
    {

        player = gameObject;

    }

    private void Start()
    {
        if (equippedGun == null)
        {
            EquipGun(defaultGun);
            animator = gunInstance.GetComponent<Animator>();
            audioSource = gunInstance.GetComponent<AudioSource>();
        }

    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        if (Input.GetButton("Fire1") && !isReloading)
        {
            if (timeSinceLastShot > equippedGun.GetRateOfFire())
            {
                Fire();
            }
        }

        
        if (Input.GetButton("Reload"))
        {
            isReloading = true;
            Reload();
        }
    }

    public void EquipGun(Gun gun)
    {
        equippedGun = gun;
        gunInstance = equippedGun.EquipGun(gun, transform.position);
        gunInstance.transform.parent = gameObject.transform;
        animator = gunInstance.GetComponent<Animator>();
        audioSource = gunInstance.GetComponent<AudioSource>();
        gunFlashPoint = gunInstance.transform.Find("FlashPoint");
    }

    private void Reload()
    {
        animator.SetTrigger("DoReload");
    }

    private void Fire()
    {
        RaycastHit hit;

        audioSource.Play();
        animator.SetTrigger("Shoot");
        timeSinceLastShot = 0f;

        equippedGun.PlayMuzzleFlash(gunFlashPoint);

        if (Physics.Raycast(rayOrigin, camera.transform.forward, out hit, 50f))
        {
            CauseDamage(hit);
            equippedGun.PlayImpactEffects(hit.point);
        }
    }



    private void CauseDamage(RaycastHit hit)
    {
        Health target = hit.transform.GetComponent<Health>();
        if (target == null) return;
        target.TakeDamge(equippedGun.GetDamage());
    }
}
