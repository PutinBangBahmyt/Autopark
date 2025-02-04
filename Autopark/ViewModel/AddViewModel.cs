using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Autopark.ViewModel
{
    public class AddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly string _connectionString = "Data Source=localhost;Initial Catalog=UP111;Integrated Security=True";
        private string _selectedTable;
        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                UpdateSelectedTableFields();
                OnPropertyChanged("SelectedTable");
            }
        }

        private DataRow _selectedDataItem;
        public DataRow SelectedDataItem
        {
            get { return _selectedDataItem; }
            set
            {
                _selectedDataItem = value;
                OnPropertyChanged("SelectedDataItem");
            }
        }

        private ObservableCollection<FieldViewModel> _selectedTableFields;
        public ObservableCollection<FieldViewModel> SelectedTableFields
        {
            get { return _selectedTableFields; }
            set
            {
                _selectedTableFields = value;
                OnPropertyChanged("SelectedTableFields");
            }
        }
        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(param => AddData());
                }
                return _addCommand;
            }
        }


        private ObservableCollection<DataRow> _tableData;
        public ObservableCollection<DataRow> TableData
        {
            get { return _tableData; }
            set
            {
                _tableData = value;
                OnPropertyChanged("TableData");
            }
        }
        private ObservableCollection<string> _tables;
        public ObservableCollection<string> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                OnPropertyChanged(nameof(Tables));
            }
        }
        public AddViewModel()
        {
            // Здесь нужно заполнить список доступных таблиц
            Tables = new ObservableCollection<string> { "Марки", "Типы_топлива", "Цвет", "Модели", "Автомобили", "Пользователи", "Прокат", "Возврат", "Штрафы"};
        }
        private void UpdateSelectedTableFields()
        {
            SelectedTableFields = new ObservableCollection<FieldViewModel>();

            if (SelectedTable == "Марки")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Название: ", Value = "" });
            }
            else if (SelectedTable == "Типы_топлива")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Тип: ", Value = "" });
            }
            else if (SelectedTable == "Цвет")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Цвет: ", Value = "" });
            }
            else if (SelectedTable == "Модели")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Модель: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Объём топлива: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Количество мест: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Год выпуска: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "ID Марки: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "ID Топлива: ", Value = "" });
            }
            else if (SelectedTable == "Автомобили")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Номер авто: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "ID модели: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "ID цвета: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Тариф за минуту: ", Value = "" });
            }
            else if (SelectedTable == "Пользователи")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Логин: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Пароль: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Имя: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Фамилия: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Отчество: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Дата рождения: ", Value = "" });
            }
            else if (SelectedTable == "Прокат")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Номер автомобиля: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Логин: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Дата аренды: ", Value = "" });
            }
            else if (SelectedTable == "Возврат")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "Дата возврата: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "ID аренды: ", Value = "" });
            }
            else if (SelectedTable == "Штрафы")
            {
                SelectedTableFields.Add(new FieldViewModel { Label = "ID_проката: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Название_штрафа: ", Value = "" });
                SelectedTableFields.Add(new FieldViewModel { Label = "Стоимость: ", Value = "" });
            }
        }


        private void AddData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "";
                    // В зависимости от выбранной таблицы формируем соответствующий запрос SQL
                    if (SelectedTable == "Марки")
                    {
                        query = "INSERT INTO Марки (Название) VALUES (@Name)";
                    }
                    else if (SelectedTable == "Типы топлива")
                    {
                        query = "INSERT INTO Типы_топлива (Тип) VALUES (@Name)";
                    }
                    else if (SelectedTable == "Цвет")
                    {
                        query = "INSERT INTO Цвет (Цвет) VALUES '@Name)";
                    }
                    else if (SelectedTable == "Модели")
                    {
                        query = "INSERT INTO Модели (Модель,[Объём топлива(л.)],[Количество мест], Год_выпуска,ID_Марки,ID_Топлива) VALUES (@Name,@VolFuel,@mesta,@Year,@ID_Marki,@ID_Fuel)";
                    }
                    else if (SelectedTable == "Автомобили")
                    {
                        query = "INSERT INTO Автомобили (Номер_авто, ID_Модели, ID_Цвета, [Тариф(за минуту)]) VALUES (@Nomer, @ID_model, @ID_color, @Tarif)";
                    }
                    else if (SelectedTable == "Пользователи")
                    {
                        query = "INSERT INTO Пользователи (Логин, Пароль, Имя, Фамилия, Отчество, Дата_рождения, ID_Статуса) VALUES (@Login, @Password, @FirstName, @LastName, @MiddleName, @DateOfBirth, 2)";
                    }
                    else if (SelectedTable == "Прокат")
                    {
                        query = "INSERT INTO Прокат (Номер_авто,Логин,Дата_аренды) VALUES (@Nomer,@Login,@Date)";
                    }
                    else if (SelectedTable == "Возврат")
                    {
                        query = "INSERT INTO Возврат (Дата_возврата, ID_Аренды) VALUES (@Date, @ID)";
                    }
                    else if (SelectedTable == "Штрафы")
                    {
                        query = "INSERT INTO Штрафы (ID_проката, Наименование_Штрафа, Стоимость_Штрафа) VALUES (@ID,@Name,@Cost)";
                    }
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавляем параметры
                        command.Parameters.AddWithValue("@Name", SelectedTableFields[0].Value); // Предполагаем, что в первом поле содержится значение для добавления

                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // В случае успешного выполнения
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные успешно добавлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить данные");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class FieldViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
