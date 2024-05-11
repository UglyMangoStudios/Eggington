namespace Eggington.Contents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class RegisterSingletonAttribute() : Attribute
    {
        /// <summary>
        /// If true, the application will force this service to build on startup. Default is false.
        /// </summary>
        public bool Startup { get; init; } = false;
    }
}
