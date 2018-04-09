using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SnackBar.DAL.Models
{
    /// <summary>
    /// Tag model
    /// </summary>
    public class Tag : BaseModel
    {
        /// <summary>
        /// Initialize new Collection in Constructor
        /// </summary>
        public Tag()
        {
            this.Products = new List<Product>();
        }

        /// <summary>
        /// Tag Name at English
        /// </summary>
        [Required]
        [Display(Name = "EnName", ResourceType = typeof(Resx.Resource))]
        public string TagName
        {
            get; set;
        }

        /// <summary>
        /// Tag Name at Russian
        /// </summary>
        [Display(Name = "RuName", ResourceType = typeof(Resx.Resource))]
        public string TagNameRu
        {
            get; set;
        }

        /// <summary>
        /// Products List
        /// </summary>
        public virtual ICollection<Product> Products
        {
            get; set;
        }
    }
}