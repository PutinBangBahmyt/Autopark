using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Autopark.Model
{
    public class TableService
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=UP111;Integrated Security=True";
    }
    public class UserBusinessLogic
    {
        public void CreateReport1()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                string sqlQuery = "SELECT Марки.Название AS Марка," +
                    " Модели.Модель,Автомобили.Номер_Автомобиля,COUNT(Прокат.ID_Проката) " +
                    "AS [Количество аренд],SUM(Возврат.Стоимость) " +
                    "AS [Итоговая сумма] FROM Автомобили " +
                    "JOIN Модели ON Автомобили.ID_модели = Модели.ID_Модели " +
                    "JOIN Марки ON Модели.ID_Марки = Марки.ID_Марки " +
                    "JOIN Прокат ON Автомобили.Номер_Автомобиля = Прокат.Номер_Автомобиля " +
                    "JOIN Возврат ON Прокат.ID_Проката = Возврат.ID_Аренды " +
                    "GROUP BY " +
                    "Марки.Название, Модели.Модель, Автомобили.Номер_Автомобиля " +
                    "ORDER BY [Итоговая сумма] DESC";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Создание нового Excel-файла и заполнение его данными из SQL-запроса
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    // Добавление надписей в верхнюю строку
                    worksheet.Cells[1, 1].Value = "Марка";
                    worksheet.Cells[1, 2].Value = "Модель";
                    worksheet.Cells[1, 3].Value = "Номер авто";
                    worksheet.Cells[1, 4].Value = "Количество взятий в аренду";
                    worksheet.Cells[1, 5].Value = "Итоговая сумма за все аренды";

                    // Начало чтения с второй строки
                    int row = 2;
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            worksheet.Cells[row, i + 1].Value = reader.GetValue(i);
                        }
                        row++;
                    }

                    // Сохранение Excel-файла
                    string filePath = "F:\\testReport\\report.xlsx";
                    package.SaveAs(new FileInfo(filePath));

                    // Печать Excel-файла
                    PrintExcelFile(filePath);
                }
            }
        }

        public void PrintExcelFile(string filePath)
        {
            // Создание нового Excel-приложения
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Открытие Excel-файла
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);

            // Печать Excel-файла
            workbook.PrintOutEx();

            // Закрытие Excel-приложения
            workbook.Close(false);
            excelApp.Quit();
        }


        public ObservableCollection<Login1> LoadUsers()
        {
            var users = new ObservableCollection<Login1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Пользователи ORDER BY ID_Статуса", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Login1
                            {
                                Login = reader.GetString(0),
                                Password = reader.GetString(1),
                                Name = reader.GetString(2),
                                Fam = reader.GetString(3),
                                Otch = reader.GetString(4),
                                Datebirth = reader.GetDateTime(5),
                                ID_Role = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }

            return users;
        }
        public ObservableCollection<Status1> LoadStatus()
        {
            var users = new ObservableCollection<Status1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Статус ORDER BY ID_Статуса", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Status1
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Marks1> LoadMarks()
        {
            var users = new ObservableCollection<Marks1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Марки ORDER BY ID_Марки", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Marks1
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Models1> LoadModels()
        {
            var users = new ObservableCollection<Models1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Модели ORDER BY ID_Модели", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Models1
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Volume = reader.GetInt32(2),
                                Mesta = reader.GetInt32(3),
                                Year = reader.GetInt32(4),
                                ID_mark = reader.GetInt32(5),
                                ID_fuel = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Cars1> LoadCars()
        {
            var users = new ObservableCollection<Cars1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Автомобили ORDER BY Номер_Автомобиля", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Cars1
                            {
                                Nomer = reader.GetString(0),
                                ID_model = reader.GetInt32(1),
                                ID_color = reader.GetInt32(2),
                                Tarif = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Fuel1> LoadFuel()
        {
            var users = new ObservableCollection<Fuel1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Типы_топлива ORDER BY ID_Топлива", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Fuel1
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Paints1> LoadPaints()
        {
            var users = new ObservableCollection<Paints1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Цвет ORDER BY ID_Цвета", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Paints1
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Rent1> LoadRent()
        {
            var users = new ObservableCollection<Rent1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Прокат ORDER BY ID_Проката", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Rent1
                            {
                                ID = reader.GetInt32(0),
                                Nomer = reader.GetString(1),
                                Login = reader.GetString(2),
                                RentDate = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Vozvrat1> LoadVozvrat()
        {
            var users = new ObservableCollection<Vozvrat1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Возврат ORDER BY ID_Возврата", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Vozvrat1
                            {
                                ID = reader.GetInt32(0),
                                VozvratDate = reader.GetDateTime(1),
                                ID_rent = reader.GetInt32(2),
                                Sum = reader.GetDecimal(3)
                            });
                        }
                    }
                }
            }
            return users;
        }
        public ObservableCollection<Fine1> LoadFine()
        {
            var users = new ObservableCollection<Fine1>();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=UP111;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Штрафы ORDER BY ID_Штрафа", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Fine1
                            {
                                ID = reader.GetInt32(0),
                                ID_rent = reader.GetInt32(1),
                                Name_fine = reader.GetString(2),
                                Cost_fine = reader.GetSqlMoney(3)
                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}