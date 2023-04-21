using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp08_personalInfoApp.ViewModels
{
    // 프로퍼티가 변경 되면 시스템에 알려주는 역할 
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void RaisePropertyChanged(string propertyNames)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyNames));
            }
        }
    }
}
