using Godot;

public partial class CameraController : Camera2D
{
    [Export] public float ZoomSpeed = 0.1f; // Speed of zooming
    [Export] public float MinZoom = 0.5f;  // Minimum zoom level
    [Export] public float MaxZoom = 3.0f;  // Maximum zoom level

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                Vector2 zoomChange = Vector2.One * ZoomSpeed;
                if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
                {
                    Zoom = (Zoom - zoomChange).Clamp(Vector2.One * MinZoom, Vector2.One * MaxZoom);
                }
                else if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
                {
                    Zoom = (Zoom + zoomChange).Clamp(Vector2.One * MinZoom, Vector2.One * MaxZoom);
                }
            }
        }
    }
}
