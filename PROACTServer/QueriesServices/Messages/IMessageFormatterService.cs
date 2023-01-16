using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices;
public interface IMessageFormatterService : IDataEditorService {
    public BranchedMessagesModel GetMessageAsMedic( Guid messageId, int limitRepliesCount );
    public BranchedMessagesModel GetMessageAsPatient( Guid messageId, int limitRepliesCount );
    public BranchedMessagesModel GetPatientMessageForAnalystConsole(
        Guid messageId, UserRoles requesterRoles, int limitRepliesCount );
    public List<BranchedMessagesModel> GetPatientMessagesForAnalystConsole(
        Patient patient, UserRoles requesterRoles, int pagingCount, int limitRepliesCount );
    public List<BranchedMessagesModel> GetMessagesAsPatient(
        Patient patient, int pagingCount, int limitRepliesCount );
    public List<BranchedMessagesModel> GetAllMessagesForMedicalTeam( Guid medicalTeamId );
    public List<MessageModel> SearchMessagesAsPatient( Patient patient, string searchParams, int maxRows );
    public List<MessageModel> SearchMessagesAsMedic( Guid medicalTeamId, string searchParams, int maxRows );
    public List<MessageModel> SearchMessagesAsResearcher( Guid medicalTeamId, string searchParams, int maxRows );
    public List<BranchedMessagesModel> GetMessagesAsMedic(
        MedicalTeam medicalTeam, int pagingCount, int limitRepliesCount );
    public List<BranchedMessagesModel> GetMessagesAsResearcher(
        MedicalTeam medicalTeam, int pagingCount, int limitRepliesCount );
    public List<BranchedMessagesModel> GetMessagesAsMedicUnreplied(
        Guid medicalTeamId, int pagingCount, int limitRepliesCount );
    public List<BranchedMessagesModel> GetMessagesAsResearcher(
        Guid medicalTeamId, int pagingCount, int limitRepliesCount );
    public List<User> GetMessageRecipients( Guid messageId );
    public List<User> GetMessageRecipientsExceptUser( Guid userId, Guid messageId );
}
