using UnityEngine;
using System.Collections;

public class DartTrap : MonoBehaviour
{
    [Header("Cài ??t Súng")]
    public GameObject dartPrefab;       // Ô ch?a phi tiêu
    public Transform firePoint;         // Ô ch?a nòng súng
    public GameObject fireEffectPrefab; // Ô ch?a l?a (Code c? c?a m ?ang thi?u dòng này)

    public float fireRate = 1.5f;

    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Instantiate(dartPrefab, firePoint.position, firePoint.rotation);

            if (fireEffectPrefab != null)
            {
                GameObject effect = Instantiate(fireEffectPrefab, firePoint.position, firePoint.rotation);
                Destroy(effect, 0.2f);
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}