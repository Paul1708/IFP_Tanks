using Components;
using Godot;
using Managers;

namespace Enemies;

public partial class HitFeedbackComponent : Node2D
{
    [Export]
    public bool doFlashSprite = true;
    [Export]
    public float maxFlashStrength = 1f;
    [Export]
    public bool doScaleSprite = true;
    [Export]
    public float maxScale = 1.2f;

    [Export]
    public float feedbackDuration = 0.8f;



    ShaderMaterial tankMaterial;
    ShaderMaterial gunMaterial;
    HealthComponent healthComponent;
    CharacterBody2D characterBody;

    public override void _Ready()
    {
        healthComponent = GetNodeOrNull<HealthComponent>("../HealthComponent");

        // Dirty hack: If the health component is not found,
        // we assume it's the player's health component and get it from the player manager
        if (healthComponent == null) healthComponent = PlayerManager.Instance.PlayerHealthComponent;


        tankMaterial = GetNode<Sprite2D>("../TankBaseSprite").Material as ShaderMaterial;
        gunMaterial = GetNode<AnimatedSprite2D>("../Gun/GunSprite").Material as ShaderMaterial;
        characterBody = GetParent<CharacterBody2D>();


        healthComponent.OnTakeDamage += StartTween;
    }

    public override void _ExitTree()
    {
        healthComponent.OnTakeDamage -= StartTween;
    }

    private void StartTween(int damage)
    {
        if (doFlashSprite)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenMethod(Callable.From<float>(SetShaderParams), maxFlashStrength, 0.0f, feedbackDuration)
             .SetTrans(Tween.TransitionType.Quart)
             .SetEase(Tween.EaseType.Out);
        }


        if (doScaleSprite)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenMethod(Callable.From<float>(SetScaleParams), maxScale, 1f, feedbackDuration)
             .SetTrans(Tween.TransitionType.Quart)
             .SetEase(Tween.EaseType.Out);
        }
    }
    private void SetShaderParams(float value)
    {
        gunMaterial.SetShaderParameter("Weight", value);
        tankMaterial.SetShaderParameter("Weight", value);
    }

    private void SetScaleParams(float value)
    {
        characterBody.Scale = new Vector2(value, value);
    }
}