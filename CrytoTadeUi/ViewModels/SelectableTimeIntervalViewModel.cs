using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CrytoTadeUi.ViewModels
{
    public class SelectableTimeIntervalViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<TimeIntervalItem> TimeIntervals { get; set; }

        private ObservableCollection<string> selectedIntervals;
        public ObservableCollection<string> SelectedIntervals
        {
            get => selectedIntervals;
            private set
            {
                if (selectedIntervals != value)
                {
                    selectedIntervals = value;
                    OnPropertyChanged(nameof(SelectedIntervals));
                }
            }
        }

        private string selectedInterval;
        public string SelectedInterval
        {
            get => selectedInterval;
            set
            {
                if (selectedInterval != value)
                {
                    selectedInterval = value;
                    OnPropertyChanged(nameof(SelectedInterval));
                }
            }
        }


        public SelectableTimeIntervalViewModel()
        {
            TimeIntervals = Enum.GetValues(typeof(TimeInterval))
                                .Cast<TimeInterval>()
                                .Select(e => new TimeIntervalItem
                                {
                                    Interval = GetEnumDescription(e),
                                    IsSelected = false
                                }).ToList();

            foreach (var interval in TimeIntervals)
            {
                interval.PropertyChanged += Interval_PropertyChanged;
            }

            SelectedIntervals = new ObservableCollection<string>();

        }

        private void Interval_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TimeIntervalItem.IsSelected))
            {
                var selectedInterval = sender as TimeIntervalItem;
                if (selectedInterval != null)
                {
                    if (selectedInterval.IsSelected)
                    {
                        SelectedInterval = selectedInterval.Interval;
                        if (!SelectedIntervals.Contains(selectedInterval.Interval))
                        {
                            SelectedIntervals.Add(selectedInterval.Interval);
                        }
                    }
                    else
                    {
                        if (SelectedIntervals.Contains(selectedInterval.Interval))
                        {
                            SelectedIntervals.Remove(selectedInterval.Interval);
                        }
                    }
                }
            }
        }



        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TimeIntervalItem : INotifyPropertyChanged
    {
        private bool isSelected;

        public string Interval { get; set; }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
