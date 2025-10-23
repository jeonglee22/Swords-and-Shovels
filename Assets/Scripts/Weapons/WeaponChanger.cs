using System;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    public Transform weaponHand;
    public GameObject swordPrefab;
    public GameObject fireWandPrefab;
    public GameObject iceWandPrefab;
    public GameObject forestWandPrefab;

    private GameObject currentWeapon;

    public int CurrentWeaponIndex { get; private set; } = 1;
    public event Action<int> OnWeaponChanged;

    private void Start()
    {
        ChangeWeapon(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeWeapon(4);
    }

    public void ChangeWeapon(int weaponIndex)
    {
        // 기존 무기 제거
        if (currentWeapon != null) 
            Destroy(currentWeapon);

        GameObject prefabToSpawn = null;

        switch (weaponIndex)
        {
            case 1:
                prefabToSpawn = swordPrefab;
                break;
            case 2:
                prefabToSpawn = fireWandPrefab;
                break;
            case 3:
                prefabToSpawn = iceWandPrefab;
                break;
            case 4:
                prefabToSpawn = forestWandPrefab;
                break;
            default: prefabToSpawn = swordPrefab; 
                break;
        }

        if (prefabToSpawn != null)
        {
            currentWeapon = Instantiate(prefabToSpawn, weaponHand);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
        }
        CurrentWeaponIndex = weaponIndex;
        OnWeaponChanged?.Invoke(CurrentWeaponIndex);
    }
}