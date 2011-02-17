using System;

namespace Machine.Fakes
{
    /// <summary>
    ///   This attribute is the configuration endpoint to machine.fakes.
    ///   It can be only be used at assembly level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigurationAttribute : Attribute
    {
        /// <summary>
        ///   Gets or sets the type of the fake engine which is used.
        /// </summary>
        public Type FakeEngineType { get; set; } 
    }
}