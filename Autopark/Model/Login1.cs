using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autopark.Model
{
    public class Login1
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Fam { get; set; }
        public string Otch { get; set; }
        public DateTime Datebirth { get; set; }
        public int ID_Role { get; set; }
    }
    public class Status1
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Marks1
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Models1
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int Mesta { get; set; }
        public int Year{ get; set; }
        public int ID_mark { get; set; }
        public int ID_fuel { get; set; }
    }
    public class Cars1
    {
        public string Nomer { get; set; }
        public int ID_model { get; set; }
        public int ID_color { get; set; }
        public int Tarif { get; set; }
    }
    public class Fuel1
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Paints1
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Rent1 
    {
        public int ID { get; set; }
        public string Nomer { get; set; }
        public string Login { get; set; }
        public DateTime RentDate { get; set; }
    }
    public class Vozvrat1
    {
        public int ID { get; set; }
        public DateTime VozvratDate { get; set; }
        public int ID_rent {  get; set; }
        public Decimal Sum { get; set; }
    }
    public class Fine1
    {
        public int ID { get; set; }
        public int ID_rent { get; set; }
        public string Name_fine { get; set; }
        public SqlMoney Cost_fine { get; set; }
    }
}
