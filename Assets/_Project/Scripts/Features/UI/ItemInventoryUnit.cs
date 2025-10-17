using _Project.Scripts.Features.Items;

namespace _Project.Scripts.Features.UI
{
    public class ItemInventoryUnit : InventoryUnit
    {
        public Item Item { get; private set; }

        public override void SetSelected(bool isSelected)
        {
            base.SetSelected(isSelected);
            Item?.SetIsSelected(isSelected);
        }

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