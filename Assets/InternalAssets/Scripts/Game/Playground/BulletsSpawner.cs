using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsSpawner : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    void Awake()
    {
        bulletPrefab.Init();
    }

    public Bullet Spawn(Vector3 position, Quaternion rotation)
    {
        Bullet bullet = bulletPrefab.Pull();
        bullet.transform.SetPositionAndRotation(position, rotation);

        return bullet;
    }
}
