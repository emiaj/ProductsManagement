using AutoMapper;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuValidation;
using ProductsManagement.Domain;

namespace ProductsManagement.Handlers.Products
{
    public class EditHandler
    {
        private readonly IProductService _productService;
        private readonly IMappingEngine _mapper;
        private readonly IFubuRequest _fubuRequest;

        public EditHandler(IProductService productService, IMappingEngine mapper,IFubuRequest fubuRequest)
        {
            _productService = productService;
            _mapper = mapper;
            _fubuRequest = fubuRequest;
        }

        public EditProductModel Query(EditProductRequest request)
        {
            // NOTE: IS THERE A WAY TO DO THIS WITHOUT USING THIS "IF" STATEMENT?
            if(_fubuRequest.Has<EditProductModel>())
            {
                return _fubuRequest.Get<EditProductModel>();
            }
            var product = _productService.GetById(request.Id);
            var model = _mapper.Map<EditProductModel>(product);
            return model;
        }

        public FubuContinuation Command(EditProductModel model)
        {
            var product = _productService.GetById(model.Id);
            _mapper.Map(model, product);
            return FubuContinuation.RedirectTo(new SuccessOperationRequest
            {
                Id = model.Id,
                Operation = OperationType.Update
            });
        }
    }

    public class EditProductRequest
    {
        [RouteInput]
        public int Id { get; set; }
    }

    public class EditProductModel
    {
        [RouteInput]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [GreaterOrEqualToZero]
        public int? Quantity { get; set; }
    }
}