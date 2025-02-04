using Autopark.Model;
using Autopark.View;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Linq;

namespace Autopark.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ILoginService _loginService;
        private IStatusCheck _statusCheck;
        public ICommand LoginCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public ICommand ReportWCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand AddWCommand { get; set; }
        public ICommand UpdateWCommand { get; set; }
        public ICommand DeleteWCommand { get; set; }
        public ICommand UpdateTable { get; set; }
        public string ErrorMessage { get; set; }
        private object _selectedTab;
        public object SelectedTab 
        {
            get { return _selectedTab; }
            set 
            {
                _selectedTab = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTab)));
            }
        }

        private ObservableCollection<Login1> _users;
        public ObservableCollection<Login1> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }
        private ObservableCollection<Status1> _status;
        public ObservableCollection<Status1> Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        private ObservableCollection<Marks1> _marks;
        public ObservableCollection<Marks1> Marks
        {
            get { return _marks; }
            set
            {
                _marks = value;
                OnPropertyChanged("Marks");
            }
        }
        private ObservableCollection<Models1> _models;
        public ObservableCollection<Models1> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                OnPropertyChanged("Models");
            }
        }
        private ObservableCollection<Cars1> _cars;
        public ObservableCollection<Cars1> Cars
        {
            get { return _cars; }
            set
            {
                _cars = value;
                OnPropertyChanged("Cars");
            }
        }
        private ObservableCollection<Fuel1> _fuel;
        public ObservableCollection<Fuel1> Fuel
        {
            get { return _fuel; }
            set
            {
                _fuel = value;
                OnPropertyChanged("Fuel");
            }
        }
        private ObservableCollection<Paints1> _paints;
        public ObservableCollection<Paints1> Paints
        {
            get { return _paints; }
            set
            {
                _paints = value;
                OnPropertyChanged("Paints");
            }
        }
        private ObservableCollection<Rent1> _rent;
        public ObservableCollection<Rent1> Rent
        {
            get { return _rent; }
            set
            {
                _rent = value;
                OnPropertyChanged("Rent");
            }
        }
        private ObservableCollection<Vozvrat1> _vozvrat;
        public ObservableCollection<Vozvrat1> Vozvrat
        {
            get { return _vozvrat; }
            set
            {
                _vozvrat = value;
                OnPropertyChanged("Vozvrat");
            }
        }
        private ObservableCollection<Fine1> _fine;
        public ObservableCollection<Fine1> Fine
        {
            get { return _fine; }
            set
            {
                _fine = value;
                OnPropertyChanged("Fine");
            }
        }
        private UserBusinessLogic _userBusinessLogic;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private string _login1;
        public string Login1
        {
            get { return _login1; }
            set
            {
                _login1 = value;
                OnPropertyChanged("Login1");
            }
        }
        public MainWindowViewModel(ILoginService loginService, IStatusCheck statusCheck)
        {
            _loginService = loginService;
            _statusCheck = statusCheck;
            _userBusinessLogic = new UserBusinessLogic();
            LoginCommand = new RelayCommand(Login);
            ShowCommand = new RelayCommand(Show);
            ReportWCommand = new RelayCommand(ReportWindow);
            ReportCommand = new RelayCommand(Report);
            AddWCommand = new RelayCommand(AddWindow);
            UpdateWCommand = new RelayCommand(UpdateWindow);
            DeleteWCommand = new RelayCommand(DeleteWindow);
            UpdateTable = new RelayCommand(Update);
            Users = _userBusinessLogic.LoadUsers();
            Status = _userBusinessLogic.LoadStatus();
            Marks = _userBusinessLogic.LoadMarks();
            Models = _userBusinessLogic.LoadModels();
            Cars = _userBusinessLogic.LoadCars();
            Fuel = _userBusinessLogic.LoadFuel();
            Paints = _userBusinessLogic.LoadPaints();
            Rent = _userBusinessLogic.LoadRent();
            Vozvrat = _userBusinessLogic.LoadVozvrat();
            Fine = _userBusinessLogic.LoadFine();
        }
        private void Login(object obj)
        {
            if (_loginService.ValidateUser(UserName, Password))
            {
                MessageBox.Show("Пароль верный");
                ErrorMessage = "";
                OnPropertyChanged("ErrorMessage");
                if (_statusCheck.ValidateUser(UserName) == 1)
                {
                    AdminWindow Database = new AdminWindow();
                    Database.DataContext = this;
                    Database.Show();
                    System.Windows.Application.Current.Windows[0].Close();
                }
                else 
                {
                    MessageBox.Show("Ну ты лошара братан");
                }
            }
            else
            {
                ErrorMessage = "Wrong login or password";
                OnPropertyChanged("ErrorMessage");
            }
        }
        private void Update(object obj) 
        {
            Users = _userBusinessLogic.LoadUsers();
            Status = _userBusinessLogic.LoadStatus();
            Marks = _userBusinessLogic.LoadMarks();
            Models = _userBusinessLogic.LoadModels();
            Cars = _userBusinessLogic.LoadCars();
            Fuel = _userBusinessLogic.LoadFuel();
            Paints = _userBusinessLogic.LoadPaints();
            Rent = _userBusinessLogic.LoadRent();
            Vozvrat = _userBusinessLogic.LoadVozvrat();
            Fine = _userBusinessLogic.LoadFine();
        }
        private void Show(object parameter) 
        {
            SelectedTab = parameter;
        }
        private void ReportWindow(object obj) 
        {
            ReportWindow report = new ReportWindow();
            report.DataContext = this;
            report.Show();
        }
        private void Report(object obj) 
        {
            string Doc = @"F:\testReport\d.docx";
            string connectionString = "Data Source=localhost;Initial Catalog=UP111;Integrated Security=True";
            string login = Login1;
            string Gos_nomer = "";
            string lastname = "";
            string name = "";
            string Data_start = "";
            string Data_end = "";
            string Price = "";
            string color = "";
            string model = "";
            string marka = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Пользователи.Фамилия, Пользователи.Имя, Прокат.Дата_Аренды, Возврат.Дата_возврата, Возврат.Стоимость, Автомобили.Номер_Автомобиля, Цвет.Цвет, Марки.Название, Модели.Модель FROM Пользователи JOIN Прокат ON Пользователи.Логин = Прокат.Логин JOIN Возврат ON Прокат.ID_Проката = Возврат.ID_Аренды JOIN Автомобили on Автомобили.Номер_Автомобиля = Прокат.Номер_Автомобиля JOIN Цвет on Цвет.id_Цвета = Автомобили.ID_Цвета JOIN Модели on Автомобили.ID_Модели = Модели.ID_Модели JOIN Марки on Модели.ID_Марки = Марки.ID_Марки WHERE Пользователи.Логин = @login", connection);
                command.Parameters.AddWithValue("@login", login);


                var wordApp = new Microsoft.Office.Interop.Word.Application
                {
                    Visible = false
                };
                using (var Reader = command.ExecuteReader())
                {
                    if (Reader.HasRows)
                    {
                        if (Reader.Read())
                        {

                            lastname = Reader[0].ToString();
                            name = Reader[1].ToString();
                            Data_start = Reader[2].ToString();
                            Data_end = Reader[3].ToString();
                            Price = Reader[4].ToString();
                            Gos_nomer = Reader[5].ToString();
                            color = Reader[6].ToString();
                            marka = Reader[7].ToString();
                            model = Reader[8].ToString();
                        }
                    }
                }
                try
                {
                    var wordDoc = wordApp.Documents.Open(Doc);
                    ReplaceSubS("[Ф]", lastname, wordDoc);
                    ReplaceSubS("[И]", name, wordDoc);
                    ReplaceSubS("[Начало]", Data_start, wordDoc);
                    ReplaceSubS("[Конец]", Data_end, wordDoc);
                    ReplaceSubS("[Стоимость]", Price, wordDoc);
                    ReplaceSubS("[ГосНомер]", Gos_nomer, wordDoc);
                    ReplaceSubS("[Цвет]", color, wordDoc);
                    ReplaceSubS("[Марка]", marka, wordDoc);
                    ReplaceSubS("[Модель]", model, wordDoc);


                    wordDoc.SaveAs("F:\\testReport\\test");
                    wordApp.Visible = true;
                }
                catch
                {

                }
            }

        }
        private void AddWindow(object obj)
        {
            AddWindow add = new AddWindow();
            add.DataContext = new AddViewModel();
            add.Show();
        }
        private void UpdateWindow(object obj)
        {
            UpdateWindow Update = new UpdateWindow();
            Update.DataContext = new UpdateViewModel();
            Update.Show();
        }
        private void DeleteWindow(object obj)
        {
            DeleteWindow delete = new DeleteWindow();
            delete.DataContext = new DeleteViewModel();
            delete.Show();
        }
        private void ReplaceSubS(string stubToReplace, string text, Microsoft.Office.Interop.Word.Document document)
        {
            var range = document.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text, Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
