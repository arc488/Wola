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
    Color initialColor;
    // Start is called before the first frame update
    void Start()
    {
        initialColor = bloodEffect.color;
        currentHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        initialColor.a = (startHealth - currentHealth) / 100;
        bloodEffect.color = initialColor;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

}
