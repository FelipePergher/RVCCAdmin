using LigaCancer.Code.Interface;
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
        private readonly IDataStore<Doctor> _doctorService;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataStore<CancerType> _cancerTypeService;
        private readonly IDataStore<Medicine> _medicineService;

        public SelectHelper(
           IDataStore<Doctor> doctorService,
           IDataStore<TreatmentPlace> treatmentPlaceService,
           IDataStore<CancerType> cancerTypeService,
           IDataStore<Medicine> medicineService)
        {
            _doctorService = doctorService;
            _cancerTypeService = cancerTypeService;
            _treatmentPlaceService = treatmentPlaceService;
            _medicineService = medicineService;
        }

        public async Task<List<SelectListItem>> GetDoctorSelectAsync()
        {
            List<Doctor> doctors = await _doctorService.GetAllAsync();

            List<SelectListItem> selectListItems = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            return selectListItems;
        }
    }
}
