using System;
using Xunit;
using Xunit.Sdk;

namespace SIO.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [TraitDiscoverer("SIO.Testing.Discoverers.UnitTraitDiscoverer", "SIO.Testing")]
    public class UnitAttribute : FactAttribute, ITraitAttribute { }
}
