using System;

using SFML.System;


namespace Canvaz.Engine.Types;


public static class TypeConversions
{
    public static Vector2f ToVector2f<T>(this Vec2<T> vec) where T : IConvertible
        => new(vec.X.ToSingle(null), vec.Y.ToSingle(null));

    public static Vector2i ToVector2i<T>(this Vec2<T> vec) where T : IConvertible
        => new(vec.X.ToInt32(null), vec.Y.ToInt32(null));

    public static Vector2u ToVector2u<T>(this Vec2<T> vec) where T : IConvertible
        => new(vec.X.ToUInt32(null), vec.Y.ToUInt32(null));


    
    public static Vec2<float> ToVec2f(this Vector2f vector)
        => new(vector.X, vector.Y);

    public static Vec2<int> ToVec2i(this Vector2i vector)
        => new(vector.X, vector.Y);
    
    public static Vec2<uint> ToVec2u(this Vector2u vector)
        => new(vector.X, vector.Y);
}