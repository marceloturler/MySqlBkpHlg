using System;
using System.IO;
using MySql.Data.MySqlClient;
using Ionic.Zip;

namespace MySqlBkpHlg
{
    class Program
    {
        static void Main(string[] args)
        {
            string connx = "server=51.161.34.153;user=hlg_portal;pwd=&hO45n8m;database=hlg_portal;" +
                "charset=utf8;convertzerodatetime=true;default command timeout=0;";

            string hlgBkp = "C:\\backups\\hlgBkp.sql";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connx))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(hlgBkp);
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                var pathToZip = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"C:\\backups"));
                var fileNameToZip = Directory.GetFiles(pathToZip);
                foreach (var file in fileNameToZip)
                {
                    ZipFile(file, pathToZip);
                }
            }
        }

        private static void ZipFile(string file, string pathToZip)
        {
           using (ZipFile zip = new ZipFile())
           { 
                if (File.Exists(file))
                {
                    try
                    {
                        zip.AddFile(file, pathToZip);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro Compactação: " + ex.Message);
                    }
                    finally
                    {
                        zip.Save(file + ".zip");
                    }
                }
            }
        }
    }
}
