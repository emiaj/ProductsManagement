using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuMVC.Core.UI;
using FubuValidation.Fields;
using ProductsManagement.Infrastructure.Validation;

namespace ProductsManagement.Conventions
{
    public class ValidationHtmlConvention : IActivator
    {
        private readonly IFieldValidationQuery _fieldValidationQuery;
        private readonly HtmlConventionRegistry _htmlConvention;
        private readonly IEnumerable<IHtmlValidationConvention> _conventions;

        public ValidationHtmlConvention(IFieldValidationQuery fieldValidationQuery,
                                        HtmlConventionRegistry htmlConvention,
                                        IEnumerable<IHtmlValidationConvention> conventions)
        {
            _fieldValidationQuery = fieldValidationQuery;
            _htmlConvention = htmlConvention;
            _conventions = conventions;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            _htmlConvention.Editors
                .Always.Modify((request, tag) =>
                {
                    var rules = _fieldValidationQuery.RulesFor(request.Accessor);
                    foreach (var rule in rules)
                    {
                        var ruleDef = new HtmlRuleDef
                        {
                            Accessor = request.Accessor,
                            RuleInstance = rule,
                            RuleType = rule.GetType()
                        };
                        foreach (var convention in _conventions.Where(convention => convention.Applies(ruleDef)))
                        {
                            convention.Apply(ruleDef, request, tag);
                        }
                    }
                });
        }
    }
}