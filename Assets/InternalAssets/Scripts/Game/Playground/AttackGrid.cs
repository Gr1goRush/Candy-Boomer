using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public  enum AttackGridRowsGroupType
{
    Top, Bottom
}

public struct AttackGridRowsGroupState
{
    public Transform container;
    public bool spawnFinished;
    public List<IAttackGridItem> items;
}

public class AttackGrid : MonoBehaviour
{
    //public int CandyCount { get; private set; }

    [SerializeField] private float spacing = 0.2f, moveSpeed, startOffset = 0.2f, superGameSpawnRate = 0.1f, coinSpawnRate = 0.4f;
    [SerializeField] private float appearIntervalMultiplier = 1.3f;

    [SerializeField] private Bomb bombSample;
    [SerializeField] private SuperGameCandy superGameCandySample;
    [SerializeField] private Candy candySample;
    [SerializeField] private Coin coinSample;

    [SerializeField] private RuntimeAnimatorController[] candyAnimatorControllers, bombAnimatorControllers;

    private float rowHeight;

    private Dictionary<AttackGridRowsGroupType, AttackGridRowsGroupState> groupStates;

    public void StartAttack(LevelRow[] topRows, LevelRow[] bottomRows)
    {
        //CandyCount = 0;

        rowHeight = candySample.GetSize().y;

        candySample.Init();
        bombSample.Init();
        superGameCandySample.Init();

        float halfScreenWorldHeight = Camera.main.orthographicSize;

        Vector3 topContainerPosition = new Vector3(0f, halfScreenWorldHeight + rowHeight - startOffset, 0);
        Vector3 bottomContainerPosition = new Vector3(0f, -halfScreenWorldHeight - rowHeight + startOffset, 0);

        groupStates = new Dictionary<AttackGridRowsGroupType, AttackGridRowsGroupState>
        {
            { AttackGridRowsGroupType.Top, InitGroup(AttackGridRowsGroupType.Top, topContainerPosition, topRows) },
            { AttackGridRowsGroupType.Bottom, InitGroup(AttackGridRowsGroupType.Bottom, bottomContainerPosition, bottomRows) },
        };
    }

    private AttackGridRowsGroupState InitGroup(AttackGridRowsGroupType groupType, Vector3 containerPosition, LevelRow[] _rows)
    {
       Transform _container = new GameObject(groupType.ToString()).transform;
        _container.SetParent(transform);
        _container.position = containerPosition;

        StartCoroutine(RowsAttacking(_rows, _container, groupType));

        return new AttackGridRowsGroupState { container = _container, items = new List<IAttackGridItem>(), spawnFinished = false };
    }

    private IEnumerator RowsAttacking(LevelRow[] rows, Transform container, AttackGridRowsGroupType group)
    {
        float moveDirection = group == AttackGridRowsGroupType.Top ? -1 : 1;

        float spawnY = 0f;
        int rowIndex = 0;
        float appearInterval = 0f;

        while (true)
        {
            if(rowIndex < rows.Length)
            {
                LevelRow _row = rows[rowIndex];
                appearInterval = _row.appearInterval * appearIntervalMultiplier;

                yield return new WaitForSeconds(appearInterval);

                SpawnRow(_row, spawnY, rowIndex, group);
                rowIndex++;

                if(rowIndex >= rows.Length)
                {
                    AttackGridRowsGroupState groupState = groupStates[group];
                    groupState.spawnFinished = true;
                    groupStates[group] = groupState;
                }
            }
            else
            {
                yield return new WaitForSeconds(appearInterval);
            }

            spawnY += (rowHeight + spacing) * -moveDirection;

            float destinationY = container.position.y + ((rowHeight + spacing) * moveDirection);
            Vector3 destination = new Vector3(container.position.x, destinationY, 0f);

            while (Vector3.Distance(container.position, destination) >= (Time.fixedDeltaTime * moveSpeed))
            {
                container.position = Vector3.MoveTowards(container.transform.position, destination, Time.fixedDeltaTime * moveSpeed);

                yield return new WaitForFixedUpdate();
            }

            container.position = destination;
        }
    }

    private List<IAttackGridItem> GetGroupItems(AttackGridRowsGroupType group)
    {
        return groupStates[group].items;
    }

