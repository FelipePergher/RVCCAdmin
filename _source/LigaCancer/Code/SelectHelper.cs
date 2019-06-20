﻿using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code
{
    public class SelectHelper
    {
        public static async Task<List<SelectListItem>> GetDoctorSelectAsync(IDataStore<Doctor> doctorService)
        {
            List<Doctor> doctors = await doctorService.GetAllAsync();

            List<SelectListItem> selectListItems = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetTreatmentPlaceSelectAsync(IDataStore<TreatmentPlace> treatmentPlaceService)
        {
            List<TreatmentPlace> treatmentPlaces = await treatmentPlaceService.GetAllAsync();

            List<SelectListItem> selectListItems = treatmentPlaces.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetCancerTypesSelectAsync(IDataStore<CancerType> cancerTypeService)
        {
            List<CancerType> cancerTypes = await cancerTypeService.GetAllAsync();

            List<SelectListItem> selectListItems = cancerTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();

            return selectListItems;
        }

        public static async Task<List<SelectListItem>> GetMedicinesSelectAsync(IDataStore<Medicine> medicineService)
        {
            List<Medicine> medicines = await medicineService.GetAllAsync();

            List<SelectListItem> selectListItems = medicines.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();

            return selectListItems;
        }
    }
}