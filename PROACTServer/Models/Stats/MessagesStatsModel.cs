namespace Proact.Services.Models.Stats {
    public class MessagesStatsModel {
        public PatientMessagesStatsModel PatientsMessagesStats { get; set; }
        public MedicMessagesStatsModel MedicsMessagesStats { get; set; }
        public NurseMessagesStatsModel NursesMessagesStats { get; set; }
    }
}
