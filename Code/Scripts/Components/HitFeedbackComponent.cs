using Code.Scripts.Managers;
using Godot;

namespace Code.Scripts.Components;

/// <summary>
/// Component that handles the flash and size change of the tank when it takes damage.
/// </summary>
public partial class HitFeedbackComponent : Node2D
{
    [Export]
    public bool DoFlashSprite = true;
    [Export]
    public float MaxFlashStrength = 1f;
    [Export]
    public bool DoScaleSprite = true;
    [Export]
    public float MaxScale = 1.2f;

    [Export]
    public float FeedbackDuration = 0.8f;
    private ShaderMaterial _tankMaterial;
    private ShaderMaterial _gunMaterial;
    private HealthComponent _healthComponent;
    private CharacterBody2D _characterBody;

    public override void _Ready()
    {
        _healthComponent = GetNodeOrNull<HealthComponent>("../HealthComponent");

        // Dirty hack: If the health component is not found,
        // we assume it's the player's health component and get it from the player manager
        if (_healthComponent == null) _healthComponent = PlayerManager.Instance.PlayerHealthComponent;


        _tankMaterial = GetNode<Sprite2D>("../TankBaseSprite").Material as ShaderMaterial;
        _gunMaterial = GetNode<AnimatedSprite2D>("../Gun/GunSprite").Material as ShaderMaterial;
        _characterBody = GetParent<CharacterBody2D>();


        _healthComponent.OnTakeDamage += StartTween;
    }

    public override void _ExitTree()
    {
        _healthComponent.OnTakeDamage -= StartTween;
    }

    /// <summary>
    /// Starts the tween for the hit feedback. It controls the flash and scale of the sprite.
    /// </summary>
    /// <param name="damage">The damage value</param>
    private void StartTween(float damage)
    {
        if (DoFlashSprite)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenMethod(Callable.From<float>(SetShaderParams), MaxFlashStrength, 0.0f, FeedbackDuration)
             .SetTrans(Tween.TransitionType.Quart)
             .SetEase(Tween.EaseType.Out);
        }


        if (DoScaleSprite)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenMethod(Callable.From<float>(SetScaleParams), MaxScale, 1f, FeedbackDuration)
             .SetTrans(Tween.TransitionType.Quart)
             .SetEase(Tween.EaseType.Out);
        }
    }
    
    /// <summary>
    /// Sets the shader parameters for the hit feedback to the input value.
    /// </summary>
    /// <param name="value">The shader value</param>
    private void SetShaderParams(float value)
    {
        _gunMaterial.SetShaderParameter("Weight", value);
        _tankMaterial.SetShaderParameter("Weight", value);
    }

    /// <summary>
    /// Sets the scale of the sprite to the input value.
    /// </summary>
    /// <param name="value">The scale value</param>
    private void SetScaleParams(float value)
    {
        _characterBody.Scale = new Vector2(value, value);
    }
}