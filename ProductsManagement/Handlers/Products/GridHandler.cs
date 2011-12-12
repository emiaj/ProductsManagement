using System;
using System.Collections.Generic;
using System.Linq;
using ProductsManagement.Domain;

namespace ProductsManagement.Handlers.Products
{
    public class GridHandler
    {
        private readonly IProductService _service;

        public GridHandler(IProductService service)
        {
            _service = service;
        }

        public GridData Data(GridRequest request)
        {
            var total = decimal.Divide(_service.AllProducts().Count(), request.Rows);
            var data = new GridData
                           {
                               Total = (int) Math.Ceiling(total),
                               Page = request.Page
                           };
            var products = _service.AllProducts()
                .Skip(request.Index * request.Rows)
                .Take(request.Rows);

            products
                .Select(x => new GridRow
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Description = new string(x.Description.Take(50).ToArray()),
                                     Quantity = x.Quantity
                                 })
                .Each(data.AddRow);
            return data;
        }
    }

    public class GridData
    {
        private readonly List<GridRow> _rows;

        public GridData()
        {
            _rows = new List<GridRow>();
            Rows = _rows;
        }

        public void AddRow(GridRow row)
        {
            _rows.Add(row);
        }

        public IEnumerable<GridRow> Rows { get; private set; }

        public int Records { get { return Rows.Count(); } }

        public int Total { get; set; }

        public int Page { get; set; }

    }

    public class GridRow
    {
        public int Id { get;  set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

    public class GridRequest
    {
        public int Rows { get; set; }
        public int Page { get; set; }
        public int Index { get { return Page - 1; } }
    }

}