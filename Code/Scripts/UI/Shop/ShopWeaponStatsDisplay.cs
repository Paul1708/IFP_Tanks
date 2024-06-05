using Godot;
using System.Linq;
using Code.Scripts.Managers;

namespace Code.Scripts.UI.Shop;

public partial class ShopWeaponStatsDisplay : HBoxContainer
{

	// Textures for the different weapon stats
	public Texture2D DamageTexture = GD.Load<Texture2D>("res://Sprites/UI/Shop/Weapons/Stats/DMG.png");
    public Texture2D BpsTextrue = GD.Load<Texture2D>("res://Sprites/UI/Shop/Weapons/Stats/BPS.png");
    public Texture2D SpeedTexture = GD.Load<Texture2D>("res://Sprites/UI/Shop/Weapons/Stats/SPEED.png");

	public override void _Ready()
	
	{
		RenderWeaponStatsDisplay();
	}

	/// <summary>
	/// Renders the weapon stats display based on the weapon stats of the weapon in the shop
	/// </summary>
	public void RenderWeaponStatsDisplay() 
	{
		var children = GetChildren();
		if (ShopManager.Instance.TryGetPanelIndex(this, out var panelIndex)) 
		{
			var weaponStats = ShopManager.Instance.WeaponsList[panelIndex].WeaponStats;
			var labels = children.OfType<Label>().ToList();
			var textures = children.OfType<TextureRect>().ToList();
			for (int i = 0; i < labels.Count; i++) 
			{
				switch (labels[i].Name) 
				{
					case "Damage":
						labels[i].Text = weaponStats.Damage.ToString();
						textures[i].Texture = DamageTexture;
						break;
					case "FireRate":
						labels[i].Text = weaponStats.BulletsPerSecond.ToString();
						textures[i].Texture = BpsTextrue;
						break;
					case "BulletSpeed":
						labels[i].Text = weaponStats.BulletSpeed.ToString();
						textures[i].Texture = SpeedTexture;
						break;
				}
			}
		}
	}
}
