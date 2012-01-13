using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Urls;
using ProductsManagement.Domain;

namespace ProductsManagement.Handlers.Products
{
    public class DeleteHandler
    {
        private readonly IProductService _productService;
        private readonly IUrlRegistry _urlRegistry;

        public DeleteHandler(IProductService productService, IUrlRegistry urlRegistry)
        {
            _productService = productService;
            _urlRegistry = urlRegistry;
        }

        public DeleteCommandModel Query(DeleteProductRequest model)
        {
            return new DeleteCommandModel
            {
                Id = model.Id,
                Name = _productService.GetById(model.Id).Name,
                HomeUrl = _urlRegistry.UrlFor<HomeQueryModel>()
            };
        }

        public FubuContinuation Command(DeleteCommandModel model)
        {
            _productService.Delete(model.Id);
            return FubuContinuation.RedirectTo(new SuccessOperationRequest
            {
                Id = model.Id,
                Operation = OperationType.Delete
            });
        }
    }

    public class DeleteProductRequest
    {
        [RouteInput]
        public int Id { get; set; }
    }

    public class DeleteCommandModel
    {
        [RouteInput]
        public int Id { get; set; }
        public string Name { get; set; }
        public string HomeUrl { get; set; }
    }

}