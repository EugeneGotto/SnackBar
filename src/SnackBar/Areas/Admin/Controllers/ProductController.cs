using AutoMapper;
using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.Controllers;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System.Globalization;
using System.Web.Mvc;

namespace SnackBar.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private Logger _logger = LogManager.GetLogger("AdminProducts");

        public ProductController(IServiceFactory factory) : base(factory)
        {
        }

        [HttpGet]
        public ActionResult EditProducts()
        {
            var prodService = Factory.GetProductService();
            return View(prodService.GetAllProducts());
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            var prodService = Factory.GetProductService();
            var product = prodService.GetProductById((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }

            _logger.Info($"{User.Identity.Name} wants Edit product \"- {product.Name} - {product.Count} pcs. - {product.Price}  BYN - {product.Barcode}-\"");
            TempData["TagList"] = Factory.GetTagService().GetAllTags();

            var model = Mapper.Map<ProductEditViewModel>(product);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductEditViewModel model, long[] selectedTags)
        {
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            model.Price = model.Price.Replace(",", separator).Replace(".", separator).Trim();
            decimal price = 0M;
            if (!decimal.TryParse(model.Price, out price))
            {
                return HttpNotFound();
            }

            var product = Mapper.Map<Product>(model);
            product.Price = price;

            var prodService = Factory.GetProductService();
            prodService.UpdateProduct(product, selectedTags);
            _logger.Info($"{User.Identity.Name} was Updated product. New \"- {product.Name} - {product.Count} pcs. - {product.Price}  BYN - {product.Barcode}-\"");
            return RedirectToAction("EditProducts");
        }

        [HttpGet]
        public ActionResult Create()
        {
            TempData["TagList"] = Factory.GetTagService().GetAllTags();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductEditViewModel model, long[] selectedTags)
        {
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            model.Price = model.Price.Replace(",", separator).Replace(".", separator).Trim();
            decimal price = 0M;
            if (!decimal.TryParse(model.Price, out price))
            {
                return HttpNotFound();
            }

            var product = Mapper.Map<Product>(model);
            product.Price = price;

            var prodService = Factory.GetProductService();
            _logger.Info($"{User.Identity.Name} start add new Product");
            var result = prodService.AddNewProduct(product, selectedTags);
            if (result == null)
            {
                ViewBag.ErrorMessage = "This product has already been added";
                _logger.Info("Product wasn't added");
                return View("Error");
            }

            _logger.Info($"Added new product \"{result.Name}\" - \"{result.Price} BYN\" - {result.Count} pcs. - {result.Barcode}");
            return RedirectToAction("EditProducts");
        }

        [HttpGet]
        public ActionResult Delete(int? id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            var prodService = Factory.GetProductService();
            var name = prodService.DeleteProduct((int)id);
            if (string.IsNullOrEmpty(name))
            {
                ViewBag.ErrorMessage = "Something was wrong";
                _logger.Info($"Product (ID={id}) wasn't deleted");
                return View("Error");
            }

            _logger.Info($"{User.Identity.Name} was moved to Deleted \"{name}\"");
            return RedirectToAction("EditProducts");
        }

        [HttpGet]
        public ActionResult DeletedProducts(int? id)
        {
            if (!id.HasValue)
            {
                var delProducts = Factory.GetProductService().GetAllDeletedProducts();
                return View(delProducts);
            }
            else
            {
                var prodService = Factory.GetProductService();
                var name = prodService.ReturnProduct((int)id);

                _logger.Info($"{User.Identity.Name} return \"{name}\" to Bar");
                return RedirectToAction("EditProducts");
            }
        }

        [HttpGet]
        public ActionResult Tags()
        {
            return View(Factory.GetTagService().GetAllTags());
        }

        [HttpPost]
        public ActionResult Tags(string TagName)
        {
            if (string.IsNullOrEmpty(TagName))
            {
                return View();
            }

            var result = Factory.GetTagService().CreateNewTag(TagName);

            return RedirectToAction("Tags");
        }

        [HttpGet]
        public ActionResult EditTag(long? id = 0)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            var tag = Factory.GetTagService().GetTagById(id.Value);

            if (tag == null)
            {
                return this.HttpNotFound();
            }

            return View(tag);
        }

        [HttpPost]
        public ActionResult EditTag(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Tags");
            }

            tag = Factory.GetTagService().UpdateTag(tag);

            if (tag == null)
            {
            }

            return this.RedirectToAction("Tags");
        }

        public ActionResult DeleteTag(long? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            if (Factory.GetTagService().DeleteTag(id.Value))
            {
                return this.RedirectToAction("Tags");
            }
            else
            {
                return this.RedirectToAction("Tags");
            }
        }
    }
}