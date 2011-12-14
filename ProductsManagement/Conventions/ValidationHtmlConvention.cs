using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuMVC.Core.UI;
using FubuValidation.Fields;
using ProductsManagement.Infrastructure.Validation;

namespace ProductsManagement.Conventions
{
    public class ValidationHtmlConvention : IActivator
    {
        private readonly BehaviorGraph _graph;
        private readonly IFieldValidationQuery _fieldValidationQuery;
        private readonly HtmlConventionRegistry _htmlConvention;
        private readonly IEnumerable<IHtmlValidationConvention> _conventions;

        public ValidationHtmlConvention(BehaviorGraph graph, IFieldValidationQuery fieldValidationQuery, HtmlConventionRegistry htmlConvention,IEnumerable<IHtmlValidationConvention> conventions )
        {
            _graph = graph;
            _fieldValidationQuery = fieldValidationQuery;
            _htmlConvention = htmlConvention;
            _conventions = conventions;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            // TODO: HOLY MOLY! SO MANY LOOPS! JUST FOR DEMO SAKE.

            var inputTypes = _graph.Behaviors.Where(behavior => behavior.InputType() != null).Select(x => x.InputType()).Distinct();
            foreach (var inputType in inputTypes)
            {
                var accessors = inputType.GetProperties()
                    .Select(x => new SingleProperty(x, inputType))
                    .ToList();

                var localInputType = inputType;
                foreach (var accessor in accessors)
                {
                    var localAccessor = accessor;
                    var rules = _fieldValidationQuery.RulesFor(accessor).ToList();
                    if (rules.Count == 0)
                    {
                        continue;
                    }
                    foreach (var rule in rules)
                    {
                        var localRule = rule;
                        _htmlConvention
                            .Editors
                            .If(x => x.ModelType == localInputType && x.Accessor.InnerProperty == localAccessor.InnerProperty)
                            .Modify((request, tag) =>
                            {
                                var ruleDef = new HtmlRuleDef
                                {
                                    Accessor = localAccessor,
                                    RuleInstance = localRule,
                                    RuleType = localRule.GetType(),
                                };
                                var conventions = _conventions.Where(convention => convention.Applies(ruleDef));
                                foreach (var convention in conventions)
                                {
                                    convention.Apply(ruleDef, request, tag);
                                }
                            });
                    }
                }
            }
        }
    }
}