using _Project.Scripts.Features.Items;

namespace _Project.Scripts.Features.UI
{
    public class ItemInventoryUnit : InventoryUnit
    {
        public Item Item { get; private set; }

        public void SetItem(Item item, ItemStorableUnit itemStorableUnit)
        {
            Item = item;
            SetIcon(itemStorableUnit.icon);
        }

        public void DelItem()
        {
            Item = null;
            SetIcon();
        }

        public override void Clear()
        {
            DelItem();
        }
    }
}