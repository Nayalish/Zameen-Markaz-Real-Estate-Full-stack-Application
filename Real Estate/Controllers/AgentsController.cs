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
    public class AgentsController : Controller
    {
        private readonly RealestateContext context;
		private readonly IWebHostEnvironment enviroment;

        public AgentsController(RealestateContext context, IWebHostEnvironment enviroment)
        {

            this.context = context;
            this.enviroment = enviroment;
        }
		public IActionResult Index()
		{
			var data = context.Agents.ToList();
			return View(data);
		}
		public IActionResult Create()
		{

			return View();
		}
		[HttpPost]
		public IActionResult Create(Agent model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					string uniquefilename = UploadImage(model);
					var data = new Agent()
					{
						Name = model.Name,
						Location = model.Location,
						Path = uniquefilename
					};
					TempData["success"] = "The Shopping center added successfully!";
					context.Agents.Add(data);
					context.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					TempData["error"] = "The Shoppinge center is not created please check!";
				}
				ModelState.AddModelError("", "Model property is not valid please check");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(String.Empty, ex.Message);
			}

			return View(model);
		}

		private string UploadImage(Agent model)
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
				var data = context.Agents.Where(e => e.Id == id).SingleOrDefault();
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
					TempData["success"] = "The shopping center is deleted successfully!";
					context.Agents.Remove(data);
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
			var data = context.Agents.Where(e => e.Id == id).SingleOrDefault();
			return View(data);

		}
		public IActionResult Edit(int id)
		{
			var data = context.Agents.Where(e => e.Id == id).SingleOrDefault();
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
		public IActionResult Edit(Agent model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var data = context.Agents.Where(e => e.Id == model.Id).SingleOrDefault();
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
					data.Name = model.Name;
					data.Location = model.Location;

					if (model.Image != null)
					{
						data.Path = uniqueFileName;
					}
					TempData["success"] = "The Shopping center is updated successfully!";
					context.Agents.Update(data);
					context.SaveChanges();

				}
				else
				{
					TempData["error"] = "The Shoppinge center is not created please check!";
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
