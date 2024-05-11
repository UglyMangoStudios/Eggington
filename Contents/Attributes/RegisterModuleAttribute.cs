namespace Eggington.Contents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class RegisterModuleAttribute : Attribute
    {
        public bool IsGlobal { get; init; } = false;
    }
}
