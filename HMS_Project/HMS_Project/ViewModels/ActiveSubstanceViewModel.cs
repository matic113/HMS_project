﻿using DALProject.Data.Contexts;
using DALProject.model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS_Project.ViewModels
{
	public class ActiveSubstanceViewModel
	{
		public ActiveSubstanceViewModel()
		{
		}
		public ActiveSubstanceViewModel(ActiveSubstance activeSubstance)
		{
			Id = activeSubstance.ActiveSubstancesId;
			ActiveSubstancesName = activeSubstance.ActiveSubstancesName;

			Medications = activeSubstance.Medications.ToHashSet();

			Interactions = activeSubstance.ActSub1.Select(interaction => new ActiveSubstanceInteractionViewModel
			{
				Interaction = interaction.Interaction,
				OtherSubstanceName = interaction.ActSub2?.ActiveSubstancesName ?? "Unknown",
				ActSubId=interaction.ActiveSubstanceId2??0
			})
			.Concat(activeSubstance.ActSub2.Select(interaction => new ActiveSubstanceInteractionViewModel
			{
				Interaction = interaction.Interaction,
				OtherSubstanceName = interaction.ActSub1?.ActiveSubstancesName ?? "Unknown",
				ActSubId = interaction.ActiveSubstanceId1 ?? 0

			}))
			.ToHashSet();
		}
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is Required"), Display(Name = "Active Substances Name")]
		public string ActiveSubstancesName { get; set; } = null!;
		public IEnumerable<ActiveSubstance>? ActiveSubstancesDateReader { get; set; }
		public IEnumerable<Medication>? MedicationsDateReader { get; set; }
		public HashSet<int> MedicationId { get; set; }
		public HashSet<int> ActivSubstanceId { get; set; }
		public ICollection<Medication>? Medications { get; set; } =new HashSet<Medication>();
		public ICollection<ActiveSubstanceInteractionViewModel>? Interactions { get; set; } = new HashSet<ActiveSubstanceInteractionViewModel>();

		public static explicit operator ActiveSubstance(ActiveSubstanceViewModel viewModel)
		{
			var activeSubstance = new ActiveSubstance
			{
				ActiveSubstancesName = viewModel.ActiveSubstancesName,
				Medications = viewModel.Medications
			};
			foreach (var InteractionSubsatance in viewModel.ActivSubstanceId)
			{
				activeSubstance.ActSub1.Add(new ActiveSubstanceInteraction
				{
					ActiveSubstanceId2 = InteractionSubsatance
                });
			}
			return activeSubstance;
		}

		public static explicit operator ActiveSubstanceViewModel(ActiveSubstance activeSubstance)
		{
			return new ActiveSubstanceViewModel(activeSubstance);
		}
	}
}
