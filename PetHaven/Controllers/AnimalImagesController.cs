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
using System.Web.Helpers;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;


namespace PetHaven.Controllers
{
    public class AnimalImagesController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: AnimalImages
        public ActionResult Index()
        {
            return View(db.AnimalImages.ToList());
        }

        // GET: AnimalImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: AnimalImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            //check the user has entered a file
            if (files[0] != null)
            {
                //if the user has entered less than ten files
                if (files.Length <= 10)
                {
                    //check they are all valid
                    foreach (var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles += ", " + file.FileName;
                        }
                    }
                    //if they are all valid then try to save them to disk
                    if (allValid)
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                SaveFileToDisk(file);
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("FileName", "Sorry an error occurred saving the files to disk, please try again");
                            }
                        }
                    }
                    //else add an error listing out the invalid files            
                    else
                    {
                        ModelState.AddModelError("FileName", "All files must be gif, png, jpeg or jpg  and less than 2MB in size. The following files" + inValidFiles +
                            " are not valid");
                    }
                }
                //the user has entered more than 10 files
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to ten files at a time");
                }
            }
            else
            {
                //if the user has not entered a file return an error message
                ModelState.AddModelError("FileName", "Please choose a file");
            }

            if (ModelState.IsValid)
            {
                bool duplicates = false;
                bool otherDbError = false;
                string duplicateFiles = "";
                foreach (var file in files)
                {
                    //try and save each file
                    var AnimalToAdd = new AnimalImage { FileName = file.FileName };
                    try
                    {
                        db.AnimalImages.Add(AnimalToAdd);
                        db.SaveChanges();
                    }
                    //if there is an exception check if it is caused by a duplicate file
                    catch (DbUpdateException ex)
                    {
                        SqlException innerException = ex.InnerException.InnerException as SqlException;
                        if (innerException != null && innerException.Number == 2601)
                        {
                            duplicateFiles += ", " + file.FileName;
                            duplicates = true;
                            db.Entry(AnimalToAdd).State = EntityState.Detached;
                        }
                        else
                        {
                            otherDbError = true;
                        }
                    }
                }
                //add a list of duplicate files to the error message
                if (duplicates)
                {
                    ModelState.AddModelError("FileName", "All files uploaded except the files" + duplicateFiles + ", which already exist in the system." +
                        " Please delete them and try again if you wish to re-add them");
                    return View();
                }
                else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Sorry an error has occurred saving to the database, please try again");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: AnimalImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnimalImage AnimalImage = db.AnimalImages.Find(id);
            //find all the mappings for this image
            var mappings = AnimalImage.AnimalImageMappings.Where(pim => pim.AnimalImageID == id);
            foreach (var mapping in mappings)
            {
                //find all mappings for any Animal containing this image
                var mappingsToUpdate = db.AnimalImageMappings.Where(pim => pim.AnimalID == mapping.AnimalID);
                //for each image in each Animal change its imagenumber to one lower if it is higher than the current image
                foreach (var mappingToUpdate in mappingsToUpdate)
                {
                    if (mappingToUpdate.ImageNumber > mapping.ImageNumber)
                    {
                        mappingToUpdate.ImageNumber--;
                    }
                }
            }

            System.IO.File.Delete(Request.MapPath(Constants.AnimalImagePath + AnimalImage.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.AnimalThumbnailPath + AnimalImage.FileName));
            db.AnimalImages.Remove(AnimalImage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            AnimalImage AnimalImage = db.AnimalImages.Find(id);

            return View(AnimalImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".gif", ".png", ".jpeg", ".jpg" };
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) && allowedFileTypes.Contains(fileExtension))
            {
                return true;
            }
            return false;
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width > 190)
            {
                img.Resize(190, img.Height);
            }
            img.Save(Constants.AnimalImagePath + file.FileName);
            if (img.Width > 100)
            {
                img.Resize(100, img.Height);
            }
            img.Save(Constants.AnimalThumbnailPath + file.FileName);
        }

    }
}
