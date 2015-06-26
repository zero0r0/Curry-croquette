using UnityEngine;

[System.Serializable]
public class Item {
	public enum ItemId { Meet, Onion, Curotte, Potato, }
	
	[SerializeField]
	private ItemId itemId;
	public ItemId item { get { return itemId; } }

	public Sprite itemSprite;
}

[System.Serializable]
public class FoodStuff : Item {
	public enum FoodStuffId { Meet, Onion, Curotte, Potato, }
	private FoodStuffId foodStuffId;

	public FoodStuff(FoodStuffId foodStuffId, Sprite foodStuffImage) {
		itemSprite = foodStuffImage;
		this.foodStuffId = foodStuffId;
	}
}
