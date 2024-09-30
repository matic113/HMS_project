﻿using DALProject.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS_Project.ViewModels
{
    internal class PrescriptionViewModel
    {
        public PrescriptionViewModel() { }
        public PrescriptionViewModel(Prescription prescription) 
        {
            PrescriptionID = prescription.PrescriptionID;
            PrescriptionItems = (HashSet<PrescriptionItem>)prescription.PrescriptionItems;
            PharmacistId = prescription.PharmacistId;
            AppointmentId = prescription.AppointmentId;
            DoctorId = prescription.DoctorId;
        }

        [Required]
        public int PrescriptionID { get; set; }
        HashSet<PrescriptionItem> PrescriptionItems { get; set; } = new HashSet<PrescriptionItem>();
        public long? PharmacistId { get; set; }
        public int AppointmentId { get; set; }
        public long DoctorId { get; set; }

        public static explicit operator Prescription(PrescriptionViewModel prescriptionViewModel)
        {
            return new Prescription()
            {
                PrescriptionID = prescriptionViewModel.PrescriptionID,
                PrescriptionItems = prescriptionViewModel.PrescriptionItems,
                PharmacistId = prescriptionViewModel?.PharmacistId,
                AppointmentId = prescriptionViewModel.AppointmentId,
                DoctorId = prescriptionViewModel.DoctorId,
            };
        }

        public static explicit operator PrescriptionViewModel(Prescription prescription)
        {
            return new PrescriptionViewModel(prescription);
        }
    }
}