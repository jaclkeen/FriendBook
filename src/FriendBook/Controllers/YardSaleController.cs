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

        /**
        * Purpose: Returns the main view for all yardsale items
        * Arguments:
        *      None
        * Return:
        *      returns the view for the yardsale index(home) page
        */
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

        /**
        * Purpose: Returns the main view for the form to create a new item for sale
        * Arguments:
        *      None
        * Return:
        *      returns the view to the form in order to create a new product
        */
        public IActionResult NewItem()
        {
            YardSaleNewItemViewModel model = new YardSaleNewItemViewModel(context);
            model.UserStyle = context.Style.Where(u => u.UserId == ActiveUser.Instance.User.UserId).SingleOrDefault();

            return View(model);
        }

        /**
        * Purpose: Post method that takes a new item and saves it to the DB
        * Arguments:
        *      model - contains all the neccessary properties that are needed to put a new item up for sale
        * Return:
        *      redirects to the main view of yardsale products "YardSale/Index"
        */
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

        /**
        * Purpose: Returns the view for that contains all of a specific user's products that are for sale
        * Arguments:
        *      int id - the UserId of which products are being requested
        * Return:
        *      returns the view that contains that specific user's products that are up for sale
        */
        public IActionResult ForSale([FromRoute] int id)
        {
            ProfileForSaleViewModel model = new ProfileForSaleViewModel(context, id);
            model.YardSaleItems = context.YardSaleItem.Where(ysi => ysi.PostingUserId == id).ToList();
            model.YardSaleItems.ForEach(ysi => ysi.ItemComments = context.Comment.Where(c => c.YardSaleItemId == ysi.YardSaleItemId).ToList());

            return View(model);
        }

        /**
        * Purpose: Deletes a specific yardsale item
        * Arguments:
        *      ItemId - the id of the item being deleted
        * Return:
        *      None
        */
        [HttpPost]
        public void DeleteYardSaleItem([FromBody] int ItemId)
        {
            List<Comment> ItemComments = context.Comment.Where(c => c.YardSaleItemId == ItemId).ToList();

            ItemComments.ForEach(ic => context.Comment.Remove(ic));
            YardSaleItem item = context.YardSaleItem.Where(ysi => ysi.YardSaleItemId == ItemId).SingleOrDefault();

            context.YardSaleItem.Remove(item);
            context.SaveChanges();
        }

        /**
        * Purpose: To post a new comment onto a yardsale item and to create a notification when completed
        * Arguments:
        *      comment - Contains all the neccessary properties needed to add a new comment onto a yardsale item
        * Return:
        *      comment.commentId - the comment id of the comment that was just saved
        */
        [HttpPost]
        public int CommentOnYardSaleItem([FromBody] Comment comment)
        {
            YardSaleItem item = context.YardSaleItem.Where(ysi => ysi.YardSaleItemId == comment.YardSaleItemId).SingleOrDefault();
            User RecievingUser = context.User.Where(u => u.UserId == item.PostingUserId).SingleOrDefault();
            User SendingUser = ActiveUser.Instance.User;

            comment.TimePosted = DateTime.Now;
            comment.UserId = SendingUser.UserId;

            context.Comment.Add(comment);

            if (RecievingUser.UserId != SendingUser.UserId)
            {
                Notification NewNotification = new Notification
                {
                    NotificationText = $"{SendingUser.FirstName} {SendingUser.LastName}, commented on your {item.ItemName} that is up for sale!",
                    NotificationType = "Sale",
                    NotificatonDate = DateTime.Now,
                    RecievingUserId = RecievingUser.UserId,
                    YardSaleItemId = item.YardSaleItemId,
                    Seen = false,
                    SenderUserId = SendingUser.UserId
                };
                context.Notification.Add(NewNotification);
            }

            context.SaveChanges();

            return comment.CommentId;
        }

        /**
        * Purpose: Gets all of the comments on a specific yardsale item
        * Arguments:
        *      YardSaleCommentId - The Id of the yardsale item of which comments are being requested
        * Return:
        *      returns a list of comments found for that yardsale item
        */
        [HttpPost]
        public List<Comment> GetYardSaleItemComments([FromBody] int YardSaleCommentId)
        {
            List<Comment> comments = context.Comment.Where(c => c.YardSaleItemId == YardSaleCommentId).ToList();
            comments.ForEach(c => c.User = context.User.Where(u => u.UserId == c.UserId).SingleOrDefault());

            return comments;
        }

        /**
        * Purpose: Deletes a single comment on a yardsale item
        * Arguments:
        *      CommentId - The id of the comment being deleted
        * Return:
        *      None
        */
        [HttpPost]
        public void RemoveCommentOnYardSaleItem([FromBody] int CommentId)
        {
            Comment c = context.Comment.Where(co => co.CommentId == CommentId).SingleOrDefault();
            context.Comment.Remove(c);
            context.SaveChanges();
        }

        /**
        * Purpose: Returns all the yardsale items that contain the certain filters
        * Arguments:
        *      model - contains the filter values gathered from user input
        * Return:
        *      returns all the yardsale items that have these filter values
        */
        [HttpPost]
        public List<YardSaleItem> FilteredYardSaleItems([FromBody] YardSaleHomeViewModel model)
        {
            List<YardSaleItem> FilteredItems = new List<YardSaleItem> { };

            if(model.ItemCategoryFilter == "0" && model.ItemNameFilter == "")
            {
                FilteredItems = context.YardSaleItem.OrderBy(d => d.DatePosted).ToList();
            }

            else if(model.ItemNameFilter == "" && model.ItemCategoryFilter != "0")
            {
                FilteredItems = context.YardSaleItem.Where(ysi => ysi.Category == model.ItemCategoryFilter).OrderBy(d => d.DatePosted).ToList();
            }

            else if(model.ItemCategoryFilter == "0" && model.ItemNameFilter != "")
            {
                FilteredItems = context.YardSaleItem.Where(ysi => ysi.ItemName.ToLower() == model.ItemNameFilter.ToLower()).OrderBy(d => d.DatePosted).ToList();
            }

            else
            {
                FilteredItems = context.YardSaleItem.Where(ysi => ysi.Category == model.ItemCategoryFilter && ysi.ItemName.ToLower() == model.ItemNameFilter.ToLower()).OrderBy(d => d.DatePosted).ToList();
            }

            FilteredItems.ForEach(i => i.PostingUser = context.User.Where(u => u.UserId == i.PostingUserId).SingleOrDefault());
            FilteredItems.ForEach(d => d.DatePosted = d.DatePosted.Date);
            FilteredItems = FilteredItems.OrderByDescending(d => d.DatePosted).ToList();

            return FilteredItems;
        }
    }
}
