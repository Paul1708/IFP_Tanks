using Godot;

namespace Code.Scripts.UI.Shop;

public abstract partial class ShopBaseTab : TabBar
{
	protected const string PriceTagPathFormat = "RichTextLabel/Control/Panel{0}/PriceTag";

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

	protected virtual void ResetScrollBar() { }
	protected virtual void Scroll() { }

}
