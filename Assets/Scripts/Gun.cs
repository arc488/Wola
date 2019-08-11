using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/CreateNewGun", order = 1)]
public class Gun : ScriptableObject
{
    [SerializeField] GameObject gunPrefab;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] float rateOfFire = 1f;
    [SerializeField] float weaponDamage = 1f;



    public GameObject EquipGun(Gun gun, Vector3 gunPosition)
    {
        return Instantiate(gunPrefab, gunPosition, Quaternion.identity);

    }

    public float GetRateOfFire()
    {
        return rateOfFire;
    }

    public float GetDamage()
    {
        return weaponDamage;
    }

    public void PlayMuzzleFlash(Transform gunFlashPoint)
    {
        
        GameObject flash = Instantiate(muzzleFlash, gunFlashPoint);
        float flashDuration = muzzleFlash.GetComponentInChildren<ParticleSystem>().main.duration;
        Destroy(flash, flashDuration);
    }

    public void PlayImpactEffects(Vector3 hit)
    {
        GameObject bulletSpark = Instantiate(impactEffect);
        bulletSpark.transform.position = hit;
        float duration = bulletSpark.GetComponentInChildren<ParticleSystem>().main.duration;
        Destroy(bulletSpark, duration);
    }

}