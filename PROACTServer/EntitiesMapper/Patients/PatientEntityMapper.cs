using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public static class PatientEntityMapper {
        public static List<PatientModel> Map( List<Patient> patients, bool anonimized ) {
            List<PatientModel> patientModels = new List<PatientModel>();

            foreach ( var patient in patients ) {
                patientModels.Add( Map( patient, anonimized ) );
            }

            return patientModels;
        }

        public static PatientModel Map( Patient patient, bool anonimized ) {
            if ( patient == null )
                return null;

            var medicalTeam = new List<MedicalTeamModel>();

            if ( patient.MedicalTeam != null ) {
                medicalTeam = new List<MedicalTeamModel>() {
                    MedicalTeamEntityMapper.Map( patient.MedicalTeam )
                };
            }

            return new PatientModel() {
                AccountId = patient.User.AccountId,
                AvatarUrl = patient.User.AvatarUrl,
                InstituteId = (Guid)patient.User.InstituteId,
                BirthYear = patient.BirthYear,
                Gender = patient.Gender,
                MedicalTeam = medicalTeam,
                Name = anonimized || string.IsNullOrWhiteSpace( patient.User.Name ) ? patient.Code : patient.User.Name,
                State = patient.User.State,
                Title = patient.User.Title,
                TreatmentStartDate = patient.TreatmentStartDate,
                TreatmentEndDate = patient.TreatmentEndDate,
                UserId = patient.User.Id,
                Code = patient.Code,
            };
        }
    }
}
