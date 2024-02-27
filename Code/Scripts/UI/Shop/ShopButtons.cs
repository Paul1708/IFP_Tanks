using Godot;
using System;

public partial class ShopButtons : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ShopManager.Instance.OnWeaponUnlocked += SetWeaponButtonStates;
		ShopManager.Instance.OnWeaponEquipped += SetWeaponButtonStates;
		SetStatButtonStates();
		SetWeaponButtonStates();
	}

	public override void _ExitTree()
	{
		ShopManager.Instance.OnWeaponUnlocked -= SetWeaponButtonStates;
		ShopManager.Instance.OnWeaponEquipped -= SetWeaponButtonStates;
	}

	public void SetStatButtonStates() 
	{
		var greatGreatGrandParent = this.GetParent().GetParent().GetParent().GetParent();

		if (greatGreatGrandParent.IsInGroup("Stats"))
		{
			this.Text = "Buy";
		}
	}

	public void SetWeaponButtonStates()
	{
		var greatGreatGrandParent = this.GetParent().GetParent().GetParent().GetParent();

		if (greatGreatGrandParent.IsInGroup("Weapons"))
		{
			if (ShopManager.Instance.panelIndexMap.TryGetValue(GetParent().Name, out int index))
			{
				var weapon = ShopManager.Instance.weaponsList[index];
				if (weapon.unlocked)
				{
					this.Text = weapon.equipped ? "Equipped" : "Equip";
				}
				else
				{
					this.Text = "Buy";
				}
			}
		}
	}
}
