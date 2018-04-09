using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Product Model
    /// </summary>
    public class Product : BaseModel
    {
        /// <summary>
        /// Initialize new Collection in constructor
        /// </summary>
        public Product()
        {
            this.Tags = new List<Tag>();
        }

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
        public decimal Price { get; set; }

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

        /// <summary>
        /// String with tags at English
        /// </summary>
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

        /// <summary>
        /// String with tags at Russian
        /// </summary>
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