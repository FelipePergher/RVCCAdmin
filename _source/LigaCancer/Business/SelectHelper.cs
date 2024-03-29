﻿// <copyright file="SelectHelper.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Business
{
    public static class SelectHelper
    {
        public static async Task<List<SelectListItem>> GetDoctorSelectAsync(IDataRepository<Doctor> doctorService)
        {
            List<Doctor> doctors = await doctorService.GetAllAsync();

            var selectListItems = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetTreatmentPlaceSelectAsync(IDataRepository<TreatmentPlace> treatmentPlaceService)
        {
            List<TreatmentPlace> treatmentPlaces = await treatmentPlaceService.GetAllAsync();

            var selectListItems = treatmentPlaces.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetCancerTypesSelectAsync(IDataRepository<CancerType> cancerTypeService)
        {
            List<CancerType> cancerTypes = await cancerTypeService.GetAllAsync();

            var selectListItems = cancerTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetMedicinesSelectAsync(IDataRepository<Medicine> medicineService)
        {
            List<Medicine> medicines = await medicineService.GetAllAsync();

            var selectListItems = medicines.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetExpenseTypeSelectAsync(IDataRepository<ExpenseType> expenseTypeService, int patientId, int includeExpense = 0)
        {
            List<ExpenseType> expenseTypes = await ((ExpenseTypeRepository)expenseTypeService).GetNotRelatedToPatient(patientId, includeExpense);

            var selectListItems = expenseTypes.Select(x => new SelectListItem
            {
                Text = $"{x.Name} ({Enums.GetDisplayName(x.ExpenseTypeFrequency)})",
                Value = x.ExpenseTypeId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetTreatmentTypeSelectAsync(IDataRepository<TreatmentType> treatmentTypeService, int patientId, int includeTreatment = 0)
        {
            List<TreatmentType> treatmentTypes = await ((TreatmentTypeRepository)treatmentTypeService).GetNotRelatedToPatient(patientId, includeTreatment);

            var selectListItems = treatmentTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.TreatmentTypeId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetAuxiliarAccessoryTypeSelectAsync(IDataRepository<AuxiliarAccessoryType> auxiliarAccessoryTypeService, int patientId, int includeExpense = 0)
        {
            List<AuxiliarAccessoryType> auxiliarAccessoryTypes = await ((AuxiliarAccessoryTypeRepository)auxiliarAccessoryTypeService).GetNotRelatedToPatient(patientId, includeExpense);

            var selectListItems = auxiliarAccessoryTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.AuxiliarAccessoryTypeId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetServiceTypesSelectAsync(IDataRepository<ServiceType> serviceTypeService)
        {
            List<ServiceType> serviceTypes = await serviceTypeService.GetAllAsync();

            var selectListItems = serviceTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ServiceTypeId.ToString()
            }).ToList();

            return selectListItems;
        }
    }
}
