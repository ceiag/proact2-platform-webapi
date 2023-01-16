using System;

namespace Proact.Services.Models;
public class NotificationTextContents {
    public string en { get; set; }
    public string it { get; set; }
}

public abstract class OneSignalNotificationCreationRequest {
    public string app_id { get; set; }
    public string small_icon { get; set; } = "ic_notification";
    public string[] include_player_ids { get; set; }
    public NotificationTextContents contents { get; set; }
}

public class OneSignalMessageInfoData {
    public Guid OpenMessageDetail { get; set; }
}

public class OneSignalSurveyInfoData {
    public string OpenNotCompiledSurveys { get; set; } = "OpenNotCompiledSurveys";
}

public class OneSignalNewMessageNotificationCreationRequest : OneSignalNotificationCreationRequest {
    public OneSignalMessageInfoData data { get; set; }
}

public class OneSignalNewSurveyNotificationCreationRequest : OneSignalNotificationCreationRequest {
    public OneSignalSurveyInfoData data { get; set; }
}
