using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class DamageLabel : Label
    {
        public override void _Ready()
        {
            // Simple tween for "floating text" effect
            var tween = CreateTween();
            tween.TweenProperty(this, "position", Position + new Vector2(0, -50), 1.0f);
            tween.Parallel().TweenProperty(this, "modulate:a", 0.0f, 1.0f);
            tween.TweenCallback(Callable.From(QueueFree));
        }
    }
}
