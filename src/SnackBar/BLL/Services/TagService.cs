using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SnackBar.BLL.Services
{
    public class TagService : BaseService, ITagService
    {
        private Logger _logger = LogManager.GetLogger("Tags");

        public TagService(IDalFactory factory) : base(factory)
        {
        }

        public Tag CreateNewTag(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var tag = uow.Repository<Tag>().Find(t => !t.IsDeleted).FirstOrDefault(t => t.TagName.ToUpper().Equals(name.ToUpper()));
                if (tag != null)
                {
                    return tag;
                }

                tag = new Tag()
                {
                    TagName = name
                };

                tag = uow.Repository<Tag>().AddOrUpdate(tag);
                uow.Save();

                return tag;
            });
        }

        public Tag UpdateTag(Tag tag)
        {
            if (tag == null || tag.Id == 0)
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var tagDb = uow.Repository<Tag>().GetById(tag.Id);
                if (tagDb == null)
                {
                    return null;
                }

                tagDb.TagName = tag.TagName;
                tagDb.TagNameRu = tag.TagNameRu;

                tag = uow.Repository<Tag>().AddOrUpdate(tagDb);
                uow.Save();

                return tag;
            });
        }

        public Tag GetTagById(long id)
        {
            if (id <= 0)
            {
                return null;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                return uow.Repository<Tag>().GetById(id);
            });
        }

        public ICollection<string> GetTagList()
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                return uow.Repository<Tag>()
                .Find(t => !t.IsDeleted)
                .Select(t => t.TagName)
                .ToList();
            });
        }

        public ICollection<TagViewModel> GetAllTags()
        {
            return this.InvokeInUnitOfWorkScope(uow =>
               {
                   return uow.Repository<Tag>()
                   .Find(t => !t.IsDeleted)
                   .Select(t => new TagViewModel()
                   {
                       Id = t.Id,
                       Name = t.TagName,
                       NameRu = (string.IsNullOrEmpty(t.TagNameRu) ? t.TagName : t.TagNameRu),
                       Count = t.Products.Where(p => p.Count > 0).Count()
                   })
                   .OrderByDescending(t => t.Count)
                   .ToList();
               });
        }

        public bool AddTagToProduct(long productId, long tagId)
        {
            if (productId <= 0 || tagId <= 0)
            {
                return false;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var product = uow.Repository<Product>().GetById(productId);
                if (product == null)
                {
                    _logger.Warn($"Can't find product with ID={productId}");
                    return false;
                }

                var tag = uow.Repository<Tag>().GetById(tagId);
                if (tag == null)
                {
                    _logger.Warn($"Can't find tag with ID={tagId}");
                    return false;
                }

                if (product.Tags.Contains(tag))
                {
                    return true;
                }

                product.Tags.Add(tag);

                uow.Repository<Product>().AddOrUpdate(product);
                try
                {
                    uow.Save();
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }

        public bool AddTagListToProduct(long productId, long[] tagIds)
        {
            if (productId <= 0)
            {
                return false;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var product = uow.Repository<Product>().GetById(productId);
                if (product == null)
                {
                    _logger.Warn($"Can't find product with ID={productId}");
                    return false;
                }

                foreach (var tagId in tagIds)
                {
                    if (tagId <= 0)
                    {
                        continue;
                    }

                    var tag = uow.Repository<Tag>().GetById(tagId);
                    if (tag == null)
                    {
                        _logger.Warn($"Can't find tag with ID={tagId}");
                        continue;
                    }

                    if (product.Tags.Contains(tag))
                    {
                        continue;
                    }

                    product.Tags.Add(tag);

                    uow.Repository<Product>().AddOrUpdate(product);
                }

                try
                {
                    uow.Save();
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }

        public ICollection<Product> GetProductsByTags(long[] tagIds)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var tags = uow.Repository<Tag>()
                .Find(t => !t.IsDeleted)
                .Include(t => t.Products)
                .Where(t => tagIds.Contains(t.Id))
                .ToList();

                var result = new List<Product>();
                foreach (var tag in tags)
                {
                    result.AddRange(tag.Products);
                }

                return result;
            });
        }

        public ICollection<Product> GetProductsByTagId(long tagId)
        {
            return this.InvokeInUnitOfWorkScope(uow =>
            {
                var result = new List<Product>();
                var tag = uow.Repository<Tag>()
                .GetById(tagId);

                if (tag == null)
                {
                    return result;
                }

                result.AddRange(tag.Products.Where(p => p.Tags.Contains(tag) && p.Count > 0).OrderBy(p => p.Name));

                return result;
            });
        }

        public bool DeleteTag(long tagId)
        {
            if (tagId <= 0)
            {
                return false;
            }

            return this.InvokeInUnitOfWorkScope(uow =>
            {
                try
                {
                    uow.Repository<Tag>().DeleteById(tagId);
                    uow.Save();
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }
    }
}