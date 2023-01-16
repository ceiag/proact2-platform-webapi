# PROACT 2.0 WebApi
Proact platform Wep Api with asp.net core.

# 📃 Documentation 
- [**Technical Documentation**](https://docs.google.com/document/d/1XitNkwCbWfw5--2c_wypwBLuMGsYa-cDYiPQACIzIx0/edit?usp=sharing)
- [**PROACT 2.0 Services Documentation**](https://docs.google.com/document/d/1xgX0qPAFjeWgdH9gVOba21DPrQkY5ucp3GL4qPTUTrw/edit?usp=sharing)

## Current Version [1.2.0][beta]
Please read the [Changelog.md](https://github.com/digital-ECMT/proact-platform-webapi/blob/develop/CHANGELOG.md) to see the changes.

# Installation

### Create Super Admin
After the installation of PROACT2.0 API you must create the first user on the system with a Super Admin role. To do this you must call the API in POST `SystemConfiguration`. You have to insert a valid email for the Super Admin and a Name and a Surname. You can use Swagger. After this operation the you will have a Super Admin that can create users. Please change the password of you Super Admin user.

### AppSettings.json
`AppSettings.json` has two separated files `AppSettings.Development.json` and `AppSettings.Production.json`. Be aware to configure in the right way your Environment.

### Azure Media Storage
First thing first you must create a Storage Account on Azure Portal. After this you have to configure the `AppSettings.json` file and insert your `ConnectionString`. Remember to set properties for `ResourceGroup` and `MediaStorageUrl` with you Azure Portal Urls.

Everytime a user upload something (video, audio or image) those are stored in different folders named in this way:

- `mediafiles-{userId}` will contains audio
- `mediafiles-thumbs-{userId}` will contains the thumbnail of the video
- `mediafiles-images-{userId}` will contains the images uploaded

For the video files, PROACT use Azure Media Services to encode and save video files. You must configure your parameters of your Azure Media Service subscription into `AppSettings.Development.json` and `AppSettings.Production.json`. You must follow the official guide lines of Azure Media Services configuration.

### Default Avatars
Open `AppSettings.json` and put the url of default avatars into:

```json
"DefaultAvatars": {
    "MedicAvatarDefaultUrl": "",
    "PatientAvatarDefaultUrl": "",
    "NurseAvatarDefaultUrl": ""
  }
```

### Azure AD B2C
Open `AppSettings.json` and follow the *[current documentation](https://docs.google.com/document/d/1_49bVIjXpugXBPZOqmD4EyNlIzRrh7FQMA7NftG7ulY/edit?usp=sharing)*.

### Logger
You must enable Logging on Azure Portal: 
https://docs.microsoft.com/en-us/azure/app-service/troubleshoot-diagnostic-logs#enable-application-logging-windows

### OneSignal for Push Notifications
PROACT 2.0 WebApi use OneSignal to send push notifications to all users. Please follow the documentation on [OneSignal Site](https://documentation.onesignal.com/).

1. Open `OneSignalConfiguration.json`
2. Put your `ApiKey` and `ApiId` generated on OneSignal Portal

### Automatic Surveys Delivery
PROACT 2.0 send scheduled surverys to the users using [Azure Fuctions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-scheduled-function). Please use the script at `SurveyCheckerTrigger.cs`

### Email sending
PROACT 2.0 use Azure to send email to the users. Please follow this [tutorial here](https://docs.microsoft.com/en-us/azure/app-service/tutorial-send-email?tabs=dotnet). Open the `appsettings.json` and modify `"EmailSender": { "RequestUrl" }` both in development and production. 
