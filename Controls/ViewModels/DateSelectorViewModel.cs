using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    public partial class DateSelectorViewModel : ObservableObject
    {
        #region 属性字段

        // 当前选中的日期
        [ObservableProperty]
        private DateTime _selectedDate;

        // 当前界面上显示的第一天（基准日期）
        private DateTime _startDate;

        // 右侧显示的完整日期文本，例如 "10月3日 周六"
        [ObservableProperty]
        private string _currentDateText; // "10月3日 周六"

        // 日期列表集合
        public ObservableCollection<DayItem> Days { get; } = new();
        #endregion

        public DateSelectorViewModel()
        {
            SelectedDate= DateTime.Today;
            _startDate = SelectedDate.AddDays(-3);// 默认显示当前日期的前后各三天
            GenerateDays();
        }

        

        #region 方法

        /// <summary>
        /// 根据 _startDate 生成 7 天的数据集合
        /// </summary>
        private void GenerateDays()
        {
            Days.Clear();
            for (int i = 0; i < 7; i++)
            {
                var date = _startDate.AddDays(i);
                Days.Add(new DayItem
                {
                    Date = date,
                    DayText = date.Day.ToString("D2"),
                    IsSelected = (date.Date == SelectedDate.Date)
                });
            }
        }

        /// <summary>
        /// 更新右侧文本显示，例如 "10月3日 周六"
        /// </summary>
        private void UpdateCurrentDateText()
        {
            // 使用中文本地化格式化星期
            string weekDay = SelectedDate.ToString("dddd", new CultureInfo("zh-CN"));
            CurrentDateText = $"{SelectedDate.Month}月{SelectedDate.Day}日 {weekDay}";
        }


        /// <summary>
        /// 每一次 SelectedDate 改变时，都会触发这个方法来更新右侧的文本显示
        /// </summary>
        /// <param name="value"></param>
        partial void OnSelectedDateChanged(DateTime value)
        {
            UpdateCurrentDateText();
        }

        #endregion

        #region Command

        [RelayCommand]
        private void SelectDay(DayItem day)
        {
            if (day == null) return;

            SelectedDate = day.Date;

            // 更新所有日期的选中状态
            foreach (var item in Days)
            {
                item.IsSelected = (item.Date.Date == SelectedDate.Date);
            }

            // TODO: 这里可以触发事件，通知右侧的“今日日程”刷新数据
            // e.g., EventAggregator.GetEvent<DateChangedEvent>().Publish(SelectedDate);
        }

        /// <summary>
        /// 跳转到前一周
        /// </summary>
        [RelayCommand]
        private void PreviousWeek()
        {
            SelectedDate=SelectedDate.AddDays(-7); // 更新选中日期为前一周的同一天
            _startDate = _startDate.AddDays(-7); // 更新起始日期为前一周的同一天
            GenerateDays(); // 重新生成日期列表
        }

        /// <summary>
        /// 跳转到后一周期
        /// </summary>
        [RelayCommand]
        private void NextWeek()
        {
            SelectedDate=SelectedDate.AddDays(7);
            _startDate = _startDate.AddDays(7);
            GenerateDays();
        }

        #endregion


    }

    // 每一天的模型
    public partial class DayItem : ObservableObject
    {
        public DateTime Date { get; set; }

        [ObservableProperty]
        private string _dayText; // "29", "30", "03"

        [ObservableProperty]
        private bool _isSelected;

    }

}
