using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;
using FriendBook.ViewModels;

namespace FriendBook.Controllers
{
    public class PostController : Controller
    {
        private FriendBookContext context;

        public PostController(FriendBookContext ctx)
        {
            context = ctx;
        }

        /**
        * Purpose: Method that is used to delete a specific post
        * Arguments:
        *      int id - the id of the particular post being deleted
        * Return:
        *      redirects to the /Home (Index) view
        */
        public IActionResult DeletePost([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            context.Post.Remove(post);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        /**
        * Purpose: Method that is used to add a like to a specific Post
        * Arguments:
        *      int id - the PostId that is being liked
        * Return:
        *      the count of likes from the particular post being liked
        */
        [HttpPost]
        public int AddLike([FromBody] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            post.Likes++;

            context.SaveChanges();

            return post.Likes;
        }

        /**
        * Purpose: Method that is used to add a dislike to a specific Post
        * Arguments:
        *      int id - the PostId that is being disliked
        * Return:
        *      the count of likes from the particular post being disliked
        */
        [HttpPost]
        public int AddDislike([FromBody] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            post.Dislikes++;

            context.SaveChanges();

            return post.Dislikes;
        }

        /**
        * Purpose: Method that is used to add a comment to a specific Post
        * Arguments:
        *      Comment comment - contains all the neccessary properties to create a new comment
        * Return:
        *      None
        */
        [HttpPost]
        public void CreateNewCommentOnPost([FromBody] Comment comment)
        {
            int UserId = ActiveUser.Instance.User.UserId;

            comment.UserId = UserId;
            comment.User = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
            comment.TimePosted = DateTime.Now;

            context.Comment.Add(comment);

            try
            {
                context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        /**
        * Purpose: Changes(Edits) the text of a specific post
        * Arguments:
        *      Post EditedPost - contains the text property needed to update an editedPost
        * Return:
        *      the count of likes from the particular post being liked
        */
        [HttpPost]
        public void EditSpecificPost([FromBody] Post EditedPost)
        {
            Post post = context.Post.Where(p => p.PostId == EditedPost.PostId).SingleOrDefault();
            post.Text = EditedPost.Text;

            context.Post.Update(post);
            context.SaveChanges();
        }

        /**
        * Purpose: Method that is used to get all comments from a particular post
        * Arguments:
        *      int id - the PostId needed to get all comments from that post
        * Return:
        *      the comments of a particular post
        */
        [HttpGet]
        public List<Comment> GetAllCommentsFromSpecificPost([FromRoute] int id)
        {
            List<Comment> comments = context.Comment.Where(p => p.PostId == id).ToList();
            comments.ForEach(c => c.User = context.User.Where(u => u.UserId == c.UserId).SingleOrDefault());

            return comments;
        }

        /**
        * Purpose: Method used to deleted a comment
        * Arguments:
        *      int id - the comment Id that is being deleted
        * Return:
        *      None
        */
        [HttpDelete]
        public void DeleteComment([FromRoute] int id)
        {
            Comment DeletedComment = context.Comment.Where(c => c.CommentId == id).SingleOrDefault();
            context.Comment.Remove(DeletedComment);
            context.SaveChanges();
        }

        /**
        * Purpose: Edit the text of a particular comment
        * Arguments:
        *       Comment comment - the comment that is being edited
        * Return:
        *      None
        */
        [HttpPost]
        public void EditSpecificCommentOnPost([FromBody] Comment comment)
        {
            Comment OldComment = context.Comment.Where(c => c.CommentId == comment.CommentId).SingleOrDefault();
            OldComment.Text = comment.Text;
            context.SaveChanges();
        }
    }
}
