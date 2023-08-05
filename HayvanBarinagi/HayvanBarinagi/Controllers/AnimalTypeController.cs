using HayvanBarinagi.Models;
using HayvanBarinagi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HayvanBarinagi.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class AnimalTypeController : Controller
    {
        //Dependency Injection
        private readonly IAnimalTypeRepository _animalTypeRepository;
        public AnimalTypeController(IAnimalTypeRepository animalTypeRepository)
        {
            _animalTypeRepository = animalTypeRepository;
        }

        public IActionResult Index()
        {
            List<AnimalType> animalTypeList = _animalTypeRepository.GetAll().ToList();
            return View(animalTypeList);
        }
        public IActionResult AddNewType()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewType(AnimalType animalType)
        {
            if (ModelState.IsValid)
            {
                _animalTypeRepository.Insert(animalType);
                _animalTypeRepository.Save();
                TempData["Success"] = "New animal type added successfuly.";
                return RedirectToAction("Index", "AnimalType");
            }
            return View();
        }
        public IActionResult EditType (int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            AnimalType? animalTypeV = _animalTypeRepository.Get(u=>u.Id == id);
            if (animalTypeV == null)
            {
                return NotFound();
            }
            return View(animalTypeV);
        }
        [HttpPost]
        public IActionResult EditType(AnimalType animalType)
        {
            if (ModelState.IsValid)
            {
                _animalTypeRepository.Update(animalType);
                _animalTypeRepository.Save();
                TempData["Success"] = "Animal type updated successfuly!";
                return RedirectToAction("Index","AnimalType");
            }
            return View();
        }
        public IActionResult DeleteType(int? id)
        {
            if (id == null || id == 0) { 
                return NotFound();
            }
            AnimalType? animalTypeV = _animalTypeRepository.Get(u => u.Id == id);
            if (animalTypeV == null) { 
                return NotFound();
            }
            return View(animalTypeV);
        }
        [HttpPost, ActionName("DeleteType")]
        public IActionResult DeleteTypePost(int? id)
        {
            AnimalType? animalTypeV = _animalTypeRepository.Get(u => u.Id == id);
            if (animalTypeV == null) { 
                return NotFound();
            }
            _animalTypeRepository.Delete(animalTypeV);
            _animalTypeRepository.Save();
            TempData["Success"] = "Animal Type deleted successfuly!";
            return RedirectToAction("Index", "Animaltype");
        }
    }
}
