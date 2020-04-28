using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Business
{
    public class SelectHelper
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
    }
}
