using System.Threading;
using AutoMapper;
using Bottles;
using FubuLocalization.Basic;
using FubuMVC.Core;
using FubuMVC.Core.Localization;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.UI;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using FubuValidation.Fields;
using ProductsManagement.Conventions;
using ProductsManagement.Domain;
using ProductsManagement.Handlers.Products;
using ProductsManagement.Infrastructure;
using ProductsManagement.Infrastructure.Validation;

namespace ProductsManagement
{
    public class CoreRegistry : FubuRegistry
    {
        public CoreRegistry()
        {
            Applies.ToThisAssembly();
            IncludeDiagnostics(true);

            Actions.IncludeTypesNamed(x => x.EndsWith("Handler"));

            Routes
                .ConstrainToHttpMethod(x => x.Method.Name.EndsWith("Query"), "GET")
                .ConstrainToHttpMethod(x => x.Method.Name.EndsWith("Command"), "POST");

            Routes
                .IgnoreNamespaceText(typeof(CoreRegistry).Namespace)
                .IgnoreNamespaceText("Handlers")
                .IgnoreMethodsNamed("Query")
                .IgnoreMethodsNamed("Command")
                .IgnoreClassSuffix("Handler");

            Routes.HomeIs<HomeHandler>(x => x.Query(new HomeQueryModel()));
            Views.TryToAttachWithDefaultConventions()
                .RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));

            this.UseSpark();

            Policies.ConditionallyWrapBehaviorChainsWith<HandlerAssetConvention>(x => x.HandlerType.Namespace.Contains("Products"));

            Policies.Add(new ProductNotFoundPolicy<EditHandler>(h => h.Query(null)));
            Policies.Add(new ProductNotFoundPolicy<DeleteHandler>(h => h.Query(null)));

            Output.ToJson.WhenCallMatches(x => x.Method.Name.Equals("Data"));

            Import<BasicLocalizationSupport>(x =>
            {
                x.LocalizationStorageIs<InMemoryLocalizationStorage>();
                x.LoadLocalizationWith<LocalizationActivator>();
                x.DefaultCulture = Thread.CurrentThread.CurrentUICulture;
            });


            var validationRulesHtmlConventionActivator = ObjectDef.ForType<ValidationHtmlConvention>();
            var validationRulesHtmlConvention = new HtmlConventionRegistry();
            validationRulesHtmlConventionActivator.DependencyByValue(validationRulesHtmlConvention);

            Services(cfg =>
                         {
                             cfg.SetServiceIfNone<IProductService, InMemoryProductService>();
                             cfg.SetServiceIfNone(Mapper.Engine);
                             cfg.SetServiceIfNone(Mapper.Configuration);
                             cfg.SetServiceIfNone<ILocaleCacheFactory, LocaleCacheFactory>();
                             cfg.SetServiceIfNone<IValidationDescriptorProvider>(new ValidationDescriptorProvider());
                             cfg.AddService<IHtmlValidationConvention, LocalizedNameConvention>();
                             cfg.AddService<IHtmlValidationConvention, RequiredHtmlValidationConvention>();
                             cfg.AddService<IHtmlValidationConvention, GreaterOrEqualToZeroHtmlValidationConvention>();
                             cfg.AddService<IHtmlValidationConvention, MaximumLengthHtmlValidationConvention>();
                             cfg.AddService<IFieldValidationSource, AddProductModelValidationSource>();
                             cfg.AddService<IActivator, AutoMapperActivator>();
                             cfg.AddService<IActivator, ValidationDescriptorProviderFiller>();
                             cfg.AddService(typeof (IActivator), validationRulesHtmlConventionActivator);
                         });

            HtmlConvention(htmlConventions);
            HtmlConvention(validationRulesHtmlConvention);

            this.Validation(validation =>
            {
                validation.Actions.Include(call => call.HasInput && call.Method.Name.Equals("Command"));
                validation.Failures
                    .IfModelIs<EditProductCommandModel>().TransferBy<ValidationDescriptor>();
            });

            Models.BindPropertiesWith<OriginalModelBinder>();

        }

        private static void htmlConventions(HtmlConventionRegistry x)
        {
            x.Editors
                .Always
                .Modify((e, tag) => tag.Attr("placeholder", e.Accessor.Name));

            x.Editors
                .If(p => p.Accessor.InnerProperty.Name.Equals("Description"))
                .Modify((e, tag) => tag.TagName("textarea").RemoveAttr("value").Text(e.Value<string>()));

            x.Editors
                .If(p => p.Accessor.InnerProperty.Name.Equals("Quantity"))
                .Modify((e, tag) => tag.AddClass("mini"));
        }
    }
}