using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Models;

namespace Demo.Controllers
{
    public class UserController : Controller
    {
        private DemoEntities db = new DemoEntities();
        private static string printCode;

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAuthCode()
        {
            VerifyCode vc=new VerifyCode();
            byte[] code = vc.GetVerifyCode();
            printCode = vc.chkCode;
            return File(code, @"image/Gif");
        }

        [HttpPost]
        public ActionResult CheckCode(string code)
        {
            bool result = code.ToLower().Trim() == printCode.ToLower().Trim();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckLogin(string name,string pwd)
        {
            bool result = false;
            if (ModelState.IsValid) { 
                using(var data=new DemoEntities()){
                    var query = from userinfo in data.userInfo
                                where userinfo.name == name && userinfo.pwd == pwd
                                select userinfo;
                    if (query.ToList().Count != 0)
                    {
                        result = true;
                    }
                }
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /User/ShowUserInfo
        public ActionResult ShowUserInfo()
        {
            return View(db.userInfo.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            userInfo userinfo = db.userInfo.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View("Create");
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(userInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.userInfo.Add(userinfo);
                db.SaveChanges();
                return RedirectToAction("ShowUserInfo");
            }

            return View(userinfo);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            userInfo userinfo = db.userInfo.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(userInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowUserInfo");
            }
            return View(userinfo);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            userInfo userinfo = db.userInfo.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            userInfo userinfo = db.userInfo.Find(id);
            db.userInfo.Remove(userinfo);
            db.SaveChanges();
            return RedirectToAction("ShowUserInfo");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}