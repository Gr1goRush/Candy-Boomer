using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootPlatformsController : MonoBehaviour
{
    [SerializeField] private float platformsSpacing = 0.2f;

    [SerializeField] private ShootPlatform shootPlatformSample;

    private int activatedPlatformsCount = 0;
    private Camera mainCamera;

    private ShootPlatform[] shootPlatforms;

    private const int spawnCount = 2;

    void Start()
    {
        mainCamera = Camera.main;

        SpawnPlatforms();

        int activateCount;
        if (PreferencesItemsManager.Instance.CanUseNextRoundItem(PreferenceItem.ExtraShootPlatformBoost))
        {
            activateCount = 2;
        }
        else
        {
            activateCount = 1;
        }

        ActivatePlatforms(activateCount);

        InputController.Instance.OnTouchFinished += OnTouchFinished;
    }

    private void OnTouchFinished(Vector2 position)
    {
        if (!gameObject.activeInHierarchy || GameController.Instance.Paused)
        {
            return;
        }

        if(GraphicRaycastController.Instance.Raycast(position))
        {
            return;
        }

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(position);

        int nearestPlatformIndex = -1;
        float minDistance = 0f;
        for (int i = 0; i < activatedPlatformsCount; i++)
        {
            ShootPlatform platform = shootPlatforms[i];
            if (platform.Shooting)
            {
                continue;
            }

            float d = Vector2.Distance(worldPosition, platform.transform.position);

            if(nearestPlatformIndex < 0 || d < minDistance)
            {
                nearestPlatformIndex = i;
                minDistance = d;
            }
        }

        if(nearestPlatformIndex >= 0)
        {
            shootPlatforms[nearestPlatformIndex].RotateAndShoot(worldPosition);
        }
    }

    private void SpawnPlatforms()
    {
        shootPlatforms = new ShootPlatform[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            ShootPlatform shootPlatform = i == 0 ? shootPlatformSample : Instantiate(shootPlatformSample, shootPlatformSample.transform.parent);
            shootPlatform.gameObject.SetActive(false);

            shootPlatforms[i] = shootPlatform;
        }
    }

    private void ActivatePlatforms(int count)
    {
        Vector3 platformSize = shootPlatformSample.GetSize();

        float fullWidth = (platformSize.x * count) + ((count - 1) * platformsSpacing);
        float _x = -fullWidth / 2f + (platformSize.x / 2f);

        for (int i = 0; i < spawnCount; i++)
        {
            ShootPlatform shootPlatform = shootPlatforms[i];

            if (i >= count)
            {
                shootPlatform.gameObject.SetActive(false);
            }
            else
            {
                shootPlatform.gameObject.SetActive(true);

                shootPlatform.transform.position = new Vector3(_x, 0f, 0f);

                _x += platformSize.x + platformsSpacing;
            }
        }

        activatedPlatformsCount = count;
    }

    private void OnDestroy()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.OnTouchFinished -= OnTouchFinished;
        }
    }
}
