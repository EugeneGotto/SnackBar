using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SnackBar.BLL.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Add new product to Bar
        /// </summary>
        /// <param name="product">New Product</param>
        /// <param name="tagsIds">Selected Tags</param>
        /// <returns>Added Product</returns>
        Product AddNewProduct(Product product, params long[] tagsIds);

        /// <summary>
        /// Get Product by Barcode
        /// </summary>
        /// <param name="barcode">Product Barcode</param>
        /// <returns>Product</returns>
        Product GetProduct(string barcode);

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>Product</returns>
        Product GetProductById(long productId);

        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="filter">Expression Filter</param>
        /// <returns>Products</returns>
        IEnumerable<Product> GetAllProducts(Expression<Func<Product, bool>> filter = null);

        /// <summary>
        /// Get all early deleted products
        /// </summary>
        /// <param name="filter">Expression Filter</param>
        /// <returns>Early deleted Products</returns>
        IEnumerable<Product> GetAllDeletedProducts(Expression<Func<Product, bool>> filter = null);

        /// <summary>
        /// Update Product in DB
        /// </summary>
        /// <param name="product">Updated Product</param>
        /// <param name="tagsIds">Selected Tags</param>
        void UpdateProduct(Product product, params long[] tagsIds);

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="productId">Product ID</param>
        string DeleteProduct(long productId);

        /// <summary>
        /// Return to Bar early deleted Product
        /// </summary>
        /// <param name="productId">Product ID</param>
        string ReturnProduct(long productId);
    }
}