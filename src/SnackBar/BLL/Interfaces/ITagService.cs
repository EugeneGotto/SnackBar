using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System.Collections.Generic;

namespace SnackBar.BLL.Interfaces
{
    public interface ITagService
    {
        /// <summary>
        /// Create new Tag
        /// </summary>
        /// <param name="name">Tag Name</param>
        /// <returns>Created Tag</returns>
        Tag CreateNewTag(string name);

        /// <summary>
        /// Update Tag
        /// </summary>
        /// <param name="tag">Update Tag</param>
        /// <returns>Updated Tag or null</returns>
        Tag UpdateTag(Tag tag);

        /// <summary>
        /// Get Tag by ID
        /// </summary>
        /// <param name="id">Tag ID</param>
        /// <returns>Tag</returns>
        Tag GetTagById(long id);

        /// <summary>
        /// Get all tag Names
        /// </summary>
        /// <returns>Tag's Name list</returns>
        ICollection<string> GetTagList();

        /// <summary>
        /// Get all Tags
        /// </summary>
        /// <returns>Tags</returns>
        ICollection<TagViewModel> GetAllTags();

        /// <summary>
        /// Add tag to product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="tagId">Added Tag ID</param>
        /// <returns>Add successfull or not</returns>
        bool AddTagToProduct(long productId, long tagId);

        /// <summary>
        /// Add tag list to product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="tagIds">Added Tag ID List</param>
        /// <returns>Add successfull or not</returns>
        bool AddTagListToProduct(long productId, long[] tagIds);

        /// <summary>
        /// Get Products by tags
        /// </summary>
        /// <param name="tagIds">Tag ID list</param>
        /// <returns>Products by tags</returns>
        ICollection<Product> GetProductsByTags(long[] tagIds);

        /// <summary>
        /// Get Products by tag ID
        /// </summary>
        /// <param name="tagId">Tag ID</param>
        /// <returns>Products with Tag</returns>
        ICollection<Product> GetProductsByTagId(long tagId);

        /// <summary>
        /// Delete Tag from DB
        /// </summary>
        /// <param name="tagId">Deleted Tag ID</param>
        /// <returns>Successfull or not</returns>
        bool DeleteTag(long tagId);
    }
}