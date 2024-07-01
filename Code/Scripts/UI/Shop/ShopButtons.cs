using Code.Scripts.Managers;
using Code.Scripts.Managers.Level;
using Godot;

namespace Code.Scripts.UI.Shop;

public partial class ShopButtons : Button
{
	private LevelManager _levelManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_levelManager = GetTree().GetFirstNodeInGroup("LevelManager") as LevelManager;

		ShopManager.Instance.OnWeaponUnlocked += SetWeaponButtonStates;
		ShopManager.Instance.OnWeaponEquipped += SetWeaponButtonStates;
		_levelManager.OnLevelReset += SetStatButtonStates;
		_levelManager.OnLevelReset += SetWeaponButtonStates;

		SetStatButtonStates();
		SetWeaponButtonStates();
	}

	public override void _ExitTree()
	{
		ShopManager.Instance.OnWeaponUnlocked -= SetWeaponButtonStates;
		ShopManager.Instance.OnWeaponEquipped -= SetWeaponButtonStates;
		_levelManager.OnLevelReset -= SetStatButtonStates;
		_levelManager.OnLevelReset -= SetWeaponButtonStates;
	}

	/// <summary>
	/// Sets the text of the button to "Buy" if the button is in the Stats group
	/// </summary>
	public void SetStatButtonStates()
	{
		var greatGreatGrandParent = GetParent().GetParent().GetParent().GetParent();

		if (greatGreatGrandParent.IsInGroup("Stats"))
		{
			Text = "Buy";
		}
	}

	/// <summary>
	/// Sets the text of the button to "Equip" if the weapon is unlocked and not equipped, to "Equipped" if the weapon is equipped else to "Buy"
	/// </summary>
	public void SetWeaponButtonStates()
	{
		var greatGreatGrandParent = GetParent().GetParent().GetParent().GetParent();

		if (greatGreatGrandParent.IsInGroup("Weapons") && ShopManager.Instance.TryGetPanelIndex(this, out int index))
		{
			var weapon = ShopManager.Instance.WeaponsList[index];
			if (weapon.Unlocked)
			{
				Text = weapon.Equipped ? "Equipped" : "Equip";
			}
			else
			{
				Text = "Buy";
			}
			
		}
	}

}
