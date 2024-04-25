using System.Collections.Generic;
using Godot;

namespace CastleOfDemise.Scripts
{
    /*
    public class MultiplayerCode
    {

        public const string basicIP = "127.0.0.1";


        public static string IpToCode(string ip)
        {
            if (ip.ToLower() == "localhost")
            {
                // on a le localhost, donc on transfère directement en code sans caulculer
                return "1MCCIR5T";
            }

            string[] ipParts = ip.Split('.');
            if (ipParts.Length != 4)
            {
                // on a pas 4 parties, donc on retourne une erreur
                return basicIP;
            }

            long ipValue = 0;
            for (int i = 0; i < 4; i++)
            {
                if (int.Parse(ipParts[i]) > 255 || int.Parse(ipParts[i]) < 0)
                {
                    // Vu que un octet ne peut pas être plus grand que 255, on retourne une erreur
                    return basicIP;
                }

                ipValue *= 1000;
                ipValue += int.Parse(ipParts[i]);
                // Console.WriteLine($"Converting {ip}, stage {i} =" + ipValue);
            }

            string code = Base36Converter.ConvertTo(ipValue);
            if (code.Length < 8)
            {
                // Tous les codes font exactement 8 caractères, donc tout va bien
                code = code.PadLeft(8, '0');
            }

            return code;

        }

        public static string CodeToIp(string code)
        {
            if (code.ToUpper() == "1MCCIR5T")
            {
                return basicIP;
            }

            if (code.Length != 8)
            {
                // Tous les codes font exactement 8 caractères, donc si on en a pas 8, on retourne une erreur
                return basicIP;
            }

            long ipValue = Base36Converter.ConvertFrom(code);

            // on check si les ip trouvées sont valide

            if (ipValue >= 10000000000 && ipValue <= 10255255255 ||
                ipValue >= 172016000000 && ipValue <= 172031255255 ||
                ipValue >= 192168000000 && ipValue <= 192168255255)
            {
                string ip = "";
                for (int i = 0; i < 4; i++)
                {
                    ip = (ipValue % 1000) + "." + ip;
                    ipValue /= 1000;
                }

                string[] ipParts = ip.Split('.');

                for (int i = 0; i < 4; i++)
                {
                    if (int.Parse(ipParts[i]) > 255 || int.Parse(ipParts[i]) < 0)
                    {
                        // Je viens de copier cette partie qui va check individuellement si l'ip est valide, donc il y a pas de problèmes normalement
                        return basicIP;
                    }
                }

                return ip.Substring(0, ip.Length - 1);
            }

            return basicIP;
            // 




        }

    }
    */
    /*
public static class Base36Converter
{
    public const int Base = 36;
    public const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string ConvertTo(long value)
    {
        string result = "";

        while (value > 0)
        {
            result = Chars[(int)(value % Base)] + result;
            value /= Base;
        }

        return result;
    }

    public static long ConvertFrom(string value)
    {
        long result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            result *= Base;
            result += Chars.IndexOf(value[i]);
        }
        return result;
    }
}
    */
    // Malhereusement, le code est inutile vu que trop chiant


    public class Player : Node2D
    {
        public int Id { get; set; }
        public  Vector2 Position { get; set; }
    }



    public class MultiplayerSynchroniser : Control
    {

        Player hostPlayer;
        Player clientPlayer;

        public override void _Ready()
        {
            hostPlayer = new Player { Id = 1 };
            clientPlayer = new Player { Id = 2 };
        }


        public override void _Process(float delta)
        {
            
        }



       

        
















    }
}

