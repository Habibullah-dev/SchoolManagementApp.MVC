using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementApp.MVC.Data;
using SchoolManagementApp.MVC.Models;
namespace SchoolManagementApp.MVC.Controllers
{
    public class LessonsController : Controller
    {
        private readonly SchoolManagementDbContext _context;
        private readonly INotyfService _notyfService;

        public LessonsController(SchoolManagementDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var schoolManagementDbContext = _context.Lessons.Include(l => l.Course).Include(l => l.Lecturer);
            return View(await schoolManagementDbContext.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .Include(l => l.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lessons/Create
     // GET: Classes/Create
        public IActionResult Create()
        {
            CreateSelectLists();
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LecturerId,CourseId,Time")] Lesson lessonModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lessonModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateSelectLists();
            return View(lessonModel);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", lesson.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "Id", lesson.LecturerId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,CourseId,Time")] Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", lesson.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "Id", lesson.LecturerId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .Include(l => l.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'SchoolManagementDbContext.Lessons'  is null.");
            }
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

         public async Task<ActionResult> ManageEnrollments(int lessonId)
        {
            var lesson = await _context.Lessons
                .Include(q => q.Course)
                .Include(q => q.Lecturer)
                .Include(q => q.Enrollments)
                    .ThenInclude(q => q.Student)
                .FirstOrDefaultAsync(m => m.Id == lessonId);
            
            var students = await _context.Students.ToListAsync();

            var model = new LessonEnrollmentViewModel();
            model.Lesson = new LessonViewModel
            {
                Id = lesson.Id,
                CourseName = $"{lesson.Course.Code} - {lesson.Course.Name}",
                LecturerName = $"{lesson.Lecturer.FirstName} {lesson.Lecturer.LastName}",
                Time = lesson.Time.ToString()
            };

            foreach (var stu in students)
            {
                model.Students.Add(new StudentEnrollmentViewModel
                {
                    Id = stu.Id,
                    FirstName = stu.FirstName,
                    LastName = stu.LastName,
                    IsEnrolled = (lesson?.Enrollments?.Any(q => q.StudentId == stu.Id))
                        .GetValueOrDefault()
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnrollStudent(int lessonId, int studentId, bool shouldEnroll)
        {
            var enrollment = new Enrollment();
            if(shouldEnroll == true)
            {
                enrollment.LessonId = lessonId;
                enrollment.StudentId = studentId;
                await _context.AddAsync(enrollment);
                _notyfService.Success($"Student Enrolled Successfully");
            }
            else
            {
                enrollment = await _context.Enrollments.FirstOrDefaultAsync(
                    q => q.LessonId == lessonId && q.StudentId == studentId);

                if(enrollment != null){
                    _context.Remove(enrollment);
                    _notyfService.Warning($"Student Unenrolled Successfully");
                }    
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageEnrollments), 
            new { lessonId = lessonId});
        }

        private void CreateSelectLists()
        {
            var courses = _context.Courses.Select(q => new 
            {
                CourseName = $"{q.Code} - {q.Name} ({q.Credits} Credits)",
                q.Id
            });
            
            ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName");
            var lecturers = _context.Lecturers.Select(q => new 
            {
                Fullname = $"{q.FirstName} {q.LastName}",
                q.Id
            });
            ViewData["LecturerId"] = new SelectList(lecturers, "Id", "Fullname");
        }

        private bool LessonExists(int id)
        {
          return (_context.Lessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
