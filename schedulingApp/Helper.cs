using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

//Get the Lang/ location info 
//Validate fields
//Exception handling in DB connection

public class Helper
{
    private CultureInfo userCulture;
    private TimeZoneInfo userTimeZone;
    private RegionInfo userRegion;

    public Helper()
    {
        InitializeUserLocation();
    }

    private void InitializeUserLocation()
    {
        userCulture = CultureInfo.CurrentUICulture;
        userTimeZone = TimeZoneInfo.Local;
        userRegion = new RegionInfo(userCulture.Name);
    }

    public string GetUserLocation()
    {
        return $"Country: {userRegion.DisplayName}\n" +
               $"Language: {userCulture.DisplayName}\n" +
               $"Timezone: {userTimeZone.DisplayName}";
    }

    public string TranslateMessage(string messageKey)
    {
        if (userCulture.Name.StartsWith("es")) // Spanish translations
        {
            switch (messageKey)
            {
                case "LoginFailed":
                    return "El nombre de usuario y la contraseña no coinciden.";
                case "LoginSuccessful":
                    return "Inicio de sesión exitoso.";
                case "UpcomingAppointment":
                    return "Tiene una cita próxima en {0} minutos.";
                case "NoUpcomingAppointments":
                    return "No tiene citas próximas.";
                case "ValidationError":
                    return "Error de validación";
                case "DatabaseError":
                    return "Error de base de datos";
                default:
                    return messageKey;
            }
        }

        // Default English translations
        switch (messageKey)
        {
            case "LoginFailed":
                return "The username and password do not match.";
            case "LoginSuccessful":
                return "Login successful.";
            case "UpcomingAppointment":
                return "You have an upcoming appointment in {0} minutes.";
            case "NoUpcomingAppointments":
                return "You have no upcoming appointments.";
            case "ValidationError":
                return "Validation Error";
            case "DatabaseError":
                return "Database Error";
            default:
                return messageKey;
        }
    }
}
