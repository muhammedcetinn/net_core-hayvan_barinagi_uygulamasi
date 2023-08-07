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
        private readonly ICustomersRepository _customersRepository;
        private readonly IGiveAnimalRepository _giveAnimalRepository;

        public AnimalController(IAnimalRepository animalRepository, IAnimalTypeRepository animalTypeRepository, IWebHostEnvironment webHostEnvironment, ICustomersRepository customers, IGiveAnimalRepository giveAnimalRepository)
        {
            _animalRepository = animalRepository;
            _animalTypeRepository = animalTypeRepository;
            _webHostEnvironment = webHostEnvironment;
            _customersRepository = customers;
            _giveAnimalRepository = giveAnimalRepository;
        }

        public IActionResult Index()
        {
            List<Animal> animalList = _animalRepository.GetAll(includeProps: "AnimalType").ToList();
            return View(animalList);
        }

        #region GiveAnimal
        [Authorize(Roles = UserRoles.Role_Customer)]
        public IActionResult GiveAnimal()
        {
            string username = User.Identity.Name;
            List<GiveAnimal> animalList = _giveAnimalRepository.GetReq(u => u.Recipient == username, includeProps: "AnimalType").ToList();
            return View(animalList);
        }
        [Authorize(Roles = UserRoles.Role_Customer)]
        public IActionResult AddGAnimal()
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.NameEN,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            string username = User.Identity.Name;
            ViewData["UserName"] = username;
            Customers? customersV = _customersRepository.Get(u => u.UserName == username);
            ViewData["About"] = customersV.About;
            return View();

        }
        [Authorize(Roles = UserRoles.Role_Customer)]
        [HttpPost]
        public IActionResult AddGAnimal(GiveAnimal giveAnimal, IFormFile? file)
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
                    giveAnimal.ImageURL = @"\img\" + file.FileName;
                }
                giveAnimal.isRequest = true;
                _giveAnimalRepository.Insert(giveAnimal);
                TempData["Success"] = "New animal added successfuly!";
                _animalRepository.Save();
                return RedirectToAction("GiveAnimal", "Animal");
            }
            return View();
        }
        #endregion

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult AddOrUpdateAnimal(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.NameEN,
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
                    Text = k.NameEN,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            Animal? animalV = _animalRepository.Get(u => u.Id == id);
            if (animalV == null) { return NotFound(); }
            string username = User.Identity.Name;
            ViewData["UserName"] = username;
            Customers? customersV = _customersRepository.Get(u=> u.UserName==username);
            ViewData["About"] = customersV.About;
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
                    Text = k.NameEN,
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
                    Text = k.NameEN,
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


        #region Give Animal Admin
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult GiveAnimalRequest()
        {
            List<GiveAnimal> requestsList = _giveAnimalRepository.GetReq(u => u.isRequest == true, includeProps: "AnimalType").ToList();
            return View(requestsList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult GiveAnimalAcceptReq(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.NameEN,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            GiveAnimal? giveAnimalV = _giveAnimalRepository.Get(u => u.Id == id);
            if (giveAnimalV == null) { return NotFound(); }
            return View(giveAnimalV);

        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult GiveAnimalAcceptReq(GiveAnimal giveAnimal)
        {
            var error = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors);

            if (ModelState.IsValid)
            {
                giveAnimal.Status = true;
                Animal animal = new Animal();
                string name= giveAnimal.Name;
                string age = giveAnimal.Age;
                string features = giveAnimal.Features;
                string imageURL = giveAnimal.ImageURL;
                int animalId = giveAnimal.AnimalTypeId;
                animal.Name = name;
                animal.Age = age;
                animal.Features = features;
                animal.ImageURL = imageURL;
                animal.AnimalTypeId = animalId;
                animal.Recipient = "null";
                animal.RecipientAbout = "null";
                _animalRepository.Insert(animal);
                _animalRepository.Save();
                TempData["Success"] = "Request Accepted!";
                _giveAnimalRepository.Update(giveAnimal);
                _giveAnimalRepository.Save();
                return RedirectToAction("GiveAnimalRequest", "Animal");
            }
            return View();
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult GiveAnimalDeclineReq(int? id)
        {
            IEnumerable<SelectListItem> AnimalTypeList = _animalTypeRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.NameEN,
                    Value = k.Id.ToString()
                });
            ViewBag.AnimalTypeList = AnimalTypeList;
            GiveAnimal? giveAnimalV = _giveAnimalRepository.Get(u => u.Id == id);
            if (giveAnimalV == null) { return NotFound(); }
            return View(giveAnimalV);

        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost]
        public IActionResult GiveAnimalDeclineReq(GiveAnimal giveAnimal)
        {
            var error = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors);

            if (ModelState.IsValid)
            {
                giveAnimal.isRequest = false;
                TempData["Success"] = "Request declined!";
                _giveAnimalRepository.Update(giveAnimal);
                _giveAnimalRepository.Save();
                return RedirectToAction("GiveAnimalRequest", "Animal");
            }
            return View();
        }
        #endregion
    }
}
