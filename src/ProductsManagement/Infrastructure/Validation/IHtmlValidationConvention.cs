using System.Collections.Generic;
using FubuCore;
using FubuLocalization;
using FubuMVC.Core.UI.Configuration;
using FubuValidation;
using FubuValidation.Fields;
using HtmlTags;

namespace ProductsManagement.Infrastructure.Validation
{
    public interface IHtmlValidationConvention
    {
        bool Applies(IHtmlRuleDef def);
        void Apply(IHtmlRuleDef def, ElementRequest request, HtmlTag tag);
    }

    public class LocalizedNameConvention : IHtmlValidationConvention
    {
        public bool Applies(IHtmlRuleDef def)
        {
            return true;
        }

        public void Apply(IHtmlRuleDef def, ElementRequest request, HtmlTag tag)
        {
            var localizedName = LocalizationManager.GetHeader(def.Accessor.InnerProperty);
            tag.MetaData("localizedName", localizedName);
        }
    }

    public class RequiredHtmlValidationConvention : IHtmlValidationConvention
    {
        public bool Applies(IHtmlRuleDef def)
        {
            return def.RuleType == typeof(RequiredFieldRule);
        }

        public void Apply(IHtmlRuleDef def, ElementRequest request, HtmlTag tag)
        {
            tag.Rules("required");
            tag.Messages("required", ValidationKeys.REQUIRED.ToString());
        }
    }

    public class GreaterOrEqualToZeroHtmlValidationConvention : IHtmlValidationConvention
    {
        public bool Applies(IHtmlRuleDef def)
        {
            return def.RuleType == typeof(GreaterOrEqualToZeroRule);
        }

        public void Apply(IHtmlRuleDef def, ElementRequest request, HtmlTag tag)
        {
            tag.Rules("min", 0);
            tag.Messages("min", ValidationKeys.GREATER_OR_EQUAL_TO_ZERO.ToString());
        }
    }

    public class MaximumLengthHtmlValidationConvention : IHtmlValidationConvention
    {
        public bool Applies(IHtmlRuleDef def)
        {
            return def.RuleType == typeof (MaximumLengthRule);
        }

        public void Apply(IHtmlRuleDef def, ElementRequest request, HtmlTag tag)
        {
            var rule = def.RuleInstance.As<MaximumLengthRule>();
            var template = ValidationKeys.MAX_LENGTH.ToString();
            var message = TemplateParser.Parse(template, new Dictionary<string, string> {{MaximumLengthRule.LENGTH, rule.Length.ToString()}});

            // NOTE: LET'S IGNORE THIS JUST FOR DEMO SAKE
            // tag.Attr("maxlength", rule.Length);

            tag.Rules("rangelength", new[] {0, rule.Length});
            tag.Messages("rangelength", message);
        }
    }
}