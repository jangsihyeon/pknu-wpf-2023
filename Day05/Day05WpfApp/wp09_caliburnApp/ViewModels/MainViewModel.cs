using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wp09_caliburnApp.Models;

namespace wp09_caliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        private string fisrtName = "Syeon";
        public string FirstName {
            get => fisrtName;
            set
            {
                fisrtName = value;
                NotifyOfPropertyChange(() => FirstName);  // 속성값이 변경된걸 시스템에 알려주는 역할 
                NotifyOfPropertyChange(nameof(CanClearName));  
                NotifyOfPropertyChange(nameof(FullName));
            }
        }

        private string lastName = "Jang";

        public string LastName
        {
            get => lastName;
            set 
            {
                lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(nameof(CanClearName));
                NotifyOfPropertyChange(nameof(FullName));  // 변화 통보 
            }
        }

        public string FullName 
        {
            get => $"{LastName} {FirstName}";
        }

        // 콤보박스에 바인딩 할 속성 
        // 이런곳에서는 var를 쓸 수 없음 
        private BindableCollection<Person> managers = new BindableCollection<Person>();

        public BindableCollection<Person> Managers
        {
            get => managers;
            set => managers = value;
        }

        // 콤보박스에 선택된 값을 지정할 속성
        private Person selectedManager;

        public Person SelectedManager
        {
            get => selectedManager;
            set
            {
                selectedManager = value;
                LastName = selectedManager.LastName;
                FirstName= selectedManager.FirstName;
                NotifyOfPropertyChange(nameof(selectedManager));
            }
        }

        public MainViewModel()
        {
            // DB를 사용하면 여기서 DB 접속 => 데이터 Select 까지 ....
            Managers.Add(new Person { FirstName = "Joey", LastName= "Tribbiani" });
            Managers.Add(new Person { FirstName = "Phoebe", LastName= "Buffay" });
            Managers.Add(new Person { FirstName = "Rachel", LastName="Green"});
            Managers.Add(new Person { FirstName = "Monica", LastName="Geller"});
        }

        public void ClearName()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public bool CanClearName
        {
            get => !(string.IsNullOrEmpty(fisrtName) && string.IsNullOrEmpty(lastName));
        }
    }
}
