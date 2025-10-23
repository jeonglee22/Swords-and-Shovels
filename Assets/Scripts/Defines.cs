public static class LayerName
{
    public readonly static string Floor = "Floor";
    public readonly static string Door = "Door";
    public readonly static string Enemy = "Enemy";
}

public static class AnimatorParameter
{
    public readonly static string Speed = "Speed";
    public readonly static string Attack = "Attack";
}

public enum MouseAim
{
    DoorWay,
    Pointer,
    Sword,
    Target,

    AimCount,
}