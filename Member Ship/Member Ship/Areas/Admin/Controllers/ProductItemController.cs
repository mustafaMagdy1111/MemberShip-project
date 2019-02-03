using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Member_Ship.Entities;
using Member_Ship.Models;
using Member_Ship.Areas.Admin.Models;
using Member_Ship.Extensions;

namespace Member_Ship.Areas.Admin.Controllers
{
    public class ProductItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductItem
        public async Task<ActionResult> Index()
        {
            var model = await db.ProductItems.Convert(db);
            return View(model);
        }

        // GET: Admin/ProductItem/Details/5
        public async Task<ActionResult> Details(int? itemId,int?productId)
        {
            if (itemId == null|| productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.Convert(db));
        }

        // GET: Admin/ProductItem/Create
          public async Task<ActionResult> Create()
            {
                var model = new ProductItemModel
                {
                    Items = await db.items.ToListAsync(),
                    Products = await db.products.ToListAsync()
                };
                return View(model);
            }
        

        // POST: Admin/ProductItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
             ProductItemModel productItem)
        {
            if (ModelState.IsValid)
            {
                ProductItem x = new ProductItem();
                x.ItemtId = productItem.ItemId;
                x.ProductId = productItem.ProductId;
                
                db.ProductItems.Add(x);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productItem);
        }

        // GET: Admin/ProductItem/Edit/5
        public async Task<ActionResult> Edit(int? itemId,int?productId)
        {
            if (itemId == null||productId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.Convert(db));
        }

        // POST: Admin/ProductItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
                               ProductItemModel productItem)
        {
            if (ModelState.IsValid)
            {
                ProductItem pd = new ProductItem();
                pd.ItemtId = productItem.ItemId;
                pd.ProductId = productItem.ProductId;
                pd.OldItemtId = productItem.OldItemtId;
                pd.OldProductId = productItem.OldProductId;
                
                var canchange = await pd.CanChange(db);
                if (canchange)
                {
                    var remove=db.ProductItems.FirstOrDefault(pi =>
                             pi.ProductId.Equals(productItem.OldProductId) &&
                             pi.ItemtId.Equals(productItem.OldItemtId));

                    db.ProductItems.Remove(remove);
                    db.ProductItems.Add(pd); 
                    await db.SaveChangesAsync();
                }

                else
                {
                    var modell = await db.ProductItems.Convert(db);

                    return RedirectToAction("index",modell);
                }
            }

            var model = await db.ProductItems.Convert(db);
            return View("Index",model);
        }

        // GET: Admin/ProductItem/Delete/5
        public async Task<ActionResult> Delete(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.Convert(db));
        }

        // POST: Admin/ProductItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> DeleteConfirmed(int itemId,int productId)
        {
            ProductItem productItem = await GetProductItem(itemId, productId);
            db.ProductItems.Remove(productItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private async Task<ProductItem>GetProductItem(int? itemId,int? productId)
        {
            try
            {
                int itmId, prdId = 0;
                int.TryParse(itemId.ToString(), out itmId);
                int.TryParse(productId.ToString(), out prdId);
                var productItem = await db.ProductItems.FirstOrDefaultAsync(
                    pi => pi.ProductId.Equals(prdId) && pi.ItemtId.Equals(itmId));
                return productItem;
;
            }
            catch 
            {

                return null;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
