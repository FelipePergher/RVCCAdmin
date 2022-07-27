// <copyright file="PatientPrintViewModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using RVCC.Business;

namespace RVCC.Models.ViewModel
{
    public class PatientPrintViewModel
    {
        #region Profile

        public string FullName { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public bool FamiliarityGroup { get; set; }

        public bool ForwardedToSupportHouse { get; set; }

        public Enums.Sex Sex { get; set; }

        public Enums.CivilState? CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string Profession { get; set; }

        public string ImmediateNecessities { get; set; }

        public DateTime ImmediateNecessitiesDateUpdated { get; set; }

        public string MonthlyIncome { get; set; }

        #endregion

        #region Patient Information

        public DateTime TreatmentBeginDate { get; set; }

        public List<string> CancerTypes { get; set; }

        public List<string> Doctors { get; set; }

        public List<string> TreatmentPlaces { get; set; }

        public List<string> Medicines { get; set; }

        public List<string> ServiceTypes { get; set; }

        #endregion

        #region Naturality

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        #endregion

        public IEnumerable<PhoneViewModel> Phones { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }

        public IEnumerable<FamilyMemberViewModel> FamilyMembers { get; set; }

        public IEnumerable<PatientExpenseTypeViewModel> PatientExpenseTypes { get; set; }

        public IEnumerable<PatientAuxiliarAccessoryTypeViewModel> PatientAuxiliarAccessoryTypes { get; set; }

        public IEnumerable<PatientBenefitViewModel> Benefits { get; set; }

        public IEnumerable<StayViewModel> Stays { get; set; }

        public IEnumerable<FileAttachmentViewModel> Files { get; set; }
    }
}
