using FubuMVC.Core;

namespace ProductsManagement.Handlers.Products
{
    public class NotFoundHandler
    {
        public NotFoundModel Query(NotFoundModel model)
        {
            return model;
        }
    }

    public class NotFoundModel
    {
        [RouteInput]
        public int Id { get; set; }
    }

}