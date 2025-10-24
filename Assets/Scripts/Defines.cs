using UnityEngine;

public enum EnemyType
{
    Melee,
    Ranged,
}
public static class LayerName
{
    public readonly static string Floor = "Floor";
    public readonly static string Door = "Door";
    public readonly static string Enemy = "Enemy";
    public readonly static string Interactive = "Interactive";
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
    Interactive,
    AimCount,
}

public static class TagString
{
    public readonly static string Enemy = "Enemy";
}
