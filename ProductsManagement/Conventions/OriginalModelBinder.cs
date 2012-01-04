using System.Reflection;
using FubuCore.Binding;
using FubuMVC.Core.Runtime;

namespace ProductsManagement.Conventions
{
    public class OriginalModelBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.IsAssignableFrom(property.DeclaringType) && property.Name.Equals("OriginalModel");
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var fubuRequest = context.Service<IFubuRequest>();
            var modelType = property.PropertyType;
            if (fubuRequest.Has(modelType))
            {
                property.SetValue(context.Object, fubuRequest.Get(modelType), null);
            }
        }
    }
}