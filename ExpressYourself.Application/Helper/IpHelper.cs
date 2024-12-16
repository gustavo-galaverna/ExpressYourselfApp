using System.Net;

namespace ExpressYourself.Application.Helper;

public static class IpHelper
{

    public static bool IsValidIP(string ipString) 
    { 
        if (IPAddress.TryParse(ipString, out _)) 
        {
             return true; 
        } 
        
        return false; 
    }

}