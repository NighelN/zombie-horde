[System.Serializable]
public class ItemData
{
    public Item item;
    public int amount;

    public ItemData()
    {
        item = null;
        amount = 0;
    }

    public ItemData(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}