    private void SpawnRow(LevelRow row, float y, int rowIndex, AttackGridRowsGroupType group)
    {
        AttackGridRowsGroupState groupState = groupStates[group];

        Vector3 itemSize = candySample.GetSize();

        int count = row.items.Length;

        float fullWidth = (itemSize.x * count) + ((count - 1) * spacing);
        float _x = -fullWidth / 2f + (itemSize.x / 2f);

        for (int i = 0; i < count; i++)
        {
            LevelRowItem _item = row.items[i];
            IAttackGridItem attackGridItem;
            GameObject attackGridItemObj;
            GridVector gridPos = new GridVector
            {
                group = group,
                x = i,
                y = rowIndex
            };

            if(_item.type == LevelRowItemType.Bomb)
            {
                Bomb bomb = bombSample.Pull();
                bomb.SetAnimatorController(bombAnimatorControllers[Random.Range(0, bombAnimatorControllers.Length)]);

                attackGridItem = bomb;
                attackGridItemObj = bomb.gameObject;
            }
            else
            {
                float _rand = Random.Range(0f, 1f);
                if(_rand <= superGameSpawnRate && rowIndex >= 2)
                {
                    SuperGameCandy superGameCandy = superGameCandySample.Pull();

                    attackGridItem = superGameCandy;
                    attackGridItemObj = superGameCandy.gameObject;
                }
                else if (_rand <= coinSpawnRate)
                {
                    Coin coin = coinSample.Pull();

                    attackGridItem = coin;
                    attackGridItemObj = coin.gameObject;
                }
                else
                {
                    Candy candy = candySample.Pull();
                    candy.SetAnimatorController(candyAnimatorControllers[Random.Range(0, candyAnimatorControllers.Length)]);

                    attackGridItem = candy;
                    attackGridItemObj = candy.gameObject;

                    //CandyCount++;
                }      
            }

            attackGridItem.SetDefault(gridPos);
            attackGridItem.OnExplodedEvent += AttackGridItem_OnExploded;
            groupState.items.Add(attackGridItem);

            attackGridItemObj.SetActive(true);
            attackGridItemObj.transform.SetParent(groupState.container);
            attackGridItemObj.transform.localPosition = new Vector3(_x, y, 0f);

            _x += itemSize.x + spacing;
        }
    }

    private void AttackGridItem_OnExploded(IAttackGridItem _item)
    {
       _item.OnExplodedEvent -= AttackGridItem_OnExploded;
        List<IAttackGridItem> _items = GetGroupItems(_item.Position.group);
        _items.Remove(_item);

        if (_items.Count <= 0 && AllGroupsSpawnFinishedAndHasNotItems())
        {
            GameController.Instance.Win();
        }
    }

    private bool AllGroupsSpawnFinishedAndHasNotItems()
    {
        foreach (var item in groupStates.Values)
        {
            if (!item.spawnFinished || item.items.Count > 0)
            {
                return false;
            }
        }

        return true;
    }

    public List<IAttackGridItem> FindItemsInRow(int rowIndex, AttackGridRowsGroupType group)
    {
        List<IAttackGridItem> result = new List<IAttackGridItem>();
        List<IAttackGridItem> attackGridItems = GetGroupItems(group);
        foreach (IAttackGridItem item in attackGridItems)
        {
            if (item.Position.y == rowIndex)
            {
                result.Add(item);
            }
        }

        return result;
    }

    public List<IAttackGridItem> FindItemsInColumn(int columnIndex, AttackGridRowsGroupType group)
    {
        List<IAttackGridItem> result = new List<IAttackGridItem>();
        List<IAttackGridItem> attackGridItems = GetGroupItems(group);
        foreach (IAttackGridItem item in attackGridItems)
        {
            if (item.Position.x == columnIndex)
            {
                result.Add(item);
            }
        }

        return result;
    }

    public IAttackGridItem FindItem(int rowIndex, int columnIndex, AttackGridRowsGroupType group)
    {
        if(rowIndex < 0 || columnIndex < 0)
        {
            return null;
        }

        List<IAttackGridItem> attackGridItems = GetGroupItems(group);
        foreach (IAttackGridItem item in attackGridItems)
        {
            if (item.Position.x == columnIndex && item.Position.y == rowIndex && item.Position.group == group)
            {
               return item;
            }
        }

        return null;
    }

    public void ExplodeRandomRate(float rate)
    {
        List<IAttackGridItem> allItems = new List<IAttackGridItem>();
        foreach (var item in groupStates.Values)
        {
            allItems.AddRange(item.items);
        }

        int count = Mathf.RoundToInt(allItems.Count * rate);
        int explodedCount = 0;
        foreach (var item in allItems)
        {
            if(item != null && !item.Exploded)
            {
                item.Explode();
                explodedCount++;

                if(explodedCount >= count)
                {
                    return;
                }
            }
        }
    }
}
