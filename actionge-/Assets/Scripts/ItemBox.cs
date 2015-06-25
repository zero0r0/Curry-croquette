using System.Collections.Generic;
using System.Linq;

public class ItemBox : SingletonMonoBehaviour<ItemBox> {

	public Item heal;
	public FoodStuff potato;

	new void Awake() {
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		
	}

	private List<Item> colledtedItem = new List<Item>();
	
	public Item this[int i] {
		get { return colledtedItem[i]; }
	}

	public void AddItem(Item item) {
		colledtedItem.Add(item);
	}

	/// <summary>
	/// 取得したアイテムをすべて取得する
	/// </summary>
	/// <returns>取得したすべてのアイテム</returns>
	public List<Item> GetAllItems() {
		return colledtedItem;
	}

	/// <summary>
	/// 指定された方の集めたアイテムをすべて取得する
	/// </summary>
	/// <typeparam name="T">アイテムの種類</typeparam>
	/// <returns>指定された種類のアイテムのリスト</returns>
	public List<T> GetAllCollectedList<T>() where T : Item {
		return (from x in colledtedItem where x is T select x).ToList() as List<T>;	
	}
}
