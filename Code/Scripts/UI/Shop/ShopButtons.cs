using Godot;
using System;

public partial class ShopButtons : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ShopManager.Instance.OnWeaponUnlocked += SetButtonStates;
		ShopManager.Instance.OnWeaponEquipped += SetButtonStates;
		SetButtonStates();
	}

	public override void _ExitTree()
	{
		ShopManager.Instance.OnWeaponUnlocked -= SetButtonStates;
		ShopManager.Instance.OnWeaponEquipped -= SetButtonStates;
	}

	public void SetButtonStates()
	{
		var grandParent = this.GetParent().GetParent();

		if (grandParent.IsInGroup("Stats"))
		{
			this.Text = "Buy";
		}
		else if (grandParent.IsInGroup("Weapons"))
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
