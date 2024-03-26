using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.Infrastructure.Operations
{
    public static class NameRegulatoryOperation
    {
        public static string RegulateCharacters(string name)
        {
            name = name
                        .Trim()
                        .Replace("\"", "")
                        .Replace("!", "")
                        .Replace("'", "")
                        .Replace("^", "")
                        .Replace("+", "")
                        .Replace("%", "")
                        .Replace("&", "")
                        .Replace("/", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("=", "")
                        .Replace("?", "")
                        .Replace("_", "")
                        .Replace(" ", "-")
                        .Replace("@", "")
                        .Replace("€", "")
                        .Replace("¨", "")
                        .Replace("~", "")
                        .Replace(",", "")
                        .Replace(";", "")
                        .Replace(":", "")
                        .Replace(".", "-")
                        .Replace("Ö", "o")
                        .Replace("ö", "o")
                        .Replace("Ü", "u")
                        .Replace("ü", "u")
                        .Replace("ı", "i")
                        .Replace("İ", "i")
                        .Replace("ğ", "g")
                        .Replace("Ğ", "g")
                        .Replace("æ", "")
                        .Replace("ß", "")
                        .Replace("â", "a")
                        .Replace("î", "i")
                        .Replace("ş", "s")
                        .Replace("Ş", "s")
                        .Replace("Ç", "c")
                        .Replace("ç", "c")
                        .Replace("<", "")
                        .Replace(">", "")
                        .Replace("|", "")
            .ToLower();

            //Sıfırıncı index ten başla (0. index dahil), toplam karakter sayısının bir eksiğine kadar olan (dahil) string ifadeyi getir. false dönene kadar bu işlemi tekrarla. 
            while (name.EndsWith("-"))
            {
                name = name.Substring(0, name.Length - 1);
            }

            return name;
        }

    }
}
