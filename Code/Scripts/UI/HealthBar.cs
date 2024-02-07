using Components;
using Godot;
using Managers;


namespace UI;

public partial class HealthBar : TextureProgressBar
{
    [Export]
    public float RedBarDelay { get; set; } = 1f;
    public TextureProgressBar greenBar;
    public TextureProgressBar redBar;
    public Timer redHealthTimer;
    public Label label;

    HealthComponent playerHealthComponent;

    public int currentHP;
    public int maxHP;
    public override void _Ready()
    {
        // Get references
        greenBar = GetNode<TextureProgressBar>("GreenHealth");
        redBar = this;
        redHealthTimer = GetNode<Timer>("RedHealthTimer");
        label = GetNode<Label>("Label");
        playerHealthComponent = PlayerManager.Instance.PlayerHealthComponent;

        // Connect signals
        playerHealthComponent.OnHealthChanged += OnHealthChanged;
        playerHealthComponent.OnMaxHealthChanged += OnMaxHealthChanged;
        redHealthTimer.Timeout += UpdateRedHealthBar;

        // Get hp values of player
        maxHP = playerHealthComponent.maxHP;
        currentHP = playerHealthComponent.currentHP;

        // Set progress bar values
        redBar.MaxValue = maxHP;
        redBar.Value = currentHP;
        greenBar.MaxValue = maxHP;
        greenBar.Value = currentHP;

        UpdateLabelText();
    }

    public override void _ExitTree()
    {
        // Disconnect signals
        playerHealthComponent.OnHealthChanged -= OnHealthChanged;
        playerHealthComponent.OnMaxHealthChanged -= OnMaxHealthChanged;
        redHealthTimer.Timeout -= UpdateRedHealthBar;
    }

    public void OnHealthChanged(int newHP)
    {
        //set the health bar's value to the current health
        greenBar.Value = newHP;

        if (newHP > currentHP)
        {
            redBar.Value = newHP;
        }
        currentHP = newHP;


        UpdateLabelText();


        redHealthTimer.Stop();
        redHealthTimer.Start(RedBarDelay);
    }

    public void OnMaxHealthChanged(int newMaxHP)
    {
        UpdateLabelText();
        maxHP = newMaxHP;
        redBar.MaxValue = maxHP;
        greenBar.MaxValue = maxHP;
    }

    private void UpdateRedHealthBar()
    {
        //set the red health bar's value to the current health
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "value", currentHP, 1).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
    }

    private void UpdateLabelText()
    {
        label.Text = $"{currentHP} / {maxHP}";
    }
}