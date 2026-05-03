using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaskManagerDashboard_WPF.Model
{
    public class TaskItem : INotifyPropertyChanged
    {
        public string? Title { get; set; }
        public string? Priority { get; set; }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }
        public string TextToDisplay
        {
            get
            {
                return $" {Title} ( {Priority} )";
            }
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}