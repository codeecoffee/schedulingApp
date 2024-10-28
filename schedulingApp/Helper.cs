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
    private const string correctUsername = "test";
    private const string correctPassword = "test";
    private string userLanguage;
    private string connectionString;
    
    public Helper()
    {
        userLanguage = GetLocalMachineLanguage();
        

    }

    private string GetLocalMachineLanguage()
    {
        CultureInfo currentCulture = CultureInfo.CurrentUICulture;
        return currentCulture.Name;
    }

    public string TranslateMessage(string message)
    {
        if(userLanguage == "pt-BR")
        {
            switch(message)
            {
                case "The username and password do not match.":
                    return "O nome de usuário ou senha está incorreto";
                case "Login successful.":
                    return "Sucesso";
                default:
                    return message; // Default to original message if not translated 
            }
        }
        return message;
    }

    public bool ValidateCredentials(string username, string password)
    {
        if (username == correctUsername && password == correctPassword)
        {
            MessageBox.Show(TranslateMessage("Login successful."));
            return true;
        }
        else
        {
            MessageBox.Show(TranslateMessage("The username and password do not match."));
            return false;
        }
    }

    public string ValidateInput(string input)
    {
        //TODO! 
        //Implement ValidateInput method
        //This should trim and validate inputs as well as compare if passwords are the same;

        return input;
    }
}
