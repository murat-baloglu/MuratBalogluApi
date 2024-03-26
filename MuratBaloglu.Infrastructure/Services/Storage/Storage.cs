using MuratBaloglu.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuratBaloglu.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected string RenameFileName(string fileName)
        {
            fileName = fileName.ToLower();

            //string uniqValue = $"{DateTime.Now.ToString("yyyyMMdd")}{DateTime.Now.Millisecond}";
            string uniqValue = DateTime.Now.Millisecond.ToString();
            string extension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string newFileName = $"{NameRegulatoryOperation.RegulateCharacters(fileNameWithoutExtension)}-{uniqValue}{extension}";

            return newFileName;
        }
    }
}
