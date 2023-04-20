using System.Windows.Media;

namespace wp05_bikeshop.Logics
{
    internal class Car : Notifier // 값이 바뀌는 것을 인지하겠다
    {
        private string names;
        public string Name{
            get => names;
            // 프로퍼티를 변경
            set
            {
                names = value;
                OnPropertyChanged("Names");   // Names 프로퍼티가 바뀌었어요 
            }
        }

        private double speed;
        public double Speed{
            get => speed; 
            set
            {
                speed = value;
                OnPropertyChanged(nameof(Speed));   // == "Speed"
            }
                }
        public Color Colorz {get; set;}
        public Human Driver {get; set;} // auto property 
    }
}