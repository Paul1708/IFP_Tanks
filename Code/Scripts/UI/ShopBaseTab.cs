using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class ShopBaseTab : TabBar
{
	protected const string PriceTagPathFormat = "RichTextLabel/Control/Panel{0}/PriceTag";
	public int[] prices = new int[4];
	public int[] itemQuantities = new int[4];
	public Label priceTag1, priceTag2, priceTag3, priceTag4;
	public List<Node> tabItems { get; set; }
	public int itemCount;

    protected Label GetPriceTag(int panelNumber)
    {
        return GetNode<Label>(string.Format(PriceTagPathFormat, panelNumber));
    }
}

