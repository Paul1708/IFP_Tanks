using Godot;
using Managers;
using System.Linq;

public partial class ShopWeaponStatsDisplay : HBoxContainer
{

	// Textures for the different weapon stats
	public Texture2D DamageTexture = GD.Load<Texture2D>("res://Sprites/UI/Shop/Weapons/Stats/DMG.png");
    public Texture2D BPSTextrue = GD.Load<Texture2D>("res://Sprites/UI/Shop/Weapons/Stats/BPS.png");
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
			var weaponStats = ShopManager.Instance.weaponsList[panelIndex].weaponStats;
			var labels = children.OfType<Label>().ToList();
			var textures = children.OfType<TextureRect>().ToList();
			for (int i = 0; i < labels.Count; i++) 
			{
				switch (labels[i].Name) 
				{
					case "Damage":
						labels[i].Text = weaponStats.damage.ToString();
						textures[i].Texture = DamageTexture;
						break;
					case "FireRate":
						labels[i].Text = weaponStats.bulletsPerSecond.ToString();
						textures[i].Texture = BPSTextrue;
						break;
					case "BulletSpeed":
						labels[i].Text = weaponStats.bulletSpeed.ToString();
						textures[i].Texture = SpeedTexture;
						break;
				}
			}
		}
	}
}
