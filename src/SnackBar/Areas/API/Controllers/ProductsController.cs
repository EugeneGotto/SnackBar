using AutoMapper;
using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Web.Http;

namespace SnackBar.Areas.API.Controllers
{
    public class ProductsController : BaseApiController
    {
        public ProductsController(IServiceFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Get all available products
        /// </summary>
        [Route("Api/Products")]
        [SwaggerResponse(200, Description = "Get all Products", Type = (typeof(IEnumerable<ProductApiViewModel>)))]
        public IEnumerable<ProductApiViewModel> GetProducts()
        {
            var products = Factory.GetProductService().GetAllProducts(p => p.Count > 0);
            var returnResult = Mapper.Map<IEnumerable<ProductApiViewModel>>(products);
            return returnResult;
        }

        // GET: api/Products/5
        /// <summary>
        /// Get Product by ID
        /// </summary>
        /// <param name="productId">Product ID</param>
        [Route("Api/Products/{productId}")]
        [SwaggerResponse(200, Description = "Get Product by ID", Type = (typeof(ProductApiViewModel)))]
        [SwaggerResponse(404, Description = "Product not found")]
        public IHttpActionResult GetProduct(long productId)
        {
            Product product = Factory.GetProductService().GetProductById(productId);
            if (product == null)
            {
                return this.NotFound();
            }

            var returnResult = Mapper.Map<ProductApiViewModel>(product);
            return this.Ok(returnResult);
        }

        /// <summary>
        /// Get Categories
        /// </summary>
        [HttpGet]
        [Route("Api/Products/Categories")]
        [SwaggerResponse(200, Description = "Get Categories List", Type = (typeof(ICollection<TagViewModel>)))]
        [SwaggerResponse(404, Description = "Can't Find any category")]
        public IHttpActionResult GetCategories()
        {
            var categories = Factory.GetTagService().GetAllTags();
            if (categories == null)
            {
                return this.NotFound();
            }

            return this.Ok(categories);
        }

        /// <summary>
        /// Get Products by Category ID
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        [HttpGet]
        [Route("Api/Products/Categories/{categoryId}")]
        [SwaggerResponse(200, Description = "Product by Category ID", Type = (typeof(ICollection<ProductApiViewModel>)))]
        [SwaggerResponse(404, Description = "Products not found")]
        [SwaggerResponse(400, Description = "Incorrect input")]
        public IHttpActionResult GetProductsByCategory(long? categoryId)
        {
            if (!categoryId.HasValue || categoryId.Value <= 0)
            {
                return this.BadRequest();
            }

            var products = Factory.GetTagService().GetProductsByTagId(categoryId.Value);

            if (products == null || products.Count == 0)
            {
                return this.NotFound();
            }

            var returnResult = Mapper.Map<IEnumerable<ProductApiViewModel>>(products);
            return this.Ok(returnResult);
        }
    }
}