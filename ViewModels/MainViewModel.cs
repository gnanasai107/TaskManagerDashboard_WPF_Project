using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using TaskManagerDashboard_WPF.Commands;
using TaskManagerDashboard_WPF.Model;
using TaskManagerDashboard_WPF.Services;

namespace TaskManagerDashboard_WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public List<string> Priorities { get; } = new() { "Low", "Medium", "High" };

        private TaskItem _selectedTask;
        public TaskItem SelectedTask
        {
            get 
            {
                return _selectedTask;
            } 
            set 
            {
                _selectedTask = value; 
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private string _taskTitle;
        public string TaskTitle
        {

            get
            {
                return _taskTitle;
            }
            set
            {
                _taskTitle = value;
                OnPropertyChanged(nameof(TaskTitle));

                if (!string.IsNullOrWhiteSpace(_taskTitle))
                    SelectedTask = null;
            }
        }

        private string _prioritySelected;
        public string SelectedPriority
        {
            get
            {
                return _prioritySelected;
            }
            set
            {
                _prioritySelected = value;
                OnPropertyChanged(nameof(SelectedPriority));

                if (!string.IsNullOrWhiteSpace(_prioritySelected))
                    SelectedTask = null;
            }
        }
        public ICommand AddTaskCommand { get; }
        public ICommand MarkCompleteCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand ChangeFilterCommand { get; }
        public ICommand SaveTasksCommand { get; }

        private string _currentFilter = "All";
        public ICollectionView FilteredTasks { get; }

        private readonly TaskStorageService _storageService;

        public MainViewModel()
        {

            Tasks = new ObservableCollection<TaskItem>
            {
                new TaskItem
                {
                    Title = "Task of Chain Cleaning for your Bike GT 650",
                    Priority = "High",
                    IsCompleted = false
                },
                new TaskItem
                {
                    Title = "Task of Cleaning GT 650",
                    Priority = "Low",
                    IsCompleted = false
                },
                new TaskItem
                {
                    Title = "Task of Adding center stand to GT 650",
                    Priority = "High",
                    IsCompleted = false
                }
            };

            AddTaskCommand = new RCommand(_ => AddTask(),_ => !string.IsNullOrWhiteSpace(TaskTitle) && !string.IsNullOrWhiteSpace(SelectedPriority));
            DeleteTaskCommand = new RCommand(_ => DeleteTask(), _ => SelectedTask != null);
            MarkCompleteCommand = new RCommand(_ => MarkComplete(), _ => SelectedTask != null && !SelectedTask.IsCompleted);

            FilteredTasks = CollectionViewSource.GetDefaultView(Tasks);

            FilteredTasks.Filter = FilterTasks;

            ChangeFilterCommand = new RCommand(filter =>
            {
                _currentFilter = filter.ToString();
                FilteredTasks.Refresh();
            });

            _storageService = new TaskStorageService();

            SaveTasksCommand = new RCommand(
                async _ => await _storageService.SaveAsync(Tasks),
                _ => Tasks.Any());

        }

        private void AddTask()
        {
            Tasks.Add(new TaskItem
            {
                Title = TaskTitle,
                Priority = SelectedPriority,
                IsCompleted = false
            });
            TaskTitle = string.Empty;
            SelectedPriority = null;
        }
        private void MarkComplete()
        {
            SelectedTask.IsCompleted = true;
        }
        private void DeleteTask()
        {
            Tasks.Remove(SelectedTask);
        }
        private bool FilterTasks(object obj)
        {
            if (obj is not TaskItem task)
                return false;

            switch (_currentFilter)
            {
                case "Completed":
                    return task.IsCompleted;
                case "Pending":
                    return !task.IsCompleted;
                default:
                    return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}