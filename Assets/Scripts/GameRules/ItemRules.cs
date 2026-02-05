using System.Collections.Generic;

public static class ItemRules
{
    public const int DefaultMax = 9999;

    private static readonly Dictionary<ItemEnum, int> MaxStack = new()
    {
        { ItemEnum.ForgeStone_Normal, 999 },
        { ItemEnum.ForgeStone_Greater, 999 },
    };

    public static int GetMax(ItemEnum type)
    {
        return MaxStack.TryGetValue(type, out var max) ? max : DefaultMax;
    }
}
