using Lab1DBwithASP.Models;
using Lab1DBwithASP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab1DBwithASP.Controllers
{
    public class ApartmentController : Controller
    {
        // GET: AppartmentController
        public IActionResult Index(int year)
        {
            ApartmentDAO apartmentDAO = new();
            return View(apartmentDAO.GetApartments(year));
        }

        // GET: AppartmentController/Details/5
        public ActionResult Details(int id, int year, int month)
        {
            ApartmentDAO apartmentDAO = new();
            ApartmentModel foundModel = apartmentDAO.GetApartmentById(id, year, month);
            return View(foundModel);
        }

        // GET: AppartmentController/Create
        public IActionResult Create(ApartmentModel apartmentModel)
        {
            ApartmentDAO apartmentDAO =new();
            apartmentDAO.Insert(apartmentModel);
            return View("Index", apartmentDAO.GetApartments((int)apartmentModel.Year));
        }

        public ActionResult CreateForm()
        {
            return View();
        }

        // POST: AppartmentController/Create
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction();
            }
            catch
            {
                return View();
            }
        }*/

        // GET: AppartmentController/Edit/5
        public ActionResult Edit(int id, int year, int month)
        {
            ApartmentDAO apartmentDAO = new();
            ApartmentModel foundModel = apartmentDAO.GetApartmentById(id, year, month);
            return View(foundModel);
        }

        public ActionResult ProcessEdit(ApartmentModel model)
        {
            ApartmentDAO apartmentDAO = new();
            apartmentDAO.Edit(model);
            return View("Details", apartmentDAO.GetApartmentById((int)model.Id, (int)model.Year, model.MonthId));
        }

        public IActionResult UpdateForm(int id)
        {
            ApartmentModel apartment = new()
            {
                Id = (UInt32)id
            };
            return View(apartment);
        }

        public IActionResult Update(ApartmentModel apartmentModel)
        {
            ApartmentDAO apartmentDAO = new();
            apartmentDAO.Update(apartmentModel);
            return View("Index", apartmentDAO.GetApartments((int)apartmentModel.Year));
        }

        // POST: AppartmentController/Edit/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

        // GET: AppartmentController/Delete/5
        public ActionResult Delete(int id)
        {
            ApartmentDAO apartmentDAO = new();
            apartmentDAO.Delete(id);
            return RedirectToAction("Index");
        }

        // POST: AppartmentController/Delete/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
