
public interface IItemContainer
{
    int ItemCount(string itemID);
    Item RemoveItem(string itemID);
    bool RemoveItem(Item item);
    bool CanAddItem(Item item, int amount = 1);
    bool AddItem(Item item);
    void Clear();
}
