using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] GameObject sparks;
    [SerializeField] float rateOfFire = 1f;
    [SerializeField] float weaponDamage = 1f;

    Vector3 rayOrigin;
    Animator animator;
    float timeSinceLastShot = 0f;
    bool isReloading = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

        if (Input.GetButton("Fire1") && !isReloading)
        {
            if (timeSinceLastShot > rateOfFire)
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

    private void Reload()
    {
        animator.SetTrigger("DoReload");
    }

    private void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, camera.transform.forward, out hit, 50f))
        {
            animator.SetTrigger("Shoot");
            timeSinceLastShot = 0f;
            GameObject bulletSpark = Instantiate(sparks);
            bulletSpark.transform.position = hit.point;
            CauseDamage(hit);
            float duration = bulletSpark.GetComponentInChildren<ParticleSystem>().main.duration;
            Destroy(bulletSpark, duration);

        }

    }

    private void CauseDamage(RaycastHit hit)
    {
        Health target = hit.transform.GetComponent<Health>();
        if (target == null) return;
        target.TakeDamge(weaponDamage);
    }
}
