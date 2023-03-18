using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;
using MyApp.ViewModels;

namespace MyApp.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ManageViewModel
            {
                Email = user.Email,
                UpcomingAppointments = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Where(a => a.PatientId == user.Id && a.AppointmentDateTime > DateTime.Now)
                    .OrderBy(a => a.AppointmentDateTime)
                    .Select(a => new AppointmentViewModel
                    {
                        AppointmentId = a.AppointmentId,
                        AppointmentDateTime = a.AppointmentDateTime,
                        DoctorName = a.Doctor.FullName,
                        ReasonForVisit = a.ReasonForVisit
                    })
                    .ToListAsync(),
                AvailableDoctors = await _context.Doctors
                    .Select(d => new { Value = d.DoctorId, Text = d.FullName })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestAppointment(ManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var appointment = new Appointment
                {
                    AppointmentDateTime = model.RequestedAppointment.AppointmentDateTime,
                    DoctorId = model.RequestedAppointment.DoctorId,
                    PatientId = user.Id,
                    ReasonForVisit = model.RequestedAppointment.ReasonForVisit
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.UpcomingAppointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == model.RequestedAppointment.PatientId && a.AppointmentDateTime > DateTime.Now)
                .OrderBy(a => a.AppointmentDateTime)
                .Select(a => new AppointmentViewModel
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDateTime = a.AppointmentDateTime,
                    DoctorName = a.Doctor.FullName,
                    ReasonForVisit = a.ReasonForVisit
                })
                .ToListAsync();

            model.AvailableDoctors = await _context.Doctors
                .Select(d => new { Value = d.DoctorId, Text = d.FullName })
                .ToListAsync();

            return View("Index", model);
        }

        public async Task<IActionResult> Appointments()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == user.Id)
                .OrderBy(a => a.AppointmentDateTime)
                .Select(a => new AppointmentViewModel
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDateTime = a.AppointmentDateTime,
                    DoctorName = a.Doctor.FullName,
                    ReasonForVisit = a.ReasonForVisit
                })
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new EditProfileViewModel
            {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            City = user.City,
            State = user.State,
            ZipCode = user.ZipCode
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.ZipCode = model.ZipCode;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> CancelAppointment(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);

        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
