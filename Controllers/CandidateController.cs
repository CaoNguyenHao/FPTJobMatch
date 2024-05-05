using System;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FPTJobMatch.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Matching;

namespace FPTJobMatch.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CandidateController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Applicant, Company")]
        public IActionResult Index()
        {
            var candidates = _dbContext.Candidates.ToList();
            return View(candidates);
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Candidate candidate, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imagePath = Path.Combine(wwwRootPath, @"\img\Candidates");
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    candidate.ImageUrl = @"\img\Candidates" + fileName;
                }
                candidate.Status = CandidateStatus.Pending;
                _dbContext.Candidates.Add(candidate);
                _dbContext.SaveChanges();

            }
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            var candidate = _dbContext.Candidates.Find(id);
            if (candidate == null)
            {
                return NotFound();
            }
            _dbContext.Candidates.Remove(candidate);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id, Candidate candidate)
        {
            if (candidate == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _dbContext.Candidates.Remove(candidate);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Company")]
        public IActionResult ApproveCandidate(int candidateId)
        {
            var candidate = _dbContext.Candidates.FirstOrDefault(c => c.Id == candidateId);
            if (candidate != null)
            {
                candidate.Status = CandidateStatus.Approved; // Đánh dấu công việc là đã duyệt
                _dbContext.SaveChanges();
                // Có thể chuyển hướng hoặc hiển thị thông báo thành công ở đây
            }
            return RedirectToAction("Index", "Candidate"); // Ví dụ: chuyển hướng đến trang dashboard của admin
        }

        [Authorize(Roles = "Company")]
        public IActionResult RejectCandidate(int candidateId)
        {
            var candidate = _dbContext.Candidates.FirstOrDefault(c => c.Id == candidateId);
            if (candidate != null)
            {
                candidate.Status = CandidateStatus.Rejected; // Đánh dấu công việc là đã duyệt
                _dbContext.SaveChanges();
                // Có thể chuyển hướng hoặc hiển thị thông báo thành công ở đây
            }
            return RedirectToAction("Index", "Candidate");
        }
    }
}

