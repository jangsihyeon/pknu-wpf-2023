using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wp08_personalInfoApp.Models;

namespace wp08_personalInfoApp.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        // View에서 사용할 멤버 변수 선언
        // 입력 쪽 변수 
        private string inFirstName;
        private string inLastName;
        private string inEmail;
        private DateTime inDate = DateTime.Now;

        // 결과 출력 쪽 변수 
        private string outFirstName;
        private string outLastName;
        private string outEmail;
        private string outDate;  // 생일 날짜는 출력할 때 문자열 대체 
        private string outAdult;
        private string outBirthday;
        private string outZodiac;

        // 일이 많아짐. 실제로 사용할 속성 
        public string InFirstName 
        { 
            get => inFirstName; 
            set 
            {
                inFirstName = value;
                RaisePropertyChanged(nameof(InFirstName));  // InFirstName 이라는 문자열이 들어가서 날려줌 
            }
         }

        public string InLastName 
        { 
            get => inLastName;
            set
            {
                inLastName = value;
                RaisePropertyChanged(nameof(InLastName)); 
            }
        }

        public string InEmail 
        { 
            get => inEmail; 
            set
            {
                inEmail = value;
                RaisePropertyChanged(nameof(InEmail));
            }
        }

        public DateTime InDate
        { 
            get => inDate; 
            set
            {
                inDate = value;
                RaisePropertyChanged(nameof(InDate));
            }
        }

        // 출력을 위한 속성들 

        public string OutFirstName 
        {
            get => outFirstName; 
            set
            {
               outFirstName = value;
                RaisePropertyChanged(nameof(OutFirstName));
            }
        }

        public string OutLastName
        { 
            get => outLastName;
            set
            {
                outLastName = value;
                RaisePropertyChanged(nameof(OutLastName));
            }
        }

        public string OutEmail
        { 
            get => outEmail;
            set
            {
                outEmail = value;
                RaisePropertyChanged(nameof(OutEmail));
            }
        }

        public string OutDate
        {
            get => outDate;
            set
            {
                outDate = value;
                RaisePropertyChanged(nameof(OutDate));
            }
        }

        public string OutAdult 
        { 
            get => outAdult;
            set
            {
                outAdult = value;
                RaisePropertyChanged(nameof(OutAdult));
            }
        }

        public string OutBirthday
        {
            get => outBirthday;
            set
            {
                outBirthday = value;
                RaisePropertyChanged(nameof(OutBirthday));
            }
        }

        public string OutZodiac 
        {
            get => outZodiac;
            set
            {
                outZodiac = value;
                RaisePropertyChanged(nameof(OutZodiac));
            }
        }

        // 버튼 클릭에 대한 이벤트 처리(명령)
        private ICommand proceedCommand;
        public ICommand ProceedCommand
        {
            get
            {
                return proceedCommand ?? (proceedCommand = new RelayCommand<object>(
                    o => Proceed(), o => !string.IsNullOrEmpty(inFirstName) &&
                                                      !string.IsNullOrEmpty(inLastName) &&
                                                      !string.IsNullOrEmpty(inEmail) &&
                                                      !string.IsNullOrEmpty(inDate.ToString())));
            }
        }

        // 버튼 클릭시 실제로 처리를 수행하는 메서드
        private void Proceed()
        {
            try
            {
                Person person = new Person(inFirstName, inLastName, inEmail, inDate);

                OutFirstName = person.Firstname;
                OutLastName = person.Lastname;
                OutEmail = person.Email;
                OutDate = person.Date.ToString("yyy년 MM월 dd일");
                OutAdult = person.IsAdult == true ? "성년" : "미성년";
                OutBirthday = person.IsBirthday == true ? "생일" : "-";
                OutZodiac = person.Zodiac;
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"예외 발생 {ex.Message}");
            }
        }
    }
}
