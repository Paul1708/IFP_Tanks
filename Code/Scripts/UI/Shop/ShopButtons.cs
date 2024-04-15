using Godot;
using Managers;
using Managers.Level;

namespace Shop;

public partial class ShopButtons : Button
{
	LevelManager levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

		ShopManager.Instance.OnWeaponUnlocked += SetWeaponButtonStates;
		ShopManager.Instance.OnWeaponEquipped += SetWeaponButtonStates;
		levelManager.OnLevelReset += SetStatButtonStates;
		levelManager.OnLevelReset += SetWeaponButtonStates;

		SetStatButtonStates();
		SetWeaponButtonStates();
	}

	public override void _ExitTree()
	{
		ShopManager.Instance.OnWeaponUnlocked -= SetWeaponButtonStates;
		ShopManager.Instance.OnWeaponEquipped -= SetWeaponButtonStates;
		levelManager.OnLevelReset -= SetStatButtonStates;
		levelManager.OnLevelReset -= SetWeaponButtonStates;
	}

	public void SetStatButtonStates()
	{
		var greatGreatGrandParent = GetParent().GetParent().GetParent().GetParent();

		if (greatGreatGrandParent.IsInGroup("Stats"))
		{
			Text = "Buy";
		}
	}

	public void SetWeaponButtonStates()
	{
		var greatGreatGrandParent = GetParent().GetParent().GetParent().GetParent();

		if (greatGreatGrandParent.IsInGroup("Weapons") && ShopManager.Instance.TryGetPanelIndex(this, out int index))
		{
			var weapon = ShopManager.Instance.weaponsList[index];
			Text = weapon.unlocked ? (weapon.equipped ? "Equipped" : "Equip") : "Buy";
		}
	}

}
