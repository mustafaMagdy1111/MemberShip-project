using Member_Ship.Areas.Admin.Models;
using Member_Ship.Entities;
using Member_Ship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Transactions;

namespace Member_Ship.Extensions
{
    public static class ConverstionExtension
    {
        public static async Task<IEnumerable<ProductModel>>
            Convert(this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products.Count().Equals(0))
                return new List<ProductModel>();
            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return from p in products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductTypeId = p.ProductTypeId,
                       productLinkTexts = texts,
                       productTypes = types

                   };



        }


        ///producut type details convert method
        public static async Task<ProductModel>
          Convert(this Product product, ApplicationDbContext db)
        {


            var text = await db.ProductLinkTexts.FirstOrDefaultAsync(p => p.Id.Equals(product.ProductLinkTextId));
            var type = await db.ProductTypes.FirstOrDefaultAsync(p => p.Id.Equals(product.ProductTypeId));


            var model= new ProductModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                ProductLinkTextId = product.ProductLinkTextId,
                ProductTypeId = product.ProductTypeId,
                productLinkTexts = new List<ProductLinkText>(),
                productTypes = new List<ProductType>()

            };
            model.productLinkTexts.Add(text);
            model.productTypes.Add(type);

            return model;
        }


        public static async Task<IEnumerable<ProductItemModel>> Convert(
         this IQueryable<ProductItem> productItems, ApplicationDbContext db)
        {
            if (productItems.Count().Equals(0))
                return new List<ProductItemModel>();

            return await (from pi in productItems
                          select new ProductItemModel
                          {
                              ItemId = pi.ItemtId,
                              ProductId = pi.ProductId,
                              ItemTitle = db.items.FirstOrDefault(
                                  i => i.Id.Equals(pi.ItemtId)).Title,
                              ProductTitle = db.products.FirstOrDefault(
                                  p => p.Id.Equals(pi.ProductId)).Title
                              }).ToListAsync();
        }
        
        ///producutItem type Edit convert method
        public static async Task<ProductItemModel>
          Convert(this ProductItem  productItem, ApplicationDbContext db,
            bool addListData=true)
        {
            
            var model = new ProductItemModel
            {
               ItemId=productItem.ItemtId,
               ProductId=productItem.ProductId,
               Items= addListData? await db.items.ToListAsync():null,
               Products= addListData ?  await db.products.ToListAsync():null,
               ItemTitle=(await db.items.FirstOrDefaultAsync(i=>i.Id.Equals(productItem.ItemtId))).Title,
                ProductTitle = (await db.products.FirstOrDefaultAsync(i => i.Id.Equals(productItem.ProductId))).Title,

            };
    

            return model;
        }

        public static async Task<bool> CanChange(
           this ProductItem productItem, ApplicationDbContext db)
        {
            var oldPI = await db.ProductItems.CountAsync(pi =>
                pi.ProductId.Equals(productItem.OldProductId) &&
                pi.ItemtId.Equals(productItem.OldItemtId));

            var newPI = await db.ProductItems.CountAsync(pi =>
                pi.ProductId.Equals(productItem.ProductId) &&
                pi.ItemtId.Equals(productItem.ItemtId));

            return oldPI.Equals(1) && newPI.Equals(0);
        }

        public static async Task Change(
            this ProductItem productItem, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(
                    pi => pi.ProductId.Equals(productItem.OldProductId) &&
                    pi.ItemtId.Equals(productItem.OldItemtId));

            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(
                pi => pi.ProductId.Equals(productItem.ProductId) &&
                pi.ItemtId.Equals(productItem.ItemtId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem
                {
                    ItemtId = productItem.ItemtId,
                    ProductId = productItem.ProductId
                };

                using (var transaction = new TransactionScope(
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);

                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }
            }
        }

    }
}
