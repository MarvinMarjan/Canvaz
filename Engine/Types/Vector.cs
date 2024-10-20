using SFML.System;

using Canvaz.Engine.Animation;


namespace Canvaz.Engine.Types;


public struct Vec2f(float x, float y) : IAnimateable<Vec2f>
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;


    public static implicit operator Vector2f(Vec2f vec2) => new(vec2.X, vec2.Y);
    public static implicit operator Vec2f(Vector2f vec2) => new(vec2.X, vec2.Y);


    // TODO: maybe this can be simplified by using interfaces? like IAdditionOperators<>?

    public static Vec2f operator+(Vec2f left, Vec2f right)
        => new(left.X + right.X, left.Y + right.Y);

    public static Vec2f operator-(Vec2f left, Vec2f right)
        => new(left.X - right.X, left.Y - right.Y);

    public static Vec2f operator*(Vec2f left, Vec2f right)
        => new(left.X * right.X, left.Y * right.Y);

    public static Vec2f operator/(Vec2f left, Vec2f right)
        => new(left.X / right.X, left.Y / right.Y);

        
    public static bool operator==(Vec2f left, Vec2f right)
        => left.Equals(right);

    public static bool operator!=(Vec2f left, Vec2f right)
        => !left.Equals(right);


    public readonly bool Equals(Vec2f other)
        => X == other.X && Y == other.Y;

    public override readonly bool Equals(object? obj)
        => obj is not null && Equals((Vec2f)obj);


    public override readonly int GetHashCode()
        => ((int)X + (int)Y) * (int)X * (int)Y;


    public readonly AnimationState AnimateThis(Vec2f to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2f(this, to, time, easingType);

    public readonly Vec2f ConvertAnimationValues(float[] values)
        => values.ToVec2f();


    public override readonly string ToString()
        => $"Vec2f({X}, {Y})";
}


public struct Vec2i(int x, int y) : IAnimateable<Vec2i>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;


    public static implicit operator Vector2i(Vec2i vec2) => new(vec2.X, vec2.Y);
    public static implicit operator Vec2i(Vector2i vec2) => new(vec2.X, vec2.Y);


    public static Vec2i operator+(Vec2i left, Vec2i right)
        => new(left.X + right.X, left.Y + right.Y);

    public static Vec2i operator-(Vec2i left, Vec2i right)
        => new(left.X - right.X, left.Y - right.Y);

    public static Vec2i operator*(Vec2i left, Vec2i right)
        => new(left.X * right.X, left.Y * right.Y);

    public static Vec2i operator/(Vec2i left, Vec2i right)
        => new(left.X / right.X, left.Y / right.Y);


    public static bool operator==(Vec2i left, Vec2i right)
        => left.Equals(right);

    public static bool operator!=(Vec2i left, Vec2i right)
        => !left.Equals(right);


    public readonly bool Equals(Vec2i other)
        => X == other.X && Y == other.Y;

    public override readonly bool Equals(object? obj)
        => obj is not null && Equals((Vec2i)obj);


    public override readonly int GetHashCode()
        => (X + Y) * X * Y;


    public readonly AnimationState AnimateThis(Vec2i to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2i(this, to, time, easingType);

    public readonly Vec2i ConvertAnimationValues(float[] values)
        => values.ToVec2i();

    public override readonly string ToString()
        => $"Vec2i({X}, {Y})";
}


public struct Vec2u(uint x, uint y) : IAnimateable<Vec2u>
{
    public uint X { get; set; } = x;
    public uint Y { get; set; } = y;


    public static implicit operator Vector2u(Vec2u vec2) => new(vec2.X, vec2.Y);
    public static implicit operator Vec2u(Vector2u vec2) => new(vec2.X, vec2.Y);


    public static Vec2u operator+(Vec2u left, Vec2u right)
        => new(left.X + right.X, left.Y + right.Y);

    public static Vec2u operator-(Vec2u left, Vec2u right)
        => new(left.X - right.X, left.Y - right.Y);

    public static Vec2u operator*(Vec2u left, Vec2u right)
        => new(left.X * right.X, left.Y * right.Y);

    public static Vec2u operator/(Vec2u left, Vec2u right)
        => new(left.X / right.X, left.Y / right.Y);


    public static bool operator==(Vec2u left, Vec2u right)
        => left.Equals(right);

    public static bool operator!=(Vec2u left, Vec2u right)
        => !left.Equals(right);


    public readonly bool Equals(Vec2u other)
        => X == other.X && Y == other.Y;

    public override readonly bool Equals(object? obj)
        => obj is not null && Equals((Vec2u)obj);


    public override readonly int GetHashCode()
        => ((int)X + (int)Y) * (int)X * (int)Y;


    public readonly AnimationState AnimateThis(Vec2u to, float time, EasingType easingType = EasingType.Linear)
        => Animate.Vec2u(this, to, time, easingType);

    public readonly Vec2u ConvertAnimationValues(float[] values)
        => values.ToVec2u();

    public override readonly string ToString()
        => $"Vec2u({X}, {Y})";
}