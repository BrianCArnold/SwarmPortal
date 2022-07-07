#if NEVER

CovarianceInAndContravarianceOut<Parent> middle_parent = new();
//Contravariant cast down to Child
IContravariantInterface<Child> contravariance_child = middle_parent;
//Covariant cast up to Grandparent
ICovariantInterface<Grandparent> covariance_grandparent = middle_parent;

//Construct most derived type.
var objectIn = new Child(32);

//Assign *in* to contravariant interface cast down to child.
contravariance_child.Property = objectIn;

//Assign *out* of covariant interface cast up to grandparent.
Grandparent objectOut = covariance_grandparent.Property;

//Same value in object.
Console.WriteLine(objectOut.Value == objectIn.Value);

//Same object being referenced, a.k.a. same pointer.
Console.WriteLine(Object.ReferenceEquals(objectIn, objectOut));

#region Types
    //For any T, it can output any child type.
    //Therefore for any types T and Y, such that T : Y
    //You can implicitly cast <out T> to <out Y>.
    public interface ICovariantInterface<out T> { T Property { get; } }
    //For any T, it can accept any child type.
    //Therefore for any types Y and T, such that Y : T
    //You can implicitly cast <in T> to <in Y>.
    public interface IContravariantInterface<in T> { T Property { set; } }
    public class CovarianceInAndContravarianceOut<T> : IContravariantInterface<T>, ICovariantInterface<T>
    { public T? Property { get; set; } }


    public record Grandparent(int Value);
    public record Parent(int Value) : Grandparent(Value);
    public record Child(int Value) : Parent(Value);
#endregion

#endif