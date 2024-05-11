namespace Eggington.Contents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class RegisterOptionAttribute(string section) : Attribute
    {
        public string Section { get; } = section;
    }
}
