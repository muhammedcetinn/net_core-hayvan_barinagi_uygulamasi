using HayvanBarinagi.Models;
using HayvanBarinagi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace HayvanBarinagi.Controllers
{
    public class AnimalController : Controller
    {
        //Dependency Injection:
        public readonly IAnimalRepository _animalRepository;
        //Foreign Key olarak verdigim sinifin kullanilmasi icin
        public readonly IAnimalTypeRepository _animalTypeRepository;
        public readonly IWebHostEnvironment _webHostEnvironment; //Image eklemek için bunu eklememiz lazım

        public AnimalController(IAnimalRepository animalRepository, IAnimalTypeRepository animalTypeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _animalRepository = animalRepository;
            _animalTypeRepository = animalTypeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Animal> animalList = _animalRepository.GetAll(includeProps: "AnimalType").ToList();
            return View(animalList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult AddOrUpdateAnimal(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;

            if (id == null || id == 0)
            {
                //Ekleme isleminde burasi calisiyor
                return View();
            }
            else
            {
                //Güncelleme isleminde burasi calisiyor
                Animal? animalV = _animalRepository.Get(u => u.Id == id);
                if (animalV == null) { return NotFound(); }
                return View(animalV);
            }

        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult AddOrUpdateAnimal(Animal animal, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string animalPath = Path.Combine(wwwRootPath, @"img");
                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(animalPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    animal.ImageURL = @"\img\" + file.FileName;
                }
                if (animal.Id == 0)
                {
                    _animalRepository.Insert(animal);
                    TempData["Success"] = "New animal added successfuly!";
                }
                else
                {
                    _animalRepository.Update(animal);
                    TempData["Success"] = "New animal updated successfuly!";
                }
                _animalRepository.Save();
                return RedirectToAction("Index", "Animal");
            }
            return View();
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult DeleteAnimal(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }
            Animal? animalV = _animalRepository.Get(u => u.Id == id);
            if (animalV == null) { return NotFound(); }
            return View(animalV);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost, ActionName("DeleteAnimal")]
        public IActionResult DeleteAnimalPost(int? id)
        {
            Animal? animal = _animalRepository.Get(u => u.Id == id);
            if (animal == null) { return NotFound(); }
            _animalRepository.Delete(animal);
            _animalRepository.Save();
            TempData["Success"] = "Animal deleted successfuly!";
            return RedirectToAction("Index", "Animal");
        }
        /*

        [Authorize(Roles = UserRoles.Role_Customer)]
        public IActionResult Requests(int? id)
        {
            List<TakeOwnership> takeOwnershipList = _takeOwnershipRepository.GetAll(includeProps: "Animal").ToList();
            return View(takeOwnershipList);
        }*/
        /**********************************************************************************/
        /**********************************************************************************/
        /**********************************************************************************/
        /**********************************************************************************/
        /**********************************************************************************/
        /**********************************************************************************/

        [Authorize(Roles = UserRoles.Role_Customer)]
        public IActionResult CreateRequest(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            Animal? animalV = _animalRepository.Get(u => u.Id == id);
            if (animalV == null) { return NotFound(); }
            ViewData["UserName"] = User.Identity.Name;
            return View(animalV);

        }
        [Authorize(Roles = UserRoles.Role_Customer)]
        [HttpPost]
        public IActionResult CreateRequest(Animal animal)
        {


            if (ModelState.IsValid)
            {
                animal.isRequest = true;
                _animalRepository.Update(animal);
                TempData["Success"] = "Request successfuly!";
                _animalRepository.Save();
                return RedirectToAction("RequestsOfUser", "Animal");
            }
            return View();
        }

        /*********************************************************************************************/
        /*********************************************************************************************/
        /*********************************************************************************************/
        /*********************************************************************************************/
        [Authorize(Roles = UserRoles.Role_Customer)]
        public IActionResult RequestsOfUser()
        {
            string user = User.Identity.Name;
            List<Animal> userRequestList = _animalRepository.GetReq(u => u.Recipient == user, includeProps: "AnimalType").ToList();
            return View(userRequestList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Requests()
        {
            List<Animal> requestsList = _animalRepository.GetReq(u => u.isRequest == true, includeProps: "AnimalType").ToList();
            return View(requestsList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult AcceptReq(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            Animal? animalV = _animalRepository.Get(u => u.Id == id);
            if (animalV == null) { return NotFound(); }
            return View(animalV);

        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult AcceptReq(Animal animal)
        {
            var error = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors);

            if (ModelState.IsValid)
            {
                animal.Status = true;
                TempData["Success"] = "Request Accepted!";
                _animalRepository.Update(animal);
                _animalRepository.Save();
                return RedirectToAction("Requests", "Animal");
            }
            return View();
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult DeclineReq(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            Animal? animalV = _animalRepository.Get(u => u.Id == id);
            if (animalV == null) { return NotFound(); }
            return View(animalV);

        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult DeclineReq(Animal animal)
        {
            var error = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors);

            if (ModelState.IsValid)
            {
                animal.isRequest = false;
                TempData["Success"] = "Request declined!";
                _animalRepository.Update(animal);
                _animalRepository.Save();
                return RedirectToAction("Requests", "Animal");
            }
            return View();
        }
    }
}
