using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.ViewModels;
using FriendBook.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FriendBook.Controllers
{
    public class YardSaleController : Controller
    {
        private IHostingEnvironment _environment;
        private FriendBookContext context;

        public YardSaleController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
            context = ctx;
        }

        public IActionResult Index()
        {
            User CurrentUser = ActiveUser.Instance.User;

            YardSaleHomeViewModel model = new YardSaleHomeViewModel(context);
            model.UserStyle = context.Style.Where(s => s.UserId == CurrentUser.UserId).SingleOrDefault();
            model.YardSaleItems = context.YardSaleItem.Where(y => y.PostingUserId == CurrentUser.UserId).OrderBy(d => d.DatePosted).ToList();
            model.YardSaleItems.ForEach(i => i.PostingUser = context.User.Where(u => u.UserId == i.PostingUserId).SingleOrDefault());

            return View(model);
        }

        public IActionResult NewItem()
        {
            YardSaleNewItemViewModel model = new YardSaleNewItemViewModel(context);
            model.UserStyle = context.Style.Where(u => u.UserId == ActiveUser.Instance.User.UserId).SingleOrDefault();

            return View(model);
        }

        public async Task<IActionResult> AddNewItem(YardSaleNewItemViewModel model)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            YardSaleItem NewItem = new YardSaleItem
            {
                DatePosted = DateTime.Now,
                ItemDescription = model.NewItem.ItemDescription,
                ItemName = model.NewItem.ItemName,
                ItemPrice = model.NewItem.ItemPrice,
                PostingUserId = ActiveUser.Instance.User.UserId,
                ItemImage1 = model.NewItemImages[0].FileName,
                Category = model.NewItem.Category,
            };

            NewItem.ItemImage2 = (model.NewItemImages.Count == 2) ? model.NewItemImages[1].FileName : null;
            NewItem.ItemImage3 = (model.NewItemImages.Count == 3) ? model.NewItemImages[2].FileName : null;
            NewItem.ItemImage4 = (model.NewItemImages.Count == 4) ? model.NewItemImages[3].FileName : null;

            context.YardSaleItem.Add(NewItem);
            foreach (var image in model.NewItemImages)
            {
                if (image != null && image.ContentType.Contains("image"))
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, image.FileName), FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index", "YardSale");
        }
    }
}
