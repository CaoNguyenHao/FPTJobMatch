using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FPTJobMatch.Data;
using FPTJobMatch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FPTJobMatch.Controllers
{
    public class ManageJobList : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ManageJobList(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /<controller>/

        public IActionResult Index()
        {
            var jobs = _dbContext.Jobs.ToList();
            return View(jobs);
        }

        [Authorize(Roles = "Company")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Company")]
        [HttpPost]
        public IActionResult Create(Job job)
        {
            job.Status = JobStatus.Pending; // Đánh dấu công việc là chờ
            _dbContext.Jobs.Add(job);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "ManageJobList"); // Chuyển hướng sau khi tạo công việc
        }

        [Authorize(Roles = "Company")]
        public IActionResult Edit(int id)
        {
            var job = _dbContext.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        [Authorize(Roles = "Company")]
        [HttpPost]
        public IActionResult Edit(int id, Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                job.Status = JobStatus.Pending;
                _dbContext.Update(job);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", "ManageJobList");
        }

        [Authorize(Roles = "Company, Admin")]
        public IActionResult Delete(int id)
        {
            var job = _dbContext.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        [Authorize(Roles = "Company, Admin")]
        [HttpPost]
        public IActionResult Delete(int id, Job job)
        {
            if (job == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _dbContext.Jobs.Remove(job);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult ApproveJob(int jobId)
        {
            var job = _dbContext.Jobs.FirstOrDefault(j => j.Id == jobId);
            if (job != null)
            {
                job.Status = JobStatus.Approved; // Đánh dấu công việc là đã duyệt
                _dbContext.SaveChanges();
                
            }
            return RedirectToAction("Index", "ManageJobList"); // Ví dụ: chuyển hướng đến trang 
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult RejectJob(int jobId)
        {
            var job = _dbContext.Jobs.FirstOrDefault(j => j.Id == jobId);
            if (job != null)
            {
                job.Status = JobStatus.Rejected; // 
                _dbContext.SaveChanges();
               
            }
            return RedirectToAction("Index", "ManageJobList");
        }
    }
}

