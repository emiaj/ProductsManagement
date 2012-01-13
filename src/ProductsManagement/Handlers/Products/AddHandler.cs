using AutoMapper;
using FubuMVC.Core.Ajax;
using FubuValidation;
using ProductsManagement.Domain;
using ProductsManagement.Domain.Entities;

namespace ProductsManagement.Handlers.Products
{
    public class AddHandler
    {
        private readonly IProductService _service;
        private readonly IMappingEngine _mapper;

        public AddHandler(IProductService service, IMappingEngine mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public AddProductModel Query(AddProductRequest model)
        {
            return new AddProductModel();
        }

        public AjaxContinuation Command(AddProductModel model)
        {
            var product = _mapper.Map<AddProductModel, Product>(model);
            _service.Save(product);
            var result = AjaxContinuation.Successful();
            result["id"] = product.Id;
            return result;
        }
    }

    public class AddProductRequest
    {

    }

    public class AddProductModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [GreaterOrEqualToZero]
        public int? Quantity { get; set; }
    }
}