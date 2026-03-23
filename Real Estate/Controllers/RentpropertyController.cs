using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Models;

namespace Real_Estate.Controllers
{
    public class RentpropertyController : Controller
    {
        private readonly RealestateContext context;
		private readonly IWebHostEnvironment enviroment;

        public RentpropertyController(RealestateContext context, IWebHostEnvironment enviroment)
        {

            this.context = context;
            this.enviroment = enviroment;
        }
		public IActionResult Index()
		{
			var data = context.RentPropertys.ToList();
			return View(data);
		}
		public IActionResult Create()
		{

			return View();
		}
		[HttpPost]
		public IActionResult Create(RentProperty model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					string uniquefilename = UploadImage(model);
					var data = new RentProperty()
					{
						Author_Name = model.Author_Name,
						Property_Name= model.Property_Name,
						Location = model.Location,
                        Price = model.Price,
                        Path = uniquefilename
					};
					TempData["success"] = "The property added successfully!";
					context.RentPropertys.Add(data);
					context.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					TempData["error"] = "The property is not created please check!";
				}
				ModelState.AddModelError("", "Model property is not valid please check");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(String.Empty, ex.Message);
			}

			return View(model);
		}

		private string UploadImage(RentProperty model)
		{
			string uniquefilename = "";
			if (model.Image != null)
			{
				string UploadFolder = Path.Combine(enviroment.WebRootPath, "images");
				uniquefilename = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
				string FilePath = Path.Combine(UploadFolder, uniquefilename);
				using (var fileStream = new FileStream(FilePath, FileMode.Create))
				{
					model.Image.CopyTo(fileStream);
				}
			}
			return uniquefilename;
		}
		public IActionResult Delete(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}
			else
			{
				var data = context.RentPropertys.Where(e => e.Id == id).SingleOrDefault();
				if (data != null)
				{
					string deleteFromFolder = Path.Combine(enviroment.WebRootPath, "images");
					string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, data.Path);
					if (currentImage != null)
					{
						if (System.IO.File.Exists(currentImage))
						{
							System.IO.File.Delete(currentImage);
						}
					}
					TempData["success"] = "The property is deleted successfully!";
					context.RentPropertys.Remove(data);
					context.SaveChanges();
					//TempData["Success"] = "Record Deleted!";
				}
			}
			return RedirectToAction("Index");
		}
		public IActionResult Details(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}
			var data = context.RentPropertys.Where(e => e.Id == id).SingleOrDefault();
			return View(data);

		}
		public IActionResult Edit(int id)
		{
			var data = context.RentPropertys.Where(e => e.Id == id).SingleOrDefault();
			if (data != null)
			{
				return View(data);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}
		[HttpPost]
		public IActionResult Edit(RentProperty model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var data = context.RentPropertys.Where(e => e.Id == model.Id).SingleOrDefault();
					string uniqueFileName = string.Empty;
					if (model.Image != null)
					{
						if (data.Path != null)
						{
							string filepath = Path.Combine(enviroment.WebRootPath, "images", data.Path);
							if (System.IO.File.Exists(filepath))
							{
								System.IO.File.Delete(filepath);
							}
						}
						uniqueFileName = UploadImage(model);
					}
					data.Author_Name = model.Author_Name;
					data.Property_Name = model.Property_Name;
					data.Location = model.Location;
					data.Price = model.Price;

					if (model.Image != null)
					{
						data.Path = uniqueFileName;
					}
					TempData["success"] = "The property is updated successfully!";
					context.RentPropertys.Update(data);
					context.SaveChanges();

				}
				else
				{
					TempData["error"] = "The property is not created please check!";
					return View(model);
				}


			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return RedirectToAction("index");
		}


	}
}
