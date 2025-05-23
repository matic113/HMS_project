﻿using DALProject.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Data.Configurations
{
    internal class ApointmentConfigurations : IEntityTypeConfiguration<Apointment>
    {
        public void Configure(EntityTypeBuilder<Apointment> builder)
        {
            #region ApointmentConfiguration 
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).UseIdentityColumn(1, 1);
            builder.Property(a => a.ApointmentDate).HasColumnType($"{DB_DataTypes_Helper.date}");
            builder.Property(a => a.ApointmentTime).HasColumnType($"{DB_DataTypes_Helper.time}");
            builder.Property(a => a.ApointmentStatus).HasColumnType($"{DB_DataTypes_Helper.nvarchar}").HasMaxLength(15);
            builder.Property(a => a.ApointmentStatus).
                HasConversion(status=>status.ToString(), (statusAsString) => (ApointmentStatusEnum)Enum.Parse(typeof(ApointmentStatusEnum), statusAsString, true));
            #endregion

            #region One2Many With Receptionist
            builder
                    .HasOne(a => a.Receptionist)
                    .WithMany(a => a.Apointments)
                    .HasForeignKey(a => a.ReceptionistUserId)
                    .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region One2Many With Patient
            builder
                    .HasOne(a => a.Patient)
                    .WithMany(a => a.Apointments)
                    .HasForeignKey(a => a.PatientUserId);
            #endregion

            #region One2Many With Clinic 
            builder
                    .HasOne(a => a.Clinic)
                    .WithMany(c => c.Apointments)
                    .HasForeignKey(c => c.ClinicId); 
            #endregion

            #region One2Many With doctor 
            builder 
                .HasOne(a=>a.Doctor)
                .WithMany(d=>d.Apointments)
                .HasForeignKey(a=>a.DoctorUserId);
            #endregion

            #region One2One With Prescription
            builder 
                .HasOne(a=>a.Prescription)
                .WithOne(a=>a.Apointment).HasForeignKey<Apointment>(p=>p.PrescriptionId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region One2One With Invoice
            builder
                .HasOne(a=>a.Invoice)
                .WithOne(i=>i.Apointment)
                .HasForeignKey<Apointment>(i=>i.InvoicId)
				.OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
