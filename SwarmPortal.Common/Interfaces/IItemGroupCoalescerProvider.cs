namespace SwarmPortal.Common;

public interface IItemDictionaryGeneratorProvider<TGroupableItem>
    where TGroupableItem : class, IGroupableItem, INamedItem, IHasRoles
{
    DictionaryGenerator<TGroupableItem> GetDictionaryGeneratorAsync(CancellationToken ct);
}
