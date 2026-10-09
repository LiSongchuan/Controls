using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Controls.ViewModels
{

    /*
     * TasksViewModel (主页面 VM，调度中心)
                    ├── DateSelectorViewModel (顶部日期 VM) -> 控件绑定它
                    ├── ObservableCollection<ScheduleItem> TodaySchedules (左侧数据源) -> 控件绑定它
                    └── ObservableCollection<TodoItem> TodoItems (右侧数据源) -> 控件绑定它
     */
    public partial class TasksViewModel : ObservableObject
    {
        // 数据源集合
        public ObservableCollection<ScheduleItem> TodaySchedules { get; } = new();
        public ObservableCollection<TodoItem> TodoItems { get; } = new();

        //TODOItem分组的集合视图: 右侧待办事项列表需要分组显示，按日期分组
        public ICollectionView GroupedTodos { get; }

        public TasksViewModel()
        {
            // 1. 初始化分组视图 (按 PriorityText 字段分组)
            GroupedTodos = CollectionViewSource.GetDefaultView(TodoItems);
            GroupedTodos.GroupDescriptions.Add(new PropertyGroupDescription("PriorityText"));

            // 2. 初始化 Mock 数据
            LoadDataForDate(DateTime.Today);

            // 3. 监听日期选择变化，实现联动，选择对应的日期，加载对应的日程和待办事项
            // 这里使用 CommunityToolkit 的属性变更监听，也可以替换成 WeakReferenceMessenger
            DateSelector.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(DateSelectorViewModel.SelectedDate))
                {
                    LoadDataForDate(DateSelector.SelectedDate);
                }
            };
        }

        #region 属性字段

        // 进度统计
        [ObservableProperty] private int _totalCount;
        [ObservableProperty] private int _completedCount;
        [ObservableProperty] private string _progressText = "0 / 0";

        // 顶部日期选择器（直接内聚进来，供 View 绑定）
        public DateSelectorViewModel DateSelector { get; } = new();

        #endregion


        #region Command

        /// <summary>
        /// 点击创建按钮
        /// </summary>
        [RelayCommand]
        private void CreateTask()
        {
            // TODO: 弹出新建待办/日程的窗口
            MessageBox.Show("点击了创建按钮，弹出新建窗口");
        }
    

        #endregion



        #region 方法
        /// <summary>
        /// 根据指定日期加载左右两边的数据
        /// </summary>
        private void LoadDataForDate(DateTime date)
        {
            LoadMockSchedules(date);
            LoadMockTodos(date);
            UpdateProgress();
        }

        private void LoadMockSchedules(DateTime date)
        {
            TodaySchedules.Clear();

            // 模拟：只显示今天的日程，其他日期为空
            if (date.Date == DateTime.Today)
            {
                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(9).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(10).AddMinutes(30),
                    Title = "产品展会",
                    Description = "线上",
                    Status = ScheduleStatus.Completed
                });

                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(10).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(11).AddMinutes(30),
                    Title = "项目周会",
                    Description = "线上",
                    Status = ScheduleStatus.Pending
                });

                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(15).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(16).AddMinutes(30),
                    Title = "外部需求对齐",
                    Description = "3F 会议室",
                    Status = ScheduleStatus.Preparing
                });
                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(9).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(10).AddMinutes(30),
                    Title = "产品展会",
                    Description = "线上",
                    Status = ScheduleStatus.Completed
                });

                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(10).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(11).AddMinutes(30),
                    Title = "项目周会",
                    Description = "线上",
                    Status = ScheduleStatus.Pending
                });

                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(15).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(16).AddMinutes(30),
                    Title = "外部需求对齐",
                    Description = "3F 会议室",
                    Status = ScheduleStatus.Preparing
                });
                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(9).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(10).AddMinutes(30),
                    Title = "产品展会",
                    Description = "线上",
                    Status = ScheduleStatus.Completed
                });

                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(10).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(11).AddMinutes(30),
                    Title = "项目周会",
                    Description = "线上",
                    Status = ScheduleStatus.Pending
                });

                TodaySchedules.Add(new ScheduleItem
                {
                    StartTime = DateTime.Today.AddHours(15).AddMinutes(30),
                    EndTime = DateTime.Today.AddHours(16).AddMinutes(30),
                    Title = "外部需求对齐",
                    Description = "3F 会议室",
                    Status = ScheduleStatus.Preparing
                });
            }
        }

        private void LoadMockTodos(DateTime date)
        {
            TodoItems.Clear();

            // 模拟待办数据
            AddTodo("完成PPT汇报", "紧急", DateTime.Today.AddDays(-1).AddHours(14.5));
            AddTodo("完成调研报告", "普通", DateTime.Today);
            AddTodo("完成调研报告", "普通", DateTime.Today);
            AddTodo("完成调研报告", "个人", DateTime.Today.AddDays(1));
            AddTodo("完成调研报告", "个人", DateTime.Today.AddDays(1));
        }

        private void AddTodo(string title, string priority, DateTime? dueTime)
        {
            var item = new TodoItem
            {
                Title = title,
                PriorityText = priority,
                DueTime = dueTime,
                IsCompleted = false,
                CompletedChanged = UpdateProgress // 绑定进度更新回调
            };
            TodoItems.Add(item);
        }

        /// <summary>
        /// 更新进度条和统计文字
        /// </summary>
        private void UpdateProgress()
        {
            TotalCount = TodoItems.Count;
            CompletedCount = TodoItems.Count(x => x.IsCompleted);
            ProgressText = $"{CompletedCount} / {TotalCount}";
        }

        

    #endregion

}   




    #region 数据模型

    /// <summary>
    /// 日程数据模型
    /// </summary>
    public partial class ScheduleItem : ObservableObject
    {
        // 开始时间
        public DateTime StartTime { get; set; }

        // 结束时间（截图中有个小字 -10:30）
        public DateTime EndTime { get; set; }

        // 标题
        public string Title { get; set; } = string.Empty;

        // 地点/方式说明（如：线上、3F xxx会议室）
        public string Description { get; set; } = string.Empty;

        // 状态枚举（如：线上·已结束）
        public ScheduleStatus Status { get; set; } =ScheduleStatus.Completed;    
    }


    /// <summary>
    /// 日程的状态枚举
    /// </summary>
    public enum ScheduleStatus
    {
        Completed,// 已结束
        Pending,// 进行中
        Preparing,//准备中 
    }

    /// <summary>
    /// 待办事项数据模型
    /// </summary>
    public partial class TodoItem : ObservableObject
    {
        [ObservableProperty] private bool _isCompleted;
        public string Title { get; set; } = string.Empty;
        public string PriorityText { get; set; } = "普通";
        public DateTime? DueTime { get; set; }

        // 用于通知外部 ViewModel 重新计算进度条
        public Action? CompletedChanged { get; set; }

        partial void OnIsCompletedChanged(bool value)
        {
            // 当勾选状态改变时，触发回调
            CompletedChanged?.Invoke();
        }
    }

    #endregion

}
