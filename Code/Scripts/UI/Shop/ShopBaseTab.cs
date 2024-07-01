using Godot;

namespace Code.Scripts.UI.Shop;

public abstract partial class ShopBaseTab : TabBar
{
	protected const string PriceTagPathFormat = "RichTextLabel/Control/Panel{0}/PriceTag";

	/// <summary>
	/// get all price tags from the scene by their path that only differs in the Panel number 
	/// </summary>
	/// <param name="panelNumber"></param>
	/// <returns></returns>
	protected Label GetPriceTagByPanel(int panelNumber)
	{
		return GetNode<Label>(string.Format(PriceTagPathFormat, panelNumber)); 
	}

	/// <summary>
	/// Parses the price from the price tag label and returns it as an int. It needs the panelnumber of the item in the tab
	/// </summary>
	/// <param name="panelNumber"></param>
	/// <returns></returns>
	protected int ParsePrice(int panelNumber)
	{
		string stringToBeReplaced = "Price: ";
		string priceText = GetNode<Label>(string.Format(PriceTagPathFormat, panelNumber)).Text.Replace(stringToBeReplaced, "");
		int price = int.TryParse(priceText, out price) ? price : 0;
		return price;
	}
	 /// <summary>
	 /// sets the Scollbar to default by stetting the value to 0
	 /// </summary>
	protected virtual void ResetScrollBar() { }

	/// <summary>
	/// Sets the X value of the positin of the control node, that is used to scroll, to the position of the hscrollbar position
	/// </summary>
	protected virtual void Scroll() { }

}
