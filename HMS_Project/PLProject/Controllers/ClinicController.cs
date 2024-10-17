﻿using BLLProject.Interfaces;
using DALProject.Data.Contexts;
using DALProject.model;
using PLProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLLProject.Specification;

namespace PLProject.Controllers
{
	[Authorize(Roles = Roles.Admin)]
	public class ClinicController : Controller
	{
		#region DPI
		private readonly IUnitOfWork unitOfWork;
		public ClinicController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		#endregion

		#region Index
		public IActionResult Index()
		{
			var clinics = unitOfWork.Repository<Clinic>().GetALL();
			var clinicViewModels = clinics.Select(c => (ClinicViewModel)c).ToList();
			return View(clinicViewModels);
		}
		#endregion

		#region Specialization
		public IActionResult AddSpecialization()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddSpecialization(DoctorSpecializationLookup doctorSpecialization)
		{
			if (ModelState.IsValid) // server side validation
			{
				unitOfWork.Repository<DoctorSpecializationLookup>().Add(doctorSpecialization);
				unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}
			return View(doctorSpecialization);
		}
		#endregion

		#region Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ClinicViewModel clinicViewModel)
		{
			if (ModelState.IsValid) // server side validation
			{
				var updatedClinic = (Clinic)clinicViewModel;

				var AddedDoctor = unitOfWork.Repository<Doctor>().Find(d => clinicViewModel.DoctorsAddedId.Contains(d.Id));
				var AddedNurse = unitOfWork.Repository<Nurse>().Find(d => clinicViewModel.NursesAddedId.Contains(d.Id));

				if (AddedDoctor is not null)
				{
					clinicViewModel.Doctors.AddRange(AddedDoctor);
					updatedClinic.Doctors = clinicViewModel.Doctors;
				}

				if (AddedNurse is not null)
				{
					clinicViewModel.Nurses.AddRange(AddedNurse);
					updatedClinic.Nurses = clinicViewModel.Nurses;
				}
				unitOfWork.Repository<Clinic>().Add(updatedClinic);
				unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}
			return View(clinicViewModel);
		}
		#endregion

		#region Details
		public IActionResult Details(int? Id)
		{
			if (!Id.HasValue)
				return BadRequest(); // 400

			var spec = new BaseSpecification<Clinic>(c => c.Id == Id);
			spec.Includes.Add(c => c.ClinicSpecilization);
			unitOfWork.Complete();

			var clinic = unitOfWork.Repository<Clinic>().GetEntityWithSpec(spec);
			var clinicViewModel = (ClinicViewModel)clinic;

			if (clinic is null)
				return NotFound(); // 404

			return View(clinicViewModel);
		}

		#endregion

		#region Edit

		public IActionResult Edit(int? Id)
		{
			if (!Id.HasValue)
				return BadRequest(); // 400

			var clinic = unitOfWork.Repository<Clinic>().Get(Id.Value);

			if (clinic is null)
				return NotFound(); // 404

			var clinicViewModel = (ClinicViewModel)clinic;

			return View(clinicViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Edit([FromRoute] int Id, ClinicViewModel viewModel)
		{
			if (Id != viewModel.Id)
				return BadRequest();

			Clinic updatedClinic = unitOfWork.Repository<Clinic>().Get(viewModel.Id);

			if (ModelState.IsValid) // server side validation
			{
				// Mapping the model
				updatedClinic.Name = viewModel.Name;
				updatedClinic.Specialization = viewModel.Specialization;
				updatedClinic.Phone = viewModel.Phone;
				updatedClinic.Price = viewModel.Price ?? default;

				viewModel.Doctors=updatedClinic.Doctors.ToList();
				viewModel.Nurses= updatedClinic.Nurses.ToList();

				var AddedDoctor = unitOfWork.Repository<Doctor>().Find(d => viewModel.DoctorsAddedId.Contains(d.Id));
				var AddedNurse = unitOfWork.Repository<Nurse>().Find(d => viewModel.NursesAddedId.Contains(d.Id));

				if (AddedDoctor is not null)
				{
					viewModel.Doctors.AddRange(AddedDoctor);
					updatedClinic.Doctors = viewModel.Doctors;
				}

				if (AddedNurse is not null)
				{
					viewModel.Nurses.AddRange(AddedNurse);
					updatedClinic.Nurses = viewModel.Nurses;
				}

				unitOfWork.Repository<Clinic>().Update(updatedClinic);
				unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		#endregion

		#region Delete

		public IActionResult Delete(int? Id)
		{
			if (!Id.HasValue)
				return BadRequest(); // 400

			var clinic = unitOfWork.Repository<Clinic>().Get(Id.Value);
			var clinicViewModel = (ClinicViewModel)clinic;

			if (clinic is null)
				return NotFound(); // 404

			return View(clinicViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete([FromRoute] int Id, ClinicViewModel clinicViewModel)
		{
			var clinic = unitOfWork.Repository<Clinic>().Get(clinicViewModel.Id);
			try
			{
				unitOfWork.Repository<Clinic>().Delete(clinic);
				unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return View(clinic);
			}
		}
		#endregion

	}
}
