using System;


namespace Canvaz.Language.Definitions.Typing;


public partial class Type
{
    public static Type operator+(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<string, string>(left, right, ref result, (l, r) => l + r);
        BinaryOperation<float, float>(left, right, ref result, (l, r) => l + r);
        BinaryOperation<int, int>(left, right, ref result, (l, r) => l + r);
        BinaryOperation<uint, uint>(left, right, ref result, (l, r) => l + r);

        if (result is null)
            throw NewError($"'+' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");

        return result;
    }


    public static Type operator-(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, float>(left, right, ref result, (l, r) => l - r);
        BinaryOperation<int, int>(left, right, ref result, (l, r) => l - r);
        BinaryOperation<uint, uint>(left, right, ref result, (l, r) => l - r);

        if (result is null)
            throw NewError($"'-' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator-(Type right)
    {
        Type? result = null;

        UnaryOperation<float, float>(right, ref result, r => -r);
        UnaryOperation<int, int>(right, ref result, r => -r);

        if (result is null)
            throw NewError($"Unary '-' is not applicable for '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator!(Type right)
        => new(!right.IsTruthy());


    public static Type operator*(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, float>(left, right, ref result, (l, r) => l * r);
        BinaryOperation<int, int>(left, right, ref result, (l, r) => l * r);
        BinaryOperation<uint, uint>(left, right, ref result, (l, r) => l * r);

        if (result is null)
            throw NewError($"'*' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator/(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, float>(left, right, ref result, (l, r) => l / r);
        BinaryOperation<int, int>(left, right, ref result, (l, r) => l / r);
        BinaryOperation<uint, uint>(left, right, ref result, (l, r) => l / r);

        if (result is null)
            throw NewError($"'/' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator==(Type left, Type right)
    {
        if (left.IsNull() && right.IsNull())
            return new(true);

        if (left.IsNull())
            return new(false);

        return new(left.Value!.Equals(right.Value));
    }


    public static Type operator!=(Type left, Type right)
        => !(left == right);


    public static bool operator true(Type type)
        => type.IsTruthy();

    public static bool operator false(Type type)
        => !type.IsTruthy();


    public static Type operator&(Type left, Type right)
        => new(left.IsTruthy() && right.IsTruthy());

    public static Type operator|(Type left, Type right)
        => new(left.IsTruthy() || right.IsTruthy());


    public static Type operator>(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, bool>(left, right, ref result, (l, r) => l > r);
        BinaryOperation<int, bool>(left, right, ref result, (l, r) => l > r);
        BinaryOperation<uint, bool>(left, right, ref result, (l, r) => l > r);

        if (result is null)
            throw NewError($"'>' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator<(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, bool>(left, right, ref result, (l, r) => l < r);
        BinaryOperation<int, bool>(left, right, ref result, (l, r) => l < r);
        BinaryOperation<uint, bool>(left, right, ref result, (l, r) => l < r);

        if (result is null)
            throw NewError($"'<' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator>=(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, bool>(left, right, ref result, (l, r) => l >= r);
        BinaryOperation<int, bool>(left, right, ref result, (l, r) => l >= r);
        BinaryOperation<uint, bool>(left, right, ref result, (l, r) => l >= r);

        if (result is null)
            throw NewError($"'>=' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public static Type operator<=(Type left, Type right)
    {
        Type? result = null;

        BinaryOperation<float, bool>(left, right, ref result, (l, r) => l <= r);
        BinaryOperation<int, bool>(left, right, ref result, (l, r) => l <= r);
        BinaryOperation<uint, bool>(left, right, ref result, (l, r) => l <= r);

        if (result is null)
            throw NewError($"'<=' is not applicable for '{left.TypeName}' and '{right.TypeName}'.");
    
        return result;
    }


    public bool IsTruthy()
    {
        if (IsNull())
            return false;

        if (TypeName.Is<bool>())
            return AsBoolean();

        throw NewError($"Can't determine truthiness of '{TypeName}'.");
    }

    public bool Equals(Type other)
        => (this == other).AsBoolean();

    public override bool Equals(object? obj)
        => obj is not null && Equals((obj as Type)!);


    public override int GetHashCode()
        => base.GetHashCode() * Value?.ToString()?.Length ?? 1;


    private static void BinaryOperation<T, TResult>(Type left, Type right, ref Type? result, Func<T, T, TResult> operation)
    {
        if (BothOfType(left.TypeName, right.TypeName, TypeName.FromGeneric<T>()))
            result = new(operation(left.As<T>(), right.As<T>()));
    }

    private static void UnaryOperation<T, TResult>(Type right, ref Type? result, Func<T, TResult> operation)
    {
        if (right.TypeName == TypeName.FromGeneric<T>())
            result = new(operation(right.As<T>()));
    }
}