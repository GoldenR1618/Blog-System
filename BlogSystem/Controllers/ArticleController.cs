using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: Article
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // Get: Article/List
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                // Get article from database
                var articles = database.Articles.Include(a => a.Author).ToList();

                return View(articles);
            }
        }

        //
        // GET: Article/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // Get the article from database

                var article = database.Articles.Where(a => a.Id == id).Include(a => a.Author).First();

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        //
        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: Article/Create
        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                // Insert article in database
                using (var database = new BlogDbContext())
                {
                    // Get author id
                    var authorId = database.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;

                    // Set articlea author
                    article.AuthorId = authorId;

                    // Save article in DB
                    database.Articles.Add(article);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(article);
        }
    }
}