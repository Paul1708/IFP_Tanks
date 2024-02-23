using Godot;
using System.Collections.Generic;


public struct Stat
{
	public string name;
	public int price;
	public Label priceTag;
	public int quantity;
	public Stat(string name)
	{
		this.name = name;
	}
	public Stat(string name, int price, Label priceTag, int quantity)
	{
		this.name = name;
		this.price = price;
		this.priceTag = priceTag;
		this.quantity = quantity;
	}
}

public struct Weapon
{
	public string name;
	public int price;
	public Label priceTag;
	public bool unlocked = false;
	public bool equiped = false;
	public Weapon(string name)
	{
		this.name = name;
	}
	public Weapon(string name, int price, Label priceTag, bool unlocked, bool equiped )
	{
		this.name = name;
		this.price = price;
		this.priceTag = priceTag;
		this.unlocked = unlocked;
		this.equiped = equiped;
	}
}

public partial class ShopBaseTab : TabBar
{
	protected const string PriceTagPathFormat = "RichTextLabel/Control/Panel{0}/PriceTag";
	[Signal] public delegate void OnItemBoughtUpdatePricesEventHandler();


	//get all price tags from the scene by their path that only differs in the Panel number 
	protected Label GetPriceTagByPanel(int panelNumber)
	{
		return GetNode<Label>(string.Format(PriceTagPathFormat, panelNumber)); 
	}

	///<summary>
	///Parses the price from the price tag label and returns it as an int. It needs the panelnumber of the item in the tab
	///</summary>
	protected int ParsePrice(int panelNumber)
	{
		string stringToBeReplaced = "Price: ";
		string priceText = GetNode<Label>(string.Format(PriceTagPathFormat, panelNumber)).Text.Replace(stringToBeReplaced, "");
		int price = int.TryParse(priceText, out price) ? price : 0;
		return price;
	}
}
