using System;

public interface IAttackGridItem
{
    public bool Exploded {  get; }
    public GridVector Position { get; }

    public event Action<IAttackGridItem> OnExplodedEvent;

    public void Explode();

    public void SetDefault(GridVector position)
    {
    }
}