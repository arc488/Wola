using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;

    bool isDead = false;
    Animator animator;

    public void TakeDamge(float damage)
    {
        health -= damage;
    }

    private void Start()
    {
        if (this.tag == "Enemy")
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            Die();
        }       
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("die");
           
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
