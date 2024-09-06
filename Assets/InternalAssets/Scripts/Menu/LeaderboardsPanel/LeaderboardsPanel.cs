using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardsPanel : MonoBehaviour
{
    [SerializeField] private LeaderboardsPanelItem itemSample;

    void Start()
    {
        LeaderboardsRecord[] records = LeaderboardsManager.Instance.GetRecords();
        for (int i = 0; i < records.Length; i++)
        {
            LeaderboardsPanelItem panelItem = Instantiate(itemSample, itemSample.transform.parent);
            panelItem.Set(records[i]);
        }

        Destroy(itemSample.gameObject);
        itemSample = null;
    }
}
