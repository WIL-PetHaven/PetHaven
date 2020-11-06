using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetHaven.DAL;
using PetHaven.Models;
using PetHaven.ViewModels;
using PagedList;

namespace PetHaven.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnimalsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Animals
        [AllowAnonymous]
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //instantiate a new view model
            AnimalIndexViewModel viewModel = new AnimalIndexViewModel();

            //select the Animals
            var Animals = db.Animals.Include(p => p.Category);

            //perform the search and save the search string to the viewModel
            if (!String.IsNullOrEmpty(search))
            {
                Animals = Animals.Where(p => p.Name.Contains(search) ||
                p.Description.Contains(search) ||
                p.Category.Name.Contains(search));
                viewModel.Search = search;
            }

            //group search results into categories and count how many items in each category
            viewModel.CatsWithCount = from matchingAnimals in Animals
                                      where
                                      matchingAnimals.CategoryID != null
                                      group matchingAnimals by
                                       matchingAnimals.Category.Name into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          AnimalCount = catGroup.Count()
                                      };

            if (!String.IsNullOrEmpty(category))
            {
                Animals = Animals.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }

            //sort the results
            switch (sortBy)
            {
                case "name":
                    Animals = Animals.OrderBy(p => p.Name);
                    break;
                case "name_rev":
                    Animals = Animals.OrderByDescending(p => p.Description);
                    break; 
                default:
                    Animals = Animals.OrderBy(p => p.Name);
                    break;
            }

            int currentPage = (page ?? 1);
            viewModel.Animals = Animals.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;
            viewModel.Sorts = new Dictionary<string, string>
            {
               {"Name", "name" },
               {"Description", "name_rev" }
            };

            return View(viewModel);
        }


        // GET: Animals/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal Animal = db.Animals.Find(id);
            if (Animal == null)
            {
                return HttpNotFound();
            }
            return View(Animal);
        }

        // GET: Animals/Create
        public ActionResult Create()
        {
            AnimalViewModel viewModel = new AnimalViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name");
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfAnimalImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.AnimalImages, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnimalViewModel viewModel)
        {
            Animal Animal = new Animal();
            Animal.Name = viewModel.Name;
            Animal.Description = viewModel.Description;
            Animal.CategoryID = viewModel.CategoryID;
            Animal.AnimalImageMappings = new List<AnimalImageMapping>();
            //get a list of selected images without any blanks
            string[] AnimalImages = viewModel.AnimalImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < AnimalImages.Length; i++)
            {
                Animal.AnimalImageMappings.Add(new AnimalImageMapping
                {
                    AnimalImage = db.AnimalImages.Find(int.Parse(AnimalImages[i])),
                    ImageNumber = i
                });
            }

            if (ModelState.IsValid)
            {
                db.Animals.Add(Animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", Animal.CategoryID);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfAnimalImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.AnimalImages, "ID", "FileName", viewModel.AnimalImages[i]));
            }
            return View(viewModel);
        }


        // GET: Animals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal Animal = db.Animals.Find(id);
            if (Animal == null)
            {
                return HttpNotFound();
            }
            AnimalViewModel viewModel = new AnimalViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", Animal.CategoryID);
            viewModel.ImageLists = new List<SelectList>();

            foreach (var imageMapping in Animal.AnimalImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.AnimalImages, "ID", "FileName", imageMapping.AnimalImageID));
            }

            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfAnimalImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.AnimalImages, "ID", "FileName"));
            }

            viewModel.ID = Animal.ID;
            viewModel.Name = Animal.Name;
            viewModel.Description = Animal.Description;

            return View(viewModel);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnimalViewModel viewModel)
        {
            var AnimalToUpdate = db.Animals.Include(p => p.AnimalImageMappings).Where(p => p.ID == viewModel.ID).Single();

            if (TryUpdateModel(AnimalToUpdate, "", new string[] { "Name", "Description", "Price", "CategoryID" }))
            {
                if (AnimalToUpdate.AnimalImageMappings == null)
                {
                    AnimalToUpdate.AnimalImageMappings = new List<AnimalImageMapping>();
                }
                //get a list of selected images without any blanks
                string[] AnimalImages = viewModel.AnimalImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < AnimalImages.Length; i++)
                {
                    //get the image currently stored
                    var imageMappingToEdit = AnimalToUpdate.AnimalImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //find the new image
                    var image = db.AnimalImages.Find(int.Parse(AnimalImages[i]));
                    //if there is nothing stored then we need to add a new mapping
                    if (imageMappingToEdit == null)
                    {
                        //add image to the imagemappings
                        AnimalToUpdate.AnimalImageMappings.Add(new AnimalImageMapping
                        {
                            ImageNumber = i,
                            AnimalImage = image,
                            AnimalImageID = image.ID
                        });
                    }
                    //else it's not a new file so edit the current mapping
                    else
                    {
                        //if they are not the same
                        if (imageMappingToEdit.AnimalImageID != int.Parse(AnimalImages[i]))
                        {
                            //assign image property of the image mapping
                            imageMappingToEdit.AnimalImage = image;
                        }
                    }
                }
                //delete any other imagemappings that the user did not include in their selections for the Animal
                for (int i = AnimalImages.Length; i < Constants.NumberOfAnimalImages; i++)
                {
                    var imageMappingToEdit = AnimalToUpdate.AnimalImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //if there is something stored in the mapping
                    if (imageMappingToEdit != null)
                    {
                        //delete the record from the mapping table directly. 
                        //just calling AnimalToUpdate.AnimalImageMappings.Remove(imageMappingToEdit) results in a FK error
                        db.AnimalImageMappings.Remove(imageMappingToEdit);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        // GET: Animals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal Animal = db.Animals.Find(id);
            if (Animal == null)
            {
                return HttpNotFound();
            }
            return View(Animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal Animal = db.Animals.Find(id);
            db.Animals.Remove(Animal);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
