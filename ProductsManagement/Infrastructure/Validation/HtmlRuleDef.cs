using System;
using FubuCore.Reflection;
using FubuValidation.Fields;

namespace ProductsManagement.Infrastructure.Validation
{
    public class HtmlRuleDef : IHtmlRuleDef
    {
        public Accessor Accessor { get; set; }
        public Type RuleType { get; set; }
        public IFieldValidationRule RuleInstance { get; set; }
    }
    public interface IHtmlRuleDef
    {
        Accessor Accessor { get; set; }
        Type RuleType { get; set; }
        IFieldValidationRule RuleInstance { get; set; }
    }

}