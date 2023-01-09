using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;



namespace OnlineStore.Controllers
{
    [Authorize(Policy = "OnlyForAdmin")]
    public class AdministratorProductsController: Controller
    {
        ApplicationContext db;
        public AdministratorProductsController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> AdministratorProducts()
        {
            List<Product> products = await db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.imagesProduct).ToListAsync();
            products.Sort();
            List<ProductIsSelect> productsIsSelect = new List<ProductIsSelect>();
            for (int i = 0; i < products.Count; i++)
            {
                productsIsSelect.Add(new ProductIsSelect(products[i]));
            }
            ProductViewModel productViewModel = new ProductViewModel { productIsSelects = productsIsSelect };
            if (TempData["ErrDelMessage"] != null)
            {
                ViewBag.ErrMessage = TempData["ErrDelMessage"].ToString();
            }
            return View(productViewModel);
        }

        public async Task<IActionResult> AdministratorAddProduct()
        {
            ProductAddViewModel productAddViewModel = new ProductAddViewModel();
            productAddViewModel.categories = await db.Categories.ToListAsync();
            productAddViewModel.categories.Sort();
            productAddViewModel.manufacturers = await db.Manufacturers.ToListAsync();
            productAddViewModel.manufacturers.Sort();
            return View(productAddViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorAddProduct(Product product, String category, String manufacturer, IFormFileCollection imagesOfProduct)
        {
            
            Product? cheackProduct = await db.Products.FirstOrDefaultAsync(p => p.Name == product.Name);
            if (cheackProduct != null)
            {
                ViewBag.ErrorName = "Такой продукт уже существует";
                ProductAddViewModel productAddViewModel = new ProductAddViewModel();
                productAddViewModel.categories = await db.Categories.ToListAsync();
                productAddViewModel.categories.Sort();
                productAddViewModel.manufacturers = await db.Manufacturers.ToListAsync();
                productAddViewModel.manufacturers.Sort();
                return View(productAddViewModel);
            }
            Product newProduct = product;
            newProduct.Category = db.Categories.FirstOrDefault(c => c.Name == category);
            newProduct.Manufacturer = db.Manufacturers.FirstOrDefault(m => m.Name == manufacturer);
            newProduct.imagesProduct = new List<ImageProduct>();

            db.Products.Add(UploadImages(newProduct, imagesOfProduct));
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorProducts");
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorDeleteProduct(ProductViewModel selectedProducts)
        {
            Product? product;
            foreach (var item in selectedProducts.productIsSelects)
            {
                if (item.IsSelect == true)
                {
                    product = await db.Products.FirstOrDefaultAsync(p => p.Id == item.Id);
                    if (product != null)
                    {
                        List<Buy>? buys = await db.Buys.Include(p=>p.Product).ToListAsync();
                        if (buys != null)
                        {
                            for (int i = 0; i < buys.Count; i++)
                            {
                                if (product.Name == buys[i].Product.Name)
                                {
                                    TempData["ErrDelMessage"] = "Ошибка удаления!!! Товар был в заказах!";
                                    return RedirectToAction("AdministratorProducts");
                                }
                            }
                        }
                        
                        List<ImageProduct> imagesOfProductDelete = await db.ImageProducts.Where(im => im.ProductId == product.Id).ToListAsync();
                        foreach (var image in imagesOfProductDelete)
                        {
                            System.IO.File.Delete(image.Path);
                            db.ImageProducts.Remove(image);
                            await db.SaveChangesAsync();
                        }
                        db.Products.Remove(product);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("AdministratorProducts");
        }

        public async Task<IActionResult> AdministratorOpenEditProduct(int? Id)
        {
            ProductEditViewModel productEditViewModel = new ProductEditViewModel();
            productEditViewModel.Product = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);
            productEditViewModel.Categories = await db.Categories.ToListAsync();
            productEditViewModel.Manufacturers = await db.Manufacturers.ToListAsync();
            productEditViewModel.ImageProducts = await db.ImageProducts.Where(im => im.ProductId == Id).ToListAsync();
            return View("AdministratorEditProduct", productEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AdministratorEditProduct(Product product, string category, string manufacturer, IFormFileCollection imagesOfProduct)
        {
            product.Category = await db.Categories.FirstOrDefaultAsync(c => c.Name == category);
            product.Manufacturer = await db.Manufacturers.FirstOrDefaultAsync(m => m.Name == manufacturer);
            product.imagesProduct = await db.ImageProducts.Where(im => im.ProductId == product.Id).ToListAsync();
            if (imagesOfProduct != null)
            {
                db.Products.Update(UploadImages(product, imagesOfProduct));
            }
            else
            {
                db.Products.Update(product);
            }
            db.Products.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorProducts");
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorDeleteImage(int? Id)
        {
            ImageProduct? delImageProduct = await db.ImageProducts.FirstOrDefaultAsync(im => im.Id == Id);
            if (delImageProduct != null)
            {
                db.ImageProducts.Remove(delImageProduct);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("AdministratorProducts");
        }


        






        public Product UploadImages(Product product, IFormFileCollection imagesOfProduct)
        {
            string uploadDirectoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\images";
            if (imagesOfProduct != null && imagesOfProduct.Count != 0)
            {
                if (product.imagesProduct != null && product.imagesProduct.Count != 0)
                {
                    for (int i = 0; i < product.imagesProduct.Count; i++)
                    {
                        if (product.imagesProduct[i].Name == "noImage.png")
                        {
                            db.ImageProducts.Remove(product.imagesProduct[i]);
                            db.SaveChanges();
                            product.imagesProduct.Remove(product.imagesProduct[i]);
                            break;
                        }
                    }
                }
                for (int i = 0; i < imagesOfProduct.Count; i++)
                {
                    string fullUploadPath = $"{uploadDirectoryPath}\\{product.Name}_{imagesOfProduct[i].FileName}";
                    using (var fileStream = new FileStream(fullUploadPath, FileMode.Create))
                    {
                        imagesOfProduct[i].CopyTo(fileStream);
                    }
                    ImageProduct imageProduct = new ImageProduct();
                    imageProduct.Name = $"{product.Name}_{imagesOfProduct[i].FileName}";
                    imageProduct.Path = fullUploadPath;
                    db.ImageProducts.Add(imageProduct);
                    db.SaveChanges();

                    if (product != null && product.imagesProduct != null)
                    {
                        product.imagesProduct.Add(db.ImageProducts.FirstOrDefault(im => im.Path == fullUploadPath));
                    }
                    
                }
            }
            else
            {
                ImageProduct imageProduct = new ImageProduct();
                imageProduct.Name = $"noImage.png";
                imageProduct.Path = $"{uploadDirectoryPath}\\noImage.png";

                db.ImageProducts.Add(imageProduct);
                db.SaveChanges();
                if (product != null && product.imagesProduct != null)
                {
                    product.imagesProduct.Add(db.ImageProducts.FirstOrDefault(im => im.Name == imageProduct.Name));
                }
            }
            return product;
        }
    }
}
