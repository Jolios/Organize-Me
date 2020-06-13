using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organize_Me
{
    class Childd
    {
        public String FirstName { get; set; }
        public String SchoolName { get; set; }
        public String EduLvl { get; set; }
        public String Gendder { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Home_School_Distance { get; set; }

        public Childd(string firstName, string schoolName, string eduLvl, string gender, string lastName, DateTime birthDate, int home_School_Distance)
        {
            FirstName = firstName;
            SchoolName = schoolName;
            EduLvl = eduLvl;
            Gendder = gender;
            LastName = lastName;
            BirthDate = birthDate;
            Home_School_Distance = home_School_Distance;
        }
    }
}
