using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemBox : SingletonMonoBehaviour<ItemBox> {

	new void Awake() {
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		//SetList();
	}

	void SetList() {
		colledtedItem.Add(meet);
		colledtedItem.Add(potato);
		colledtedItem.Add(item);
	}

	private List<Item> colledtedItem = new List<Item>();
	public List<Item> CollectedItem { get { return colledtedItem; } }

	[SerializeField]
	public FoodStuff meet;
	[SerializeField]
	public FoodStuff potato;
	[SerializeField]
	public FoodStuff item;
	
	public Item this[int i] {
		get { return colledtedItem[i]; }
	}

	public void AddItem(Item item) {
		colledtedItem.Add(item);
	}

	/// <summary>
	/// 指定された方の集めたアイテムをすべて取得する
	/// </summary>
	/// <typeparam name="T">アイテムの種類</typeparam>
	/// <returns>指定された種類のアイテムのリスト</returns>
	public List<T> GetAllCollectedList<T>() where T : Item {
		return (from x in colledtedItem where x is T select x).Cast<T>().ToList<T>();
	}
}
