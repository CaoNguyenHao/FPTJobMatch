using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPTJobMatch.Data;
using FPTJobMatch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPTJobMatch.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public JobController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {

            var approvedJobs = _dbContext.Jobs.Where(j => j.Status == JobStatus.Approved).ToList();
            return View(approvedJobs);
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
            return View(job);
        }
    }
}

