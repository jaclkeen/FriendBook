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
            YardSaleHomeViewModel model = new YardSaleHomeViewModel(context);
            User CurrentUser = ActiveUser.Instance.User;

            //GET ALL FRIEND ITEMS WHERE THE CURRENT USER IS THE SENDING USER IN THE FRIEND REQUEST RELATIONSHIP
            //  AND THE RELATIONSHIP STATUS IS 1 (ARE FRIENDS)
            List<YardSaleItem> FriendItems1 = (from r in context.Relationship
                                               join ysi in context.YardSaleItem on r.ReciverUserId equals ysi.PostingUserId
                                               where r.SenderUserId == CurrentUser.UserId && r.Status == 1
                                               select ysi).ToList();

            //GET ALL FRIEND ITEMS WHERE THE CURRENT USER IS THE RECIEVING USER IN THE FRIEND REQUEST RELATIONSHIP
            //  AND THE RELATIONSHIP STATUS IS 1 (ARE FRIENDS)
            List<YardSaleItem> FriendItems2 = (from r in context.Relationship
                                               join ysi in context.YardSaleItem on r.SenderUserId equals ysi.PostingUserId
                                               where r.ReciverUserId == CurrentUser.UserId && r.Status == 1
                                               select ysi).ToList();

            //GET ALL OF THE CURRENT USERS YARDSALE ITEMS
            List<YardSaleItem> UserItems = context.YardSaleItem.Where(ysi => ysi.PostingUserId == CurrentUser.UserId).ToList();

            //ADD ALL OF THE 3 LISTS ABOVE TOGETHER INTO ONE MODEL LIST, AND ORDER BY THE DATE POSTED
            model.YardSaleItems = FriendItems1.Concat(FriendItems2).Concat(UserItems).OrderByDescending(i => i.DatePosted).ToList();

            model.UserStyle = context.Style.Where(s => s.UserId == CurrentUser.UserId).SingleOrDefault();
            model.YardSaleItems.ForEach(i => i.PostingUser = context.User.Where(u => u.UserId == i.PostingUserId).SingleOrDefault());
            model.YardSaleItems.ForEach(i => i.ItemComments = context.Comment.Where(c => c.YardSaleItemId == i.YardSaleItemId).ToList());
            
            foreach(YardSaleItem item in model.YardSaleItems)
            {
                foreach(Comment c in item.ItemComments)
                {
                    c.User = context.User.Where(u => u.UserId == c.UserId).SingleOrDefault();
                }
            }

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

            NewItem.ItemImage2 = (model.NewItemImages.Count > 1) ? model.NewItemImages[1].FileName : null;
            NewItem.ItemImage3 = (model.NewItemImages.Count > 2) ? model.NewItemImages[2].FileName : null;
            NewItem.ItemImage4 = (model.NewItemImages.Count > 3) ? model.NewItemImages[3].FileName : null;

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

        public IActionResult ForSale([FromRoute] int id)
        {
            ProfileForSaleViewModel model = new ProfileForSaleViewModel(context, id);
            model.YardSaleItems = context.YardSaleItem.Where(ysi => ysi.PostingUserId == id).ToList();
            model.YardSaleItems.ForEach(ysi => ysi.ItemComments = context.Comment.Where(c => c.YardSaleItemId == ysi.YardSaleItemId).ToList());


            return View(model);
        }
       
        [HttpPost]
        public int CommentOnYardSaleItem([FromBody] Comment comment)
        {
            comment.TimePosted = DateTime.Now;
            comment.UserId = ActiveUser.Instance.User.UserId;

            context.Comment.Add(comment);
            context.SaveChanges();

            return comment.CommentId;
        }

        [HttpPost]
        public void RemoveCommentOnYardSaleItem([FromBody] int CommentId)
        {
            Comment c = context.Comment.Where(co => co.CommentId == CommentId).SingleOrDefault();
            context.Comment.Remove(c);
            context.SaveChanges();
        }

        [HttpGet]
        public List<YardSaleItem> YardSaleItems()
        {
            User CurrentUser = ActiveUser.Instance.User;

            //GET ALL FRIEND ITEMS WHERE THE CURRENT USER IS THE SENDING USER IN THE FRIEND REQUEST RELATIONSHIP
            //  AND THE RELATIONSHIP STATUS IS 1 (ARE FRIENDS)
            List<YardSaleItem> FriendItems1 = (from r in context.Relationship
                                               join ysi in context.YardSaleItem on r.ReciverUserId equals ysi.PostingUserId
                                               where r.SenderUserId == CurrentUser.UserId && r.Status == 1
                                               select ysi).ToList();

            //GET ALL FRIEND ITEMS WHERE THE CURRENT USER IS THE RECIEVING USER IN THE FRIEND REQUEST RELATIONSHIP
            //  AND THE RELATIONSHIP STATUS IS 1 (ARE FRIENDS)
            List<YardSaleItem> FriendItems2 = (from r in context.Relationship
                                               join ysi in context.YardSaleItem on r.SenderUserId equals ysi.PostingUserId
                                               where r.ReciverUserId == CurrentUser.UserId && r.Status == 1
                                               select ysi).ToList();

            //GET ALL OF THE CURRENT USERS YARDSALE ITEMS
            List<YardSaleItem> UserItems = context.YardSaleItem.Where(ysi => ysi.PostingUserId == CurrentUser.UserId).ToList();

            //ADD ALL OF THE 3 LISTS ABOVE TOGETHER INTO ONE MODEL LIST, AND ORDER BY THE DATE POSTED
            List<YardSaleItem> Items = FriendItems1.Concat(FriendItems2).Concat(UserItems).OrderByDescending(i => i.DatePosted).ToList();
            Items.ForEach(i => i.PostingUser = context.User.Where(u => u.UserId == i.PostingUserId).SingleOrDefault());
            //Items.ForEach(i => i.ItemComments = context.Comment.Where(c => c.YardSaleItemId == i.YardSaleItemId).ToList());

            return Items;
        }
    }
}
