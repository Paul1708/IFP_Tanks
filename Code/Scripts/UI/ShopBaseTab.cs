using Godot;
using System;


public partial class ShopBaseTab : TabBar
{
	protected const string PriceTagPathFormat = "RichTextLabel/Control/Panel{0}/PriceTag";
	public int price1, price2, price3, price4;
	public Label priceTag1, priceTag2, priceTag3, priceTag4;
	public int itemQuantity1, itemQuantity2, itemQuantity3, itemQuantity4; 

    protected Label GetPriceTag(int panelNumber)
    {
        return GetNode<Label>(string.Format(PriceTagPathFormat, panelNumber));
    }
}

