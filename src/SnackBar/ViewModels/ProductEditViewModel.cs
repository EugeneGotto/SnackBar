using SnackBar.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace SnackBar.ViewModels
{
    public class ProductEditViewModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resx.Resource))]
        public string Name { get; set; }

        /// <summary>
        /// Product Count
        /// </summary>
        [Required]
        [Display(Name = "Count", ResourceType = typeof(Resx.Resource))]
        public byte Count { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        [Required]
        [Display(Name = "Price", ResourceType = typeof(Resx.Resource))]
        [RegularExpression(@"^-?\d{1,3}([.,]\d{1,2})?$", ErrorMessageResourceName = "MoneyValidation", ErrorMessageResourceType = typeof(Resx.Resource))]
        public string Price { get; set; }

        /// <summary>
        /// Product Barcode
        /// </summary>
        [Required]
        [Display(Name = "Barcode", ResourceType = typeof(Resx.Resource))]
        public string Barcode { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        [Display(Name = "Tags", ResourceType = typeof(Resx.Resource))]
        public virtual ICollection<Tag> Tags
        {
            get; set;
        }

        public string TagStringEn
        {
            get
            {
                if (this.Tags == null || this.Tags.Count == 0)
                {
                    return string.Empty;
                }

                var sb = new StringBuilder();
                foreach (var tag in this.Tags)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(tag.TagName);
                    }
                    else
                    {
                        sb.Append(", " + tag.TagName);
                    }
                }

                return sb.ToString();
            }
        }

        public string TagStringRu
        {
            get
            {
                if (this.Tags == null || this.Tags.Count == 0)
                {
                    return string.Empty;
                }

                var sb = new StringBuilder();
                foreach (var tag in this.Tags)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(string.IsNullOrEmpty(tag.TagNameRu) ? tag.TagName : tag.TagNameRu);
                    }
                    else
                    {
                        sb.Append(", " + (string.IsNullOrEmpty(tag.TagNameRu) ? tag.TagName : tag.TagNameRu));
                    }
                }

                return sb.ToString();
            }
        }
    }
}