using Lab1DBwithASP.Models;
using Lab1DBwithASP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing;

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

        public IActionResult PaymentsIndex(int year, int month, bool apartsMonth, int apartStart, int apartEnd, string sort)
        {
            ApartmentDAO apartmentDAO = new();
            List<ApartmentModel>? models = new();
            if (apartsMonth)
                models = apartmentDAO.GetApartmentsMonth(year, month, apartStart, apartEnd);
            else
                models = apartmentDAO.GetApartmentYear(year, apartStart);

            IEnumerable<ApartmentModel>? query = null;
            switch (sort)
            {
                case "ApartAsc":
                    query = models.OrderBy(element => element.Id);
                    break;
                case "ApartDesc":
                    query = models.OrderByDescending(element => element.Id);
                    break;
                case "DateAsc":
                    query = models.OrderBy(element => element.Year)
                        .OrderBy(element => element.MonthId);
                    break;
                case "DateDesc":
                    query = models.OrderByDescending(element => element.Year)
                        .OrderByDescending(element => element.MonthId);
                    break;
                case "PaymentAsc":
                    query = models.OrderBy(element => element.Paid);
                    break;
                case "PaymentDesc":
                    query = models.OrderByDescending(element => element.Paid);
                    break;
            }
            if(query!= null)
                return View(query);
            return View(models);
        }

        public IActionResult ChargesIndex(int year, int month, bool apartsMonth, int apartStart, int apartEnd, string sort)
        {
            ApartmentDAO apartmentDAO = new();
            List<ApartmentModel>? models = new();
            if (apartsMonth)
                models = apartmentDAO.GetApartmentsMonth(year, month, apartStart, apartEnd);
            else
                models = apartmentDAO.GetApartmentYear(year, apartStart);

            IEnumerable<ApartmentModel>? query = null;
            switch (sort)
            {
                case "ApartAsc":
                    query = models.OrderBy(element => element.Id);
                    break;
                case "ApartDesc":
                    query = models.OrderByDescending(element => element.Id);
                    break;
                case "DateAsc":
                    query = models.OrderBy(element => element.Year)
                        .OrderBy(element => element.MonthId);
                    break;
                case "DateDesc":
                    query = models.OrderByDescending(element => element.Year)
                        .OrderByDescending(element => element.MonthId);
                    break;
                case "ChargesAsc":
                    query = models.OrderBy(element => element.Additional);
                    break;
                case "ChargesDesc":
                    query = models.OrderByDescending(element => element.Additional);
                    break;
            }
            if (query != null)
                return View(query);
            return View(models);
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
            ApartmentDAO apartmentDAO = new();
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

        public IActionResult UpdateForm(int id, int year)
        {
            ApartmentModel apartment = new()
            {
                Id = (UInt32)id,
                Year = (UInt32)year
            };
            return View(apartment);
        }

        public IActionResult Update(ApartmentModel apartmentModel)
        {
            ApartmentDAO apartmentDAO = new();
            apartmentDAO.Update(apartmentModel);
            return View("Index", apartmentDAO.GetApartments((int)DateTime.Now.Year));
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
            return View("Index", apartmentDAO.GetApartments(DateTime.Now.Year));
        }

        public ActionResult DeleteEntry(int id, UInt32 year)
        {
            ApartmentDAO apartmentDAO = new();
            apartmentDAO.DeleteEntry(id);
            return View("Index", apartmentDAO.GetApartments((int)year));
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
