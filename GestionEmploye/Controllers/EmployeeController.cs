using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;

using GestionEmploye.Models.Repositories;
using GestionEmploye.Models;
using GestionEmploye.ViewModels;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;

namespace WebApplication2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            // Récupère la liste des employés depuis la couche de données
            var employees = _employeeRepository.GetAllEmployee();
            return View(employees); // Renvoie une vue avec la liste des employés
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            // Récupère les détails d'un employé spécifique en fonction de son ID
            var employee = _employeeRepository.GetEmployee(id);
            return View(employee); // Renvoie une vue avec les détails de l'employé
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View(); // Affiche un formulaire pour créer un nouvel employé
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    // Logique pour télécharger une image d'employé
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                // Crée un nouvel employé avec les données du formulaire
                Employe newEmployee = new Employe
                {
                    Name = model.Name,
                    Salary = model.Salary,
                    Departement = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee); // Ajoute l'employé à la base de données
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);
            EditViewModel employeeEditViewModel = new EditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Salary = employee.Salary,
                Department = employee.Departement,
            };
            return View(employeeEditViewModel);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                Employe employee = _employeeRepository.GetEmployee(model.Id);
                // Update the employee object with the data in the model object
                employee.Name = model.Name;
                employee.Salary = model.Salary;
                employee.Departement = model.Department;
                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Photo != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                Employe updatedEmployee = _employeeRepository.Update(employee);
                if (updatedEmployee != null)
                    return RedirectToAction("index");
                else
                    return NotFound();
            }
            return View(model);
        }
        [NonAction]
        private string ProcessUploadedFile(EditViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }



        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Supprime un employé de la base de données
                _employeeRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
