using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float startHealth = 100f;
    [SerializeField] float regenerateRate = 1f;
    [SerializeField] Image bloodEffect = null;
    [SerializeField] float currentHealth;

    bool isDead = false;
    Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        currentColor = bloodEffect.color;
        currentHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < startHealth)
        {
            currentHealth += regenerateRate;
        } 
        currentColor.a = (startHealth - currentHealth) / 100;
        bloodEffect.color = currentColor;

        if (currentHealth <= 1)
        {
            isDead = true;
        }

    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

}
