using System;
using FubuMVC.Validation;

namespace ProductsManagement.Infrastructure
{
    public class ValidationDescriptor : IFubuContinuationModelDescriptor
    {
        private readonly IValidationDescriptorProvider _provider;

        public ValidationDescriptor(IValidationDescriptorProvider provider)
        {
            _provider = provider;
        }

        public Type DescribeModelFor(ValidationFailure context)
        {
            return _provider.GetDescriptor(context.Target);
        }
    }
}