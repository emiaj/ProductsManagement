using System.Collections.Generic;
using System.Reflection;
using FubuValidation.Fields;
using ProductsManagement.Handlers.Products;

namespace ProductsManagement.Conventions
{
    public class AddProductModelValidationSource : IFieldValidationSource
    {
        public IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            if(property.DeclaringType == typeof(AddProductModel) && property.Name.Equals("Name"))
            {
                yield return new MaximumLengthRule(20);
            }
        }

        public void Validate()
        {
        }
    }
}