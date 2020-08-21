using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TestDownloadFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string downloadFolder = @"C:\downloads\";

            //формирую строку post-запроса
            Console.WriteLine("Укажите имя файла:");
            string filename = Console.ReadLine();
            string password = "pass";
            string data = "password=" + password + "&" + "filename=" + filename;
            HttpWebRequest request = HttpWebRequest.CreateHttp("http://bim-starter.com/downloadfile.php");
            request.Method = "POST";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            //отправляю web запрос
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            //получаю ответ
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Error response");
                    return;
                }

                //headers - это "сопроводительная документация" к запросу, из неё можно получить например имя файла
                Console.WriteLine("Headers:");
                WebHeaderCollection headers = response.Headers;
                for (int i = 0; i < headers.Count; ++i)
                {
                    string key = headers.Keys[i];
                    string value = headers[i];
                    Console.WriteLine(key + ": " +  value);
                }
                string checkFilename = headers["Content-Disposition"].Split('=').Last();
                if(filename != checkFilename)
                {
                    Console.WriteLine("Filenames isnt equals");
                    return;
                }

                string pathToSave = Path.Combine(downloadFolder, checkFilename);

                //сохраняю файл через байт-буфер и FileStream
                byte[] buffer = new byte[1024];
                int read;
                using (FileStream download = new FileStream(pathToSave, FileMode.Create))
                {
                    Stream stream = response.GetResponseStream();
                    while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        download.Write(buffer, 0, read);
                    }
                }
            }
            Console.WriteLine("Запрос выполнен...");
            Console.ReadKey();
        }
    }
}

