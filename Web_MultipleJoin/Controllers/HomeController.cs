using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MultipleJoin.Models;
using Web_MultipleJoin.ViewModel;

namespace Web_MultipleJoin.Controllers
{
    public class HomeController : Controller
    {
        EMPcodeEntities entities = new EMPcodeEntities();
        public ActionResult Index()
        {
            var data = (from a in entities.Students
                        join b in entities.Subjects
                        on a.Id equals b.studentid
                        select new TotalDetail
                        {
                            student = a,
                            subject = b

                        }
                        ).ToList();
                      
            return View(data);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TotalDetail totalDetail) 
        {
            if (ModelState.IsValid)
            {
                entities.Students.Add(totalDetail.student);
                entities.Subjects.Add(totalDetail.subject);
                entities.SaveChanges();
                return RedirectToAction("Index");   

            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = (from c in entities.Students
                        join d in entities.Subjects
                        on c.Id equals d.studentid
                        select new TotalDetail
                        {
                            student = c,
                            subject = d
                        }).Where(x => x.student.Id == id).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(TotalDetail detail)
        {
          
            var data = entities.Students.Where(x => x.Id == detail.student.Id).FirstOrDefault();
            data.Firstname = detail.student.Firstname;
            data.Lastname = detail.student.Lastname;

            var datas = entities.Subjects.Where(x => x.studentid == data.Id).FirstOrDefault();
            datas.subjectname = detail.subject.subjectname;

            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data1 = entities.Students.Where(x => x.Id == id).FirstOrDefault();
            entities.Entry(data1).State = System.Data.Entity.EntityState.Deleted;

            var data2 = entities.Subjects.Where(x => x.studentid == data1.Id).FirstOrDefault();
            entities.Entry(data2).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var data=(from e in entities.Students
                      join f in entities.Subjects
                      on e.Id equals f.studentid
                      select new TotalDetail
                      {
                          student = e,
                          subject = f
                      }).Where(x => x.student.Id == id).FirstOrDefault();
            return View(data);
        }
                   
    }
}