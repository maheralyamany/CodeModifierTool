using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
/// <summary>Represents: MyClass</summary>

public class MyClass
{
    /// <summary>Performs do something</summary>

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void DoSomething()
    {
        Console.WriteLine("Hello");
    }
    /// <summary>Initializes a new instance of the MyClass class</summary>

    [MethodImpl(MethodImplOptions.NoInlining)]
    public MyClass() { }
    /// <summary>Gets or sets: Property1</summary>

    [Column("Property1")]
    public int Property1 { get; set; } // auto-property
    /// <summary>Gets: Property3</summary>

    [Column("Property3")]
    public int Property3 { get; }
    /// <summary>Gets: Property4</summary>

    [Column("Property4")]
    public int Property4 => Property1;
    /// <summary>Gets or sets: Property2</summary>

    [Column("Property2")]
    public string Property2
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get
        {
            return "val";
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        set
        {
            Console.WriteLine(value);
        }
    }


    /// <summary>Performs local function demo</summary>
    /// <param name="msg">The msg collection</param>
    /// <returns>The result of the local function demo</returns>

    [MethodImpl(MethodImplOptions.NoInlining)]
    public bool LocalFunctionDemo(string msg)
    {
        static void Inner() { }
        Inner();
        return false;
    }
}
