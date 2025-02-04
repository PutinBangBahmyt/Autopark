using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Autopark.ViewModel
{
    public class UpdateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedTable;
        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                LoadTableData();
                OnPropertyChanged("SelectedTable");
            }
        }

        private ObservableCollection<string> _tables;
        public ObservableCollection<string> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                OnPropertyChanged("Tables");
            }
        }

        private DataTable _tableData;
        public DataTable TableData
        {
            get { return _tableData; }
            set
            {
                _tableData = value;
                OnPropertyChanged("TableData");
            }
        }

        public ICommand UpdateCommand { get; private set; }

        public UpdateViewModel()
        {
            UpdateCommand = new RelayCommand(param => UpdateData(), param => CanUpdate());

            // Здесь вы можете добавить логику для заполнения списка таблиц
            Tables = new ObservableCollection<string> { "Марки", "Типы_топлива", "Цвет", "Модели", "Автомобили", "Пользователи", "Аренда", "Возврат", "Штрафы"};
        }

        private void LoadTableData()
        {
            if (!string.IsNullOrEmpty(SelectedTable))
            {
                try
                {
                    string connectionString = "Data Source=localhost;Initial Catalog=UP111;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"SELECT * FROM {SelectedTable}";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            TableData = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private bool CanUpdate()
        {
            // Добавьте здесь логику для определения, когда кнопка обновления должна быть доступна
            return true;
        }

        private void UpdateData()
        {
            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=UP111;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Для каждой строки в таблице проверяем, были ли изменены какие-либо столбцы
                    foreach (DataRow row in TableData.Rows)
                    {
                        // Формируем запрос UPDATE динамически на основе выбранной таблицы и изменённых столбцов
                        string updateQuery = $"UPDATE {SelectedTable} SET ";

                        List<string> updateColumns = new List<string>();
                        List<string> whereConditions = new List<string>();

                        foreach (DataColumn column in TableData.Columns)
                        {
                            // Если значение в текущей ячейке отличается от оригинального значения в базе данных,
                            // добавляем столбец для обновления
                            if (!row[column.ColumnName, DataRowVersion.Current].Equals(row[column.ColumnName, DataRowVersion.Original]))
                            {
                                updateColumns.Add($"{column.ColumnName} = @{column.ColumnName}");
                            }
                        }

                        // Если есть столбцы для обновления, формируем запрос UPDATE
                        if (updateColumns.Count > 0)
                        {
                            updateQuery += string.Join(", ", updateColumns);

                            // Определяем имя столбца идентификатора строки для текущей таблицы
                            string idColumnName;
                            switch (SelectedTable)
                            {
                                case "Марки": idColumnName = "ID_Марки"; break;
                                case "Типы_Топлива": idColumnName = "ID_Топлива"; break;
                                case "Цвет": idColumnName = "ID_Цвета"; break;
                                case "Пользователи": idColumnName = "Логин"; break;
                                case "Аренда": idColumnName = "ID_Аренды"; break;
                                case "Возврат": idColumnName = "ID_Возврата"; break;
                                case "Модели": idColumnName = "ID_Модели"; break;
                                case "Автомобили": idColumnName = "Номер_авто"; break;
                                case "Штрафы": idColumnName = "ID_Штрафа"; break;
                                default:
                                    throw new InvalidOperationException("Неизвестная таблица");
                            }

                            // Добавляем условие WHERE для идентификатора строки
                            whereConditions.Add($"{idColumnName} = @{idColumnName}");
                            updateQuery += " WHERE " + string.Join(" AND ", whereConditions);

                            using (SqlCommand command = new SqlCommand(updateQuery, connection))
                            {
                                foreach (DataColumn column in TableData.Columns)
                                {
                                    // Добавляем параметры только для изменённых столбцов
                                    if (updateColumns.Contains($"{column.ColumnName} = @{column.ColumnName}"))
                                    {
                                        command.Parameters.AddWithValue($"@{column.ColumnName}", row[column.ColumnName]);
                                    }
                                }

                                // Добавляем параметр для условия WHERE (идентификатор строки)
                                command.Parameters.AddWithValue($"@{idColumnName}", row[idColumnName]);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Данные успешно обновлены в базе данных.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
