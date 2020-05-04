using StringCalculatorGraph.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorGraph.ViewModels
{
    public class StringCalculatorViewModel : INotifyPropertyChanged
    {
        private IList<Segment> _segments;

        public IList<Segment> Segments
        {
            get { return _segments; }
            set
            {
                _segments = value;
                OnPropertyChanged("Segments");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
