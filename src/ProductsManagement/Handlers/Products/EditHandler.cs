using AutoMapper;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuValidation;
using ProductsManagement.Domain;

namespace ProductsManagement.Handlers.Products
{
    public class EditHandler
    {
        private readonly IProductService _productService;
        private readonly IMappingEngine _mapper;

        public EditHandler(IProductService productService, IMappingEngine mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public EditProductModel Query(EditProductModel model)
        {
            var product = _productService.GetById(model.Id);
            _mapper.Map(product, model);
            return model;
        }

        public FubuContinuation Command(EditProductCommandModel model)
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

    public class EditProductCommandModel
    {
        [RouteInput]
        public int Id { get; set; }
        [Required]
        [MaximumStringLength(20)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [GreaterOrEqualToZero]
        public int? Quantity { get; set; }
    }

    public class EditProductModel : EditProductCommandModel
    {
        public EditProductCommandModel OriginalModel { get; set; }
    }
}