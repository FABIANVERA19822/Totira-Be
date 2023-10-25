using System;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class MaskAttribute : Attribute
{
    public MaskAttribute() { }
}