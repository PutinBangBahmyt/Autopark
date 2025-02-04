using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Autopark.Model;
using Autopark.View;
using Autopark.ViewModel;

namespace Autopark.ViewModel
{
    public class DeleteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string connectionString = "Data Source=localhost;Initial Catalog=UP111;Integrated Security=True";

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                _deleteCommand = value;
                OnPropertyChanged(nameof(DeleteCommand));
            }
        }

        private string _selectedTable;
        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public ObservableCollection<string> Tables { get; set; }

        public DeleteViewModel()
        {
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            LoadData();
        }

        private void Delete(object parameter)
        {
            string tableName = SelectedTable;
            string id = ID; 
            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(id))
            {
                string deleteQuery;
                // Проверяем, для какой таблицы нужно сформировать имя столбца и присваиваем соответствующее значение переменной idColumnName
                switch (tableName)
                {
                    case "Марки": deleteQuery = $"DELETE FROM Марки WHERE ID_Марки = {id}"; break;
                    case "Типы_Топлива": deleteQuery = $"DELETE FROM Типы_топлива WHERE ID_Топлива = {id}"; break;
                    case "Цвет": deleteQuery = $"DELETE FROM Цвет WHERE ID_Цвета = {id}"; break;
                    case "Пользователи": deleteQuery = $"DELETE FROM Пользователи WHERE Логин = {id}"; break;
                    case "Аренда": deleteQuery = $"DELETE FROM Аренда WHERE ID_Аренды = {id}"; break;
                    case "Возврат": deleteQuery = $"DELETE FROM Возврат WHERE ID_Возврата = {id}"; break;
                    case "Модели": deleteQuery = $"DELETE FROM Модели WHERE ID_Модели = {id}"; break;
                    case "Автомобили": deleteQuery = $"DELETE FROM Автомобили WHERE Номер_авто = {id}"; break;
                    case "Штрафы": deleteQuery = $"DELETE FROM Штрафы WHERE ID_Штрафа = {id}"; break;

                    default:
                        throw new InvalidOperationException("Неизвестная таблица");
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно удалена.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }

                // Удаление связанных записей из дочерних таблиц
                DeleteFromChildTables(tableName, id);
            }
        }


        private void DeleteFromChildTables(string tableName, string id)
        {
            switch (tableName)
            {
                case "Марки":
                    DeleteFromChildTable("Модели", "ID_Марки", id);
                    break;
                case "Типы_топлива":
                    DeleteFromChildTable("Модели", "ID_Топлива", id);
                    break;
                case "Цвет":
                    DeleteFromChildTable("Автомобили", "ID_Цвета", id);
                    break;
                case "Сервисные_Центры":
                    DeleteFromChildTable("Сервисное_обслуживание", "ID_Центра", id);
                    break;
                case "Модели":
                    DeleteFromChildTable("Автомобили", "ID_Модели", id);
                    break;
                case "Автомобили":
                    DeleteFromChildTable("Сервисное_осблуживание", "Номер_авто", id);
                    DeleteFromChildTable("Прокат", "Номер_авто", id);
                    break;
                case "Прокат":
                    DeleteFromChildTable("Возврат", "ID_Аренды", id);
                    DeleteFromChildTable("Штрафы", "ID_Аренды", id);
                    break;
                case "Пользователи":
                    DeleteFromChildTable("Прокат", "Логин", id);
                    break;
            }
        }


        private void DeleteFromChildTable(string childTableName, string foreignKeyColumnName, string id)
        {
            string deleteQuery = $"DELETE FROM {childTableName} WHERE {foreignKeyColumnName} = {id}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private bool CanDelete(object parameter)
        {
            return true; // Всегда разрешать удаление
        }

        private void LoadData()
        {
            Tables = new ObservableCollection<string> { "Марки", "Типы_топлива", "Цвет", "Модели", "Автомобили", "Пользователи", "Аренда", "Возврат", "Штрафы" };
        }
    }
}
