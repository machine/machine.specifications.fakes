using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Machine.Fakes.Internal
{
    internal class MappingRegistry
    {
        readonly IDictionary<Type, ICollection<IMapping>> _mappings;

        internal MappingRegistry()
        {
            _mappings = new Dictionary<Type, ICollection<IMapping>>();
        }

        internal void Register(IMapping mapping)
        {
            if (!_mappings.ContainsKey(mapping.InterfaceType))
                _mappings[mapping.InterfaceType] = new Collection<IMapping>();

            _mappings[mapping.InterfaceType].Add(mapping);
        }

        internal bool IsRegistered(Type type)
        {
            return _mappings.ContainsKey(type);
        }

        internal IEnumerable<IMapping> GetMappingsFor(Type type)
        {
            return !IsRegistered(type) ? Enumerable.Empty<IMapping>() : _mappings[type];
        }
    }
}