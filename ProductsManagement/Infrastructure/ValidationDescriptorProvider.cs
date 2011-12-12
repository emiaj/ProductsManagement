using System;
using System.Collections.Concurrent;
using FubuMVC.Core.Registration.Nodes;

namespace ProductsManagement.Infrastructure
{
    public class ValidationDescriptorProvider : IValidationDescriptorProvider
    {
        private readonly ConcurrentDictionary<ActionCall, Type> _dictionary;

        public ValidationDescriptorProvider()
        {
            _dictionary = new ConcurrentDictionary<ActionCall, Type>();
        }

        public void Register(ActionCall target, Type descriptor)
        {
            _dictionary[target] = descriptor;
        }

        public bool HasDescriptor(ActionCall target)
        {
            return _dictionary.ContainsKey(target);
        }

        public Type GetDescriptor(ActionCall target)
        {
            return HasDescriptor(target) ? _dictionary[target] : null;
        }
    }
    public interface IValidationDescriptorProvider
    {
        void Register(ActionCall target, Type descriptor);
        bool HasDescriptor(ActionCall target);
        Type GetDescriptor(ActionCall target);
    }
}