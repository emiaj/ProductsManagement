using FubuMVC.Core;
using FubuMVC.Core.Urls;
using ProductsManagement.Domain;

namespace ProductsManagement.Handlers.Products
{
    public class SuccessHandler
    {
        private readonly IUrlRegistry _registry;
        private readonly IProductService _service;

        public SuccessHandler(IUrlRegistry registry, IProductService service)
        {
            _registry = registry;
            _service = service;
        }

        public SuccessOperationModel Query(SuccessOperationRequest request)
        {
            var productName = request.Operation == OperationType.Delete ? "" : _service.GetById(request.Id).Name;
            return new SuccessOperationModel
            {
                HomeUrl = _registry.UrlFor<HomeQueryModel>(),
                ProductName = productName,
                Id = request.Id,
                Operation = request.Operation
            };
        }
    }

    public class SuccessOperationModel
    {
        public int Id { get; set; }
        public string HomeUrl { get; set; }
        public string ProductName { get; set; }
        public OperationType Operation { get; set; }
    }

    public class SuccessOperationRequest
    {
        [RouteInput]
        public int Id { get; set; }

        [RouteInput]
        public OperationType Operation { get; set; }
    }
    public enum OperationType
    {
        Add,
        Update,
        Delete
    }
}