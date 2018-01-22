using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Models;
using PagedList;

namespace Demo.Controllers
{
    public class UserController : Controller
    {
        private DemoEntities db = new DemoEntities();
        /// <summary>
        /// 随机生成的验证码
        /// </summary>
        private static string printCode = String.Empty;

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

        private List<userInfo> getUserInfos(int pageIndex, int pageSize, ref int totalCount)
        {
            var users = (from user in db.userInfo orderby user.id descending select user).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            totalCount = db.userInfo.Count();
            return users.ToList();
        }

        //
        // GET: /User/ShowUserInfo
        public ActionResult ShowUserInfo(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = 3;
            int totalCount = 0;
            var users = getUserInfos(pageIndex, pageSize,ref totalCount);
            var usersAsPageList = new StaticPagedList<userInfo>(users, pageIndex, pageSize, totalCount);
            return View(usersAsPageList);
